using MovieWebApp.Models;
using MovieWebApp.Repository;
using MovieWebApp.DTO;
using AutoMapper;

namespace MovieWebApp.Service
{
    public class WatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMapper _mapper;

        public WatchlistService(IMapper mapper, IWatchlistRepository watchlistRepository)
        {
            _mapper = mapper;
            _watchlistRepository = watchlistRepository;
        }

        public async Task<WatchlistDTO> Get(int customerId, int dvdCatalogId)
        {
            var dvdList = await _watchlistRepository.Get(customerId, dvdCatalogId);
            return _mapper.Map<WatchlistDTO>(await _watchlistRepository.Get(customerId, dvdCatalogId));
        }
        public async Task<IEnumerable<WatchlistDTO>> GetAll(int customerId)
        {
            var watchlist = await _watchlistRepository.GetAll(customerId);
            return _mapper.Map<List<WatchlistDTO>>(watchlist);
        }
        public async Task<WatchlistDTO> Save(WatchlistDTO watchlistDto)
        {
            return _mapper.Map<WatchlistDTO>(await _watchlistRepository.Save(_mapper.Map<Watchlist>(watchlistDto)));
        }

        public async Task<WatchlistDTO> Update(WatchlistDTO watchlistDTO)
        {
            return _mapper.Map<WatchlistDTO>(await _watchlistRepository.Update(_mapper.Map<Watchlist>(watchlistDTO)));
        }

        public async Task Delete(int customerId,int dvdCopyId)
        {
            await _watchlistRepository.Delete(customerId, dvdCopyId);
        }
    }
}
