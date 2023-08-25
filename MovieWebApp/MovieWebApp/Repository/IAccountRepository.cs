using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface IAccountRepository
    {
        public Task<Account> Save(Account account);
        public Task<Account> Get(string email,string password);
    }
}
