using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Integration.Contracts;
using MovieWebApp.Repository;
using MovieWebApp.Service;
using MovieWebApp.DTO;
using AutoMapper;
using MovieWebApp.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class DVDCatalogController : ControllerBase
    {
        private readonly DVDCatalogService _dVDCatalogService;
        private readonly ILoggerManager _logger;
        public DVDCatalogController(IMapper mapper, ILoggerManager logger, IDVDCatalogRepository dVDCatalogRepository)
        {
            _logger = logger;
            _dVDCatalogService = new DVDCatalogService(mapper,dVDCatalogRepository);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? title, int limit = 10, int pageNumber = 1)
        {
            try
            {
                return Ok(await _dVDCatalogService.GetAll(title ?? "", limit, pageNumber));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        // GET api/<DVDCatalogController>/5
        [HttpGet("{DVDCatalogId}")]
        public async Task<IActionResult> Get(int DVDCatalogId)
        {
            try
            {
                return Ok(await _dVDCatalogService.GetById(DVDCatalogId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex); 
                throw new InternalServerException(ex.Message);
            }
            
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post(DVDCatalogDTO dVDCatalogDTO)
        {
            if (dVDCatalogDTO == null)
            {
                return BadRequest("Invalid request object");
            }

            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "DVDCatalogs",new Guid().ToString());
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //return Ok(new { dbPath });
                }

                dVDCatalogDTO.fileName = folderName;

                await _dVDCatalogService.Save(dVDCatalogDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
           
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(int DVDCatalogId)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "DVDCatalogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var dvd = await _dVDCatalogService.GetById(DVDCatalogId);
                    if (dvd != null)
                    {
                        var existingFile = Path.Combine(pathToSave, dvd.fileName);
                        if (System.IO.File.Exists(existingFile))
                        {
                            System.IO.File.Delete(existingFile);
                        }
                    }

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    FileInfo fileInfo = new FileInfo(fullPath);
                    fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                    System.IO.File.Move(fullPath, Path.Combine(pathToSave,fileName));
                    fullPath = Path.Combine(pathToSave, fileName);
                    var dVDCatalogDTO = new DVDCatalogDTO
                    {
                        Id = DVDCatalogId,
                        fileName = fileName
                    };
                    await _dVDCatalogService.UpdateDVDFileUrl(DVDCatalogId, dVDCatalogDTO);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }


        [HttpPut("{DVDCatalogId}")]
        public async Task<IActionResult> Put(int DVDCatalogId, DVDCatalogDTO dVDCatalogDTO)
        {
            if(dVDCatalogDTO == null)
            {
                return BadRequest("Invalid request object");
            }

            try
            { 
                await _dVDCatalogService.Update(DVDCatalogId, dVDCatalogDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
          
        }

        [HttpDelete("{dvdCatalogId}")]
        public async Task<IActionResult> Delete(int dvdCatalogId)
        {
            try
            {
                if (await _dVDCatalogService.GetById(dvdCatalogId) == null)
                {
                    throw new NotFoundException("DVD Catalog not found");
                }

                await _dVDCatalogService.Delete(dvdCatalogId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpGet]
        [Route("{dvdCatalogId}/dvdcopy")]
        public async Task<IActionResult> GetDvdCopies(int dvdCatalogId, string? code, int limit,int pageNumber)
        {
            try
            {
                return Ok(await _dVDCatalogService.GetDVDCopies(dvdCatalogId,code ?? "", limit, pageNumber));
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpGet]
        [Route("{dvdCatalogId}/dvdcopy/{code}")]
        public async Task<IActionResult> GetDvdCopy(string code)
        {
            try
            {
                return Ok(await _dVDCatalogService.GetDvdcopy(code));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
           
        }

        [HttpPost]
        [Route("{dvdCatalogId}/dvdcopy")]
        public async Task<IActionResult> AddDvdCopy(int dvdCatalogId,DvdcopyDTO dvdcopyDTO)
        {
            try
            {
                await _dVDCatalogService.AddDVDCopy(dvdCatalogId, dvdcopyDTO);
                return Ok();
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpPut]
        [Route("dvdcopy/{dvdCopyId}")]
        public async Task<IActionResult> UpdateDvdCopy(int dvdCopyId,DvdcopyDTO dvdcopyDTO)
        {
            try
            {
                if(dvdcopyDTO == null)
                {
                    return BadRequest("Invalid request object");
                }

                var dvdCopy = await _dVDCatalogService.UpdateDvdCopy(dvdCopyId, dvdcopyDTO);
                if (dvdCopy != null) return Ok();
                else throw new NotFoundException("DVD code not found");
            }catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new InternalServerException(ex.Message);
            }
        }

        [HttpDelete]
        [Route("dvdcopy/{dvdCopyId}")]
        public async Task<IActionResult> DeleteDvdCopy(int dvdCopyId)
        {
            try
            {
                if(await _dVDCatalogService.GetDvdcopy(dvdCopyId) == null)
                {
                    throw new NotFoundException("Dvd code not found");
                }
                await _dVDCatalogService.DeleteDVDCopy(dvdCopyId);
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
