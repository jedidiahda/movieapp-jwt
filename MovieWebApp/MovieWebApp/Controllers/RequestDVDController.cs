using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Models;
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
    public class RequestDVDController : ControllerBase
    {
        private readonly RequestDVDService _requestDVDService;
        private readonly ILoggerManager _logger;
        public RequestDVDController(IMapper mapper, ILoggerManager logger, IRequestDVDRepository requestDVDRepository)
        {
            _logger = logger;
            _requestDVDService = new RequestDVDService(mapper,requestDVDRepository);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int customerId)
        {
            try
            {
                return Ok(await _requestDVDService.GetAll(customerId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int requestId)
        {
            try
            {
                await _requestDVDService.Delete(requestId);
                return Ok();
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestedDvdDTO requestedDvd)
        {
            try
            {
                return Ok(await _requestDVDService.Save(requestedDvd));
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }
    }

   
}
