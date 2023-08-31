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

        public AccountController(JwtHandler jwtHandler, 
            ILoggerManager logger,IMapper mapper, IAccountRepository accountRepository,
            IConfiguration configuration,ICustomerRepository customerRepository)
        {
            _logger = logger;
            _accountService = new AccountService(jwtHandler,accountRepository, configuration,customerRepository,mapper);
        }

        // POST api/<AccountController>
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
        public async Task<IActionResult> Login(AccountDTO accountDTO)
        {
            _logger.LogInfo("Account login");
            try
            {

                IActionResult response = Unauthorized();
                var token = await _accountService.AuthenticateUser(accountDTO);

                if (token != string.Empty)
                {
                    response = Ok(new { token = token });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }

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
