using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Repository;
using MovieWebApp.Service;
using MovieWebApp.DTO;
using MovieWebApp.Exceptions;
using MovieWebApp.JwtFeatures;
using MovieWebApp.Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly AccountService _accountService;
        private ResponseDTO _responseDTO;

        public AccountController(JwtHandler jwtHandler, 
            ILoggerManager logger,IMapper mapper, IAccountRepository accountRepository,
            IConfiguration configuration,ICustomerRepository customerRepository)
        {
            _responseDTO = new ResponseDTO();
            _logger = logger;
            _accountService = new AccountService(jwtHandler,accountRepository, configuration,customerRepository,mapper);
        }

        // Register
        [HttpPost]
        public async Task<IActionResult> Post(AccountDTO accountDTO)
        {
            try
            {
                if (accountDTO == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid request");
                }
                await _accountService.Save(accountDTO);
               return Ok();
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            _logger.LogInfo("Account login");
            try
            {

                _responseDTO.result = Unauthorized();
                LoginResponseDTO loginResponseDTO = await _accountService.AuthenticateUser(loginRequestDTO);

                if(loginResponseDTO.token == String.Empty)
                {
                    _responseDTO.isSuccess = false;
                    _responseDTO.message = "User not found";
                    return _responseDTO;
                }

                _responseDTO.result = loginResponseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                _responseDTO.isSuccess = false;
                _responseDTO.message = ex.Message;
            }
            return _responseDTO;
        }


        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDTO externalAuth)
        {
            try
            {
                var token = await _accountService.ExternalLogin(externalAuth);
                return Ok(new { Token = token, IsAuthSuccessful = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }

        }

    }
}
