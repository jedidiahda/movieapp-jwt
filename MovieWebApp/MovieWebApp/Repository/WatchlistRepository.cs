using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public WatchlistRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task Delete(int customerId, int dvdCatalogId)
        {
            var dvdcopy = await _movieDbContext.Watchlists.Where(s => s.CustomerId == customerId && s.DvdcatalogId == dvdCatalogId).FirstOrDefaultAsync();
            if (dvdcopy == null) throw new Exception("Watchlist item not found");
            _movieDbContext.Watchlists.Remove(dvdcopy);
            await _movieDbContext.SaveChangesAsync(); 
        }

        public async Task<Watchlist?> Get(int customerId, int dvdCatalogId)
            => await _movieDbContext.Watchlists.Where(s => s.CustomerId == customerId && s.DvdcatalogId == dvdCatalogId).FirstOrDefaultAsync();
        

        public async Task<IEnumerable<Watchlist>> GetAll(int customerId)
        {
            return await (from w in _movieDbContext.Watchlists
                       join d in _movieDbContext.Dvdcatalogs on w.DvdcatalogId equals d.Id
                       where w.CustomerId == customerId
                       select new Watchlist
                       {
                           CustomerId = w.CustomerId,
                           Dvdcatalog = d,
                           DvdcatalogId = w.DvdcatalogId,
                           Rank = w.Rank,
                       }).ToListAsync();
        }

        public async Task<Watchlist> Save(Watchlist watchlist)
        {
            _movieDbContext.Watchlists.Add(watchlist);
            await _movieDbContext.SaveChangesAsync();
            return watchlist;
        }

        public async Task<Watchlist> Update(Watchlist watchlist)
        {
            var saveWatchlist = await Get(watchlist.CustomerId, watchlist.DvdcatalogId);
            if (saveWatchlist == null) throw new Exception("Watchlist not found");

            saveWatchlist.Rank = watchlist.Rank;
            await _movieDbContext.SaveChangesAsync();
            return saveWatchlist;
        }
    }
}
