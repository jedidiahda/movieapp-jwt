using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public AccountRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task<Account?> Get(string email,string password) => await _movieDbContext.Accounts.Where(s => s.Email == email && s.Password == password).SingleOrDefaultAsync();

        public async Task<Account?> Get(string email) => await _movieDbContext.Accounts.Where(s => s.Email == email).SingleOrDefaultAsync();

        public async Task<Account> Save(Account account)
        {
            account.Active = true;
            _movieDbContext.Accounts.Add(account);
            await _movieDbContext.SaveChangesAsync();
            return account;
        }
    }
}
