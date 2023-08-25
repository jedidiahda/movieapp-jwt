using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface IWatchlistRepository
    {
        public Task<Watchlist> Save(Watchlist watchlist);
        public Task<Watchlist> Update(Watchlist watchlist);
        public Task<IEnumerable<Watchlist>> GetAll(int customerId);
        public Task<Watchlist?> Get(int customerId,int dvdCatalogId);
        public Task Delete(int customerId, int dvdCatalogId);

    }
}
