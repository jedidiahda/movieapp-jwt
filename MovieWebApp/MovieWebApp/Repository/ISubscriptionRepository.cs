using MovieWebApp.Models;
using MovieWebApp.DTO;

namespace MovieWebApp.Repository
{
    public interface ISubscriptionRepository
    {
        public Task<Subscription> Save(Subscription subscription);
        public Task<IEnumerable<Subscription>> GetAll();

        public Task<Subscription> Update(int subscriptionId, Subscription subscription);
        public Task<Subscription?> Get(int subscriptionId);
        public Task Delete(int subscriptionId);

        public Task<CustomerSubscription> Subscribe(CustomerSubscription customerSubscription, Payment payment);
        public Task<CustomerSubscription?> GetAvailableScription(int customerId, DateTime date);
        public Task<IEnumerable<DvdStatusDTO>> GetCustomerDvdStatus(int customerSubId);
        
    }
}
