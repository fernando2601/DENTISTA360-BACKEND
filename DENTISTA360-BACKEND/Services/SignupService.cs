using Dapper;
using DENTISTA360_BACKEND.Data;
using DENTISTA360_BACKEND.DTOs;
using DENTISTA360_BACKEND.Models;
using DENTISTA360_BACKEND.Services;

namespace DENTISTA360_BACKEND.Services
{
    public class SignupService : ISignupService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IAuthService _authService;

        public SignupService(IDbConnectionFactory dbConnectionFactory, IAuthService authService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _authService = authService;
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest request)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Verificar se email ou CPF já existem
                var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
                    @"SELECT ""Id"" FROM ""User"" WHERE ""Email"" = @Email OR ""CPF"" = @CPF",
                    new { Email = request.PersonalData.Email, CPF = request.PersonalData.CPF },
                    transaction
                );

                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email ou CPF já cadastrado");
                }

                // 2. Verificar se CNPJ já existe
                var existingClinic = await connection.QueryFirstOrDefaultAsync<Clinica>(
                    @"SELECT ""Id"" FROM ""Clinica"" WHERE ""CNPJ"" = @CNPJ",
                    new { CNPJ = request.CompanyData.CNPJ },
                    transaction
                );

                if (existingClinic != null)
                {
                    throw new InvalidOperationException("CNPJ já cadastrado");
                }

                // 3. Criar endereço da clínica
                var clinicAddressId = await connection.QuerySingleAsync<int>(
                    @"INSERT INTO ""Endereco"" (""Logradouro"", ""Numero"", ""Bairro"", ""Cidade"", ""Estado"", ""CEP"", ""Complemento"")
                      VALUES (@Logradouro, @Numero, @Bairro, @Cidade, @Estado, @CEP, @Complemento)
                      RETURNING ""Id""",
                    new
                    {
                        Logradouro = request.CompanyData.Logradouro,
                        Numero = request.CompanyData.Numero,
                        Bairro = request.CompanyData.Bairro,
                        Cidade = request.CompanyData.Cidade,
                        Estado = request.CompanyData.Estado,
                        CEP = request.CompanyData.CEP,
                        Complemento = (string?)null
                    },
                    transaction
                );

                // 4. Criar clínica
                var clinicId = await connection.QuerySingleAsync<int>(
                    @"INSERT INTO ""Clinica"" (""NomeFantasia"", ""RazaoSocial"", ""CNPJ"", ""EnderecoId"", ""Phone"", ""Email"", ""NomeResponsavel"")
                      VALUES (@NomeFantasia, @RazaoSocial, @CNPJ, @EnderecoId, @Phone, @Email, @NomeResponsavel)
                      RETURNING ""Id""",
                    new
                    {
                        NomeFantasia = request.CompanyData.NomeFantasia,
                        RazaoSocial = request.CompanyData.RazaoSocial,
                        CNPJ = request.CompanyData.CNPJ,
                        EnderecoId = clinicAddressId,
                        Phone = (string?)null,
                        Email = (string?)null,
                        NomeResponsavel = request.PersonalData.NomeCompleto
                    },
                    transaction
                );

                // 5. Hash da senha
                var hashedPassword = _authService.HashPassword(request.PersonalData.Senha);

                // 6. Criar usuário (sem endereço por enquanto)
                var userId = await connection.QuerySingleAsync<int>(
                    @"INSERT INTO ""User"" (""Nome"", ""EnderecoId"", ""Phone"", ""Email"", ""Cargo"", ""Senha"", ""CPF"")
                      VALUES (@Nome, @EnderecoId, @Phone, @Email, @Cargo, @Senha, @CPF)
                      RETURNING ""Id""",
                    new
                    {
                        Nome = request.PersonalData.NomeCompleto,
                        EnderecoId = (int?)null,
                        Phone = request.PersonalData.Telefone,
                        Email = request.PersonalData.Email,
                        Cargo = (string?)null,
                        Senha = hashedPassword,
                        CPF = request.PersonalData.CPF
                    },
                    transaction
                );

                // 7. Associar usuário com clínica como diretor
                await connection.ExecuteAsync(
                    @"INSERT INTO user_clinic (user_id, clinic_id, group_type)
                      VALUES (@UserId, @ClinicId, @GroupType)",
                    new
                    {
                        UserId = userId,
                        ClinicId = clinicId,
                        GroupType = "director"
                    },
                    transaction
                );

                transaction.Commit();

                return new SignupResponse
                {
                    UserId = userId,
                    ClinicId = clinicId,
                    Message = "Cadastro realizado com sucesso"
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

