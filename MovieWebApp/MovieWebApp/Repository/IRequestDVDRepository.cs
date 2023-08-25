using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface IRequestDVDRepository
    {
        Task<RequestedDvd> Save(RequestedDvd requestedDvd);
        Task<IEnumerable<RequestedDvd>> GetAll(int customerId);
        Task Delete(int requestId);
    }
}
