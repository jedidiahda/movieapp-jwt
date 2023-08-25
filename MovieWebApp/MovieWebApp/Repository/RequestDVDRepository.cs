using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public class RequestDVDRepository : IRequestDVDRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public RequestDVDRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }
        public async Task Delete(int requestId)
        {
            var request = await _movieDbContext.RequestedDvds.Where(s=>s.Id == requestId).SingleOrDefaultAsync();
            if (request == null) throw new Exception("Request not found");
            _movieDbContext.RequestedDvds.Remove(request);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RequestedDvd>> GetAll(int customerId)
            => await _movieDbContext.RequestedDvds.Where(s=>s.CustomerId==customerId).ToListAsync();
        

        public async Task<RequestedDvd> Save(RequestedDvd requestedDvd)
        {
            _movieDbContext.RequestedDvds.Add(requestedDvd);
            await _movieDbContext.SaveChangesAsync();
            return requestedDvd;
        }
    }
}
