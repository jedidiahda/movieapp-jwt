using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public CustomerRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }
        public async Task<Customer> Get(int customerId) => await _movieDbContext.Customers.Where(s => s.Id == customerId).FirstOrDefaultAsync();
        public async Task<Customer> Get(string email) => await _movieDbContext.Customers.Where(s => s.Email == email).FirstOrDefaultAsync();

        public async Task<IEnumerable<Customer>> GetAll(int limit, int pageNumber) => await _movieDbContext.Customers.Skip(pageNumber * limit).Take(limit).ToListAsync();

        public async Task<Customer> Save(Customer customer)
        {
            _movieDbContext.Customers.Add(customer);
            await _movieDbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> Update(int customerId, Customer customer)
        {
            var saveCustomer = await _movieDbContext.Customers.Where(s => s.Id == customerId).SingleOrDefaultAsync();
            if (saveCustomer == null) return null;

            saveCustomer.Email = customer.Email;
            saveCustomer.Gender = customer.Gender;
            saveCustomer.Address = customer.Address;

            await _movieDbContext.SaveChangesAsync();
            return saveCustomer;
        }
    }
}
