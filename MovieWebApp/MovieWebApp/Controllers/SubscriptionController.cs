using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Repository;
using MovieWebApp.Service;
using MovieWebApp.DTO;
using AutoMapper;
using MovieWebApp.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MovieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _SubscriptionService;
        private readonly ILoggerManager _logger;
        public SubscriptionController(IMapper mapper, ILoggerManager logger, ISubscriptionRepository subscriptionRepository)
        {
            _logger = logger;
            _SubscriptionService = new SubscriptionService(mapper, subscriptionRepository);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _SubscriptionService.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }

        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> Get(int subscriptionId)
        {
            try
            {
                return Ok(await _SubscriptionService.GetSubscription(subscriptionId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post(SubscriptionDTO subscriptionDTO)
        {
            if (subscriptionDTO == null)
            {
                return BadRequest("Invalid request object");
            }
            try
            {
                await _SubscriptionService.Save(subscriptionDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }


        [HttpPut("{subscriptionId}")]
        public async Task<IActionResult> Put(int subscriptionId,SubscriptionDTO subscriptionDTO)
        {
            try
            {
                await _SubscriptionService.Update(subscriptionId, subscriptionDTO);
                return Ok(subscriptionDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }

        [HttpDelete("{subscriptionId}")]
        public async Task<IActionResult> Delete(int subscriptionId)
        {
            try
            {
                var subscription = await _SubscriptionService.GetSubscription(subscriptionId);
                if (subscription == null)
                {
                    throw new NotFoundException("Subscription not found");
                }

                await _SubscriptionService.Delete(subscriptionId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("subscribe")]
        public async Task<IActionResult> Subcribe(CustomerSubscriptionDTO customerSubscriptionDTO)
        {

            if (customerSubscriptionDTO == null)
            {
                return BadRequest("Invalid request object");
            }

            try
            {
                return Ok(await _SubscriptionService.Subscribe(customerSubscriptionDTO));
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAvailableScription")]
        public async Task<IActionResult> GetAvailableScription(int customerId,DateTime date)
        {
            try
            {
                var subscriptions = await _SubscriptionService.GetAvailableScription(customerId, date);
                if (subscriptions == null) return Ok();

                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetCustomerDvdStatus")]
        public async Task<IActionResult> GetCustomerDVDStatus(int customerSubId)
        {
            try
            {
                return Ok(await _SubscriptionService.GetCustomerDvdStatus(customerSubId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }
    }
}
