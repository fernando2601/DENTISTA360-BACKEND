using Dapper;
using DENTISTA360_BACKEND.Data;
using DENTISTA360_BACKEND.Models;

namespace DENTISTA360_BACKEND.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT u.""Id"", u.""Nome"", u.""EnderecoId"", u.""Phone"", u.""Email"", u.""Cargo"", u.""Senha"", u.""CPF""
                FROM ""User"" u
                WHERE u.""Email"" = @Email";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT u.""Id"", u.""Nome"", u.""EnderecoId"", u.""Phone"", u.""Email"", u.""Cargo"", u.""Senha"", u.""CPF""
                FROM ""User"" u
                WHERE u.""Id"" = @Id";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<List<Clinica>> GetUserClinicsAsync(int userId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT c.""Id"", c.""NomeFantasia"", c.""RazaoSocial"", c.""CNPJ"", c.""EnderecoId"", c.""Phone"", c.""Email"", c.""NomeResponsavel""
                FROM ""Clinica"" c
                INNER JOIN user_clinic uc ON c.""Id"" = uc.clinic_id
                WHERE uc.user_id = @UserId";

            var result = await connection.QueryAsync<Clinica>(sql, new { UserId = userId });
            return result.ToList();
        }

        public async Task<string?> GetUserPermissionInClinicAsync(int userId, int clinicId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT uc.group_type
                FROM user_clinic uc
                WHERE uc.user_id = @UserId AND uc.clinic_id = @ClinicId";

            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { UserId = userId, ClinicId = clinicId });
        }
    }
}
