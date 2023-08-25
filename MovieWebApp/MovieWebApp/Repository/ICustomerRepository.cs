using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface ICustomerRepository
    {
        public Task<Customer> Save(Customer customer);
        public Task<Customer> Get(int customerId);
        public Task<Customer> Get(string email);

        public Task<Customer> Update(int customerId, Customer customer);
        public Task<IEnumerable<Customer>> GetAll(int limit,int pageNumber);
    }
}
