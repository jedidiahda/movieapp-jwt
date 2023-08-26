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
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;

        public AccountController(JwtHandler jwtHandler, 
            ILoggerManager logger,IMapper mapper, IAccountRepository accountRepository,
            IConfiguration configuration,ICustomerRepository customerRepository)
        {
            _jwtHandler = jwtHandler;
            _logger = logger;
            _mapper = mapper;
            _accountService = new AccountService(jwtHandler,accountRepository, configuration,customerRepository,mapper);
            _customerService = new CustomerService(mapper, customerRepository);
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<IActionResult> Post(AccountDTO accountDTO)
        {
            try
            {
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
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            try
            {
                var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
                if (payload == null)
                    return BadRequest("Invalid External Authentication.");

                var user = await _accountService.Get(payload.Email);
                if (user == null)
                {
                    user = new AccountDTO
                    {
                        Email = payload.Email,
                        Role = Role.USER,
                        Password = ""
                    };

                    await _accountService.Save(user);

                    var customer = new CustomerDTO
                    {
                        Email = payload.Email,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        Gender= "M",
                        Address = ""
                    };
                    await _customerService.Save(customer);
                }

                if (user == null)
                    return BadRequest("Invalid External Authentication.");

                //check for the Locked out account

                var token = await _jwtHandler.GenerateToken(user);

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
