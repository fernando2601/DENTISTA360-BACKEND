using Microsoft.AspNetCore.Mvc;
using DENTISTA360_BACKEND.DTOs;
using DENTISTA360_BACKEND.Services;

namespace DENTISTA360_BACKEND.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    public class SignupController : ControllerBase
    {
        private readonly ISignupService _signupService;
        private readonly ILogger<SignupController> _logger;

        public SignupController(ISignupService signupService, ILogger<SignupController> logger)
        {
            _signupService = signupService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza o cadastro de um novo usuário e clínica
        /// </summary>
        /// <param name="request">Dados pessoais e da empresa</param>
        /// <returns>ID do usuário e clínica criados</returns>
        /// <response code="200">Cadastro realizado com sucesso</response>
        /// <response code="400">Dados de entrada inválidos ou email/CPF/CNPJ já cadastrados</response>
        [HttpPost("signup")]
        [ProducesResponseType(typeof(SignupResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _signupService.SignupAsync(request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Tentativa de cadastro com dados já existentes");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o cadastro");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
}

