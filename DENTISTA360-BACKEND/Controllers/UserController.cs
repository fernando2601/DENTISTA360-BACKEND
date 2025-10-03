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
        /// Obtém informações do usuário logado e suas clínicas
        /// </summary>
        /// <returns>Informações do usuário e clínicas associadas</returns>
        /// <response code="200">Informações obtidas com sucesso</response>
        /// <response code="401">Token inválido ou expirado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("info")]
        [ProducesResponseType(typeof(UserInfoResponse), StatusCodes.Status200OK)]
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

                var response = new UserInfoResponse
                {
                    User = new UserInfo
                    {
                        Nome = user.Nome
                    },
                    Clinicas = clinicas.Select(c => new ClinicaInfo
                    {
                        Id = c.Id,
                        NomeFantasia = c.NomeFantasia
                    }).ToList()
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
