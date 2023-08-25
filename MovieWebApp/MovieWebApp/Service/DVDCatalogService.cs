using MovieWebApp.Repository;
using MovieWebApp.DTO;
using AutoMapper;
using MovieWebApp.Models;

namespace MovieWebApp.Service
{
    public class DVDCatalogService
    {
        private readonly IDVDCatalogRepository _dVDCatalogRepository;
        private readonly IMapper _mapper;

        public DVDCatalogService(IMapper mapper, IDVDCatalogRepository dVDCatalogRepository)
        {
            _mapper = mapper;
            _dVDCatalogRepository = dVDCatalogRepository;
        }

        public async Task<DVDCatalogDTO> Save(DVDCatalogDTO dvdcatalogDTO)
        {
            var dvdCatalog = await _dVDCatalogRepository.Save(_mapper.Map<Dvdcatalog>(dvdcatalogDTO));
            return _mapper.Map<DVDCatalogDTO>(dvdCatalog);
        }
        public async Task<DVDCatalogDTO> GetById(int id)
        {
            return _mapper.Map<DVDCatalogDTO>(await _dVDCatalogRepository.GetById(id));
        }
        public async Task<DVDCatalogDTO> Update(int dvdCatalogId, DVDCatalogDTO dvdcatalogDTO)
        {
            var dvdCatalog = await _dVDCatalogRepository.Update(dvdCatalogId, _mapper.Map<Dvdcatalog>(dvdcatalogDTO));
            return _mapper.Map<DVDCatalogDTO>(dvdCatalog);
        }

        public async Task<IEnumerable<DVDCatalogDTO>> GetAll(string title, int limit, int pageNumber)
        {
            var dvdList = await _dVDCatalogRepository.GetAll(title, limit, pageNumber);
            return _mapper.Map<List<DVDCatalogDTO>>(dvdList);
        }

        public async Task Delete(int dvdCatalogId)
        {
            var dvdCatalog = await _dVDCatalogRepository.GetById(dvdCatalogId);
            if (dvdCatalog == null) throw new Exception("DVD catalog not found");

            await _dVDCatalogRepository.Delete(dvdCatalog);
        }

        public async Task AddDVDCopy(int dvdCatalogId, DvdcopyDTO dvdcopy)
        {
            await _dVDCatalogRepository.AddDVDCopy(dvdCatalogId,_mapper.Map<Dvdcopy>(dvdcopy));
        }

        public async Task<IEnumerable<DvdcopyDTO>> GetDVDCopies(int dvdCatalogId,string code, int limit, int pageNumber)
        {
            var dvdCopies = await _dVDCatalogRepository.GetDVDCopies(dvdCatalogId, code, limit, pageNumber);
            return _mapper.Map<List<DvdcopyDTO>>(dvdCopies);
        }

        public async Task<DvdcopyDTO?> GetDvdcopy(string code)
        {
            var dvdCopy = await _dVDCatalogRepository.GetDvdcopy(code);
            if (dvdCopy == null) throw new Exception("DVD copy not found");
            else
                return _mapper.Map<DvdcopyDTO>(dvdCopy);
        }

        public async Task<DvdcopyDTO?> GetDvdcopy(int dvdCopyId)
        {
            var dvdCopy = await _dVDCatalogRepository.GetDvdcopy(dvdCopyId);
            if (dvdCopy == null) throw new Exception("DVD copy not found");
            else
                return _mapper.Map<DvdcopyDTO>(dvdCopy);
        }

        public async Task DeleteDVDCopy(int dvdCopyId)
        {
            await _dVDCatalogRepository.DeleteDVDCopy(dvdCopyId);
        }

        public async Task<DvdcopyDTO?> UpdateDvdCopy(int id, DvdcopyDTO dvdcopyDTO)
        {
            var dvdCopy = await _dVDCatalogRepository.UpdateDvdCopy(id,_mapper.Map<Dvdcopy>(dvdcopyDTO));
            return _mapper.Map<DvdcopyDTO>(dvdCopy);
        }
    }
}
