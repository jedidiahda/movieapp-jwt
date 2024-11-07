using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface IAccountRepository
    {
        Task<Account> Save(Account account);
        Task<Account?> Get(string email,string password);
        Task<Account?> Get(string email);

    }
}
