using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Domain.Repositories;

namespace DENTISTA360_BACKEND.Controllers
{
    [ApiController]
    [Route("clinica")]
    [Authorize]
    [Produces("application/json")]
    public class ClinicController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ClinicController> _logger;

        public ClinicController(IUserRepository userRepository, ILogger<ClinicController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Obtém as permissões do usuário logado em uma clínica específica
        /// </summary>
        /// <param name="clinicaId">ID da clínica</param>
        /// <returns>Permissão do usuário na clínica</returns>
        /// <response code="200">Permissão obtida com sucesso</response>
        /// <response code="401">Token inválido ou expirado</response>
        /// <response code="403">Usuário não tem acesso à clínica</response>
        /// <response code="404">Clínica não encontrada</response>
        [HttpGet("{clinicaId}/permissions")]
        [ProducesResponseType(typeof(ClinicPermissionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClinicPermissions(int clinicaId)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Token inválido" });
                }

                var permission = await _userRepository.GetUserPermissionInClinicAsync(userId, clinicaId);
                
                if (string.IsNullOrEmpty(permission))
                {
                    return Forbid("Usuário não tem acesso à clínica especificada");
                }

                // Map the permission from database to a more user-friendly format
                var mappedPermission = MapPermissionToUserFriendlyName(permission);

                return Ok(new ClinicPermissionResponse { Permission = mappedPermission });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter permissões da clínica {ClinicaId} para usuário {UserId}", 
                    clinicaId, User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        private static string MapPermissionToUserFriendlyName(string permission)
        {
            return permission.ToLower() switch
            {
                "employee" => "Funcionário",
                "director" => "Diretor",
                "doctor" => "Médico",
                _ => permission
            };
        }
    }
}
