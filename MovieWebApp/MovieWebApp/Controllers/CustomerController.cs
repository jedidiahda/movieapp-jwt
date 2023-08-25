using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Repository;
using MovieWebApp.Service;
using MovieWebApp.DTO;
using AutoMapper;
using MovieWebApp.Exceptions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly ILoggerManager _logger;

        public CustomerController(IMapper mapper, ILoggerManager logger, ICustomerRepository customerRepository)
        {
            _customerService = new CustomerService(mapper, customerRepository);
            _logger = logger;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{customerId}")]
        [Authorize]
        public async Task<IActionResult> Get(int customerId)
        {
            try
            {
                return Ok(await _customerService.Get(customerId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> Post(CustomerDTO customerDTO)
        {
            _logger.LogInfo("Post customer " + customerDTO.ToString());
            try
            {
                await _customerService.Save(customerDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{customerId}")]
        [Authorize]
        public async Task<IActionResult> Put(int customerId, CustomerDTO customerDTO)
        {
            _logger.LogInfo("Put customer " + customerDTO.ToString());
            try
            {
                await _customerService.Update(customerId, customerDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }


    }
}
