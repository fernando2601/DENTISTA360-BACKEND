using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DENTISTA360_BACKEND.DTOs;
using DENTISTA360_BACKEND.Repositories;

namespace DENTISTA360_BACKEND.Controllers
{
    [ApiController]
    [Route("usuarios")]
    [Authorize]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtém informações do usuário logado e clínica base
        /// </summary>
        /// <returns>Informações do usuário e clínica base associada</returns>
        /// <response code="200">Informações obtidas com sucesso</response>
        /// <response code="401">Token inválido ou expirado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("info")]
        [ProducesResponseType(typeof(ClinicBaseInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                var clinicas = await _userRepository.GetUserClinicsAsync(userId);
                
                // Retorna a primeira clínica ou uma clínica padrão
                var primeiraClinica = clinicas.FirstOrDefault();

                var response = new ClinicBaseInfoResponse
                {
                    User = new UserInfo
                    {
                        Nome = user.Nome
                    },
                    ClinicBaseInfo = primeiraClinica != null 
                        ? new ClinicBasicInfo
                        {
                            Id = primeiraClinica.Id.ToString(),
                            NomeFantasia = primeiraClinica.NomeFantasia
                        }
                        : new ClinicBasicInfo
                        {
                            Id = "0",
                            NomeFantasia = "Nenhuma clínica associada"
                        }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter informações do usuário: {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
}
