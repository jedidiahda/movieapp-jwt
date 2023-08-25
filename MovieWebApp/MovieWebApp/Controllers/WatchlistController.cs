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
    [ApiController, Authorize]
    public class WatchlistController : ControllerBase
    {
        private readonly WatchlistService _watchlistService;
        private readonly ILoggerManager _logger;
        public WatchlistController(IMapper mapper,ILoggerManager logger, IWatchlistRepository watchlistRepository)
        {
            _logger = logger;   
            _watchlistService = new WatchlistService(mapper,watchlistRepository);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Get(int customerId)
        {
            try
            {
                return Ok(await _watchlistService.GetAll(customerId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        // GET api/<WatchlistController>/5
        [HttpGet]
        public async Task<IActionResult> Get(int customerId,int watchlistId)
        {
            try
            {
                return Ok(await _watchlistService.Get(customerId, watchlistId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
            
        }

        // POST api/<WatchlistController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WatchlistDTO watchlistDTO)
        {
            try
            {
                return Ok(await _watchlistService.Save(watchlistDTO));
            }catch (Exception ex)
            {
                _logger.LogInfo("Post watchlist " + watchlistDTO.ToString());
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        // PUT api/<WatchlistController>/5
        [HttpPut]
        [Route("UpdateRank")]
        public async Task<IActionResult> Put(int customerId,int dvdcatalogId, int rank)
        {
            try
            {
                var watchlistDTO = new WatchlistDTO();
                watchlistDTO.CustomerId = customerId;
                watchlistDTO.DvdcatalogId = dvdcatalogId;
                watchlistDTO.Rank = rank;
                return Ok(await _watchlistService.Update(watchlistDTO));
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int customerId,int dvdCatalogId)
        {
            try
            {
                await _watchlistService.Delete(customerId, dvdCatalogId);
                return Ok();
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

    }
}
