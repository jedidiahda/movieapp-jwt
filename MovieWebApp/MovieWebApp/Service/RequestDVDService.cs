using MovieWebApp.Models;
using MovieWebApp.Repository;
using MovieWebApp.DTO;
using AutoMapper;

namespace MovieWebApp.Service
{
    public class RequestDVDService
    {
        private readonly IRequestDVDRepository _requestDVDRepository;
        private readonly IMapper _mapper;
        public RequestDVDService(IMapper mapper,IRequestDVDRepository requestDVDRepository)
        {
            _mapper = mapper;
            _requestDVDRepository = requestDVDRepository;
        }
        public  async Task Delete(int requestId)
        {
            await _requestDVDRepository.Delete(requestId);
        }

        public async Task<IEnumerable<RequestedDvd>> GetAll(int customerId)
            => await _requestDVDRepository.GetAll(customerId);
        

        public async Task<RequestedDvdDTO> Save(RequestedDvdDTO requestedDvd)
        {
            var requestDVD = await _requestDVDRepository.Save(_mapper.Map<RequestedDvd>(requestedDvd));
            return _mapper.Map<RequestedDvdDTO>(requestDVD);
        }
        
    }
}
