using Microsoft.AspNetCore.Mvc;
using DENTISTA360_BACKEND.DTOs;
using DENTISTA360_BACKEND.Services;

namespace DENTISTA360_BACKEND.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Autentica um usuário e retorna um token JWT
        /// </summary>
        /// <param name="request">Dados de login (email e senha)</param>
        /// <returns>Token de acesso JWT</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        /// <response code="400">Dados de entrada inválidos</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var token = await _authService.AuthenticateAsync(request.Email, request.Senha);
                
                if (token == null)
                {
                    return Unauthorized(new { message = "Credenciais inválidas" });
                }

                return Ok(new LoginResponse { AccessToken = $"Bearer {token}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para o usuário: {Email}", request.Email);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
}
