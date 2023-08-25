using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Exceptions;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Repository;
using MovieWebApp.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class CustomerDeliveryController : ControllerBase
    {
        private readonly CustomerDeliveryService _customerDeliveryService;
        private readonly ILoggerManager _logger;
        public CustomerDeliveryController(IMapper mapper, ILoggerManager logger, ICustomerDeliveryRepository customerDeliveryRepository)
        {
            _logger = logger;
            _customerDeliveryService = new CustomerDeliveryService(mapper, customerDeliveryRepository);
        }

        // GET: api/<CustomerDeliveryController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _customerDeliveryService.GetValidCustomerDelivery());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpPost]
        [Route("deliver")]
        public async Task<IActionResult> DeliveryToCustomer(int subscriptionId, string code,int dvdCatalogId)
        {
            try
            {
                await _customerDeliveryService.SendDvdToCustomer(subscriptionId, code, dvdCatalogId);
                return Ok();
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }


        [HttpPost]
        [Route("return")]
        public async Task<IActionResult> ReturnDVD(int subscriptionId, string code, int dvdCatalogId)
        {
            try
            {
                await _customerDeliveryService.ReturnDVDFromCustomer(subscriptionId, code, dvdCatalogId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpGet]
        [Route("DVdStatus")]
        public async Task<IActionResult> GetDVDStatus()
        {
            try
            {
                return Ok(await _customerDeliveryService.GetDvdstatuses());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }

    }
}
