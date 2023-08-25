using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Repository;
using MovieWebApp.Service;
using MovieWebApp.DTO;
using MovieWebApp.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(ILoggerManager logger,IMapper mapper, IAccountRepository accountRepository,IConfiguration configuration,ICustomerRepository customerRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _accountService = new AccountService(accountRepository,configuration,customerRepository,mapper);
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
    }
}
