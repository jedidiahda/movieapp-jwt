

using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;
using MovieWebApp.DTO;

namespace MovieWebApp.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public SubscriptionRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task Delete(int subscriptionId)
        {

            var saveSubscription = await _movieDbContext.Subscriptions.Where(s => s.Id == subscriptionId).FirstOrDefaultAsync();

            if (saveSubscription != null)
            {
                saveSubscription.IsDeleted = true;
                await _movieDbContext.SaveChangesAsync();
            }
        }

        public async Task<Subscription?> Get(int subscriptionId)
            => await _movieDbContext.Subscriptions.Where(s => s.Id == subscriptionId).FirstOrDefaultAsync();
        

        public async Task<IEnumerable<Subscription>> GetAll()
        => await _movieDbContext.Subscriptions.Where(s=>s.IsDeleted == false).ToListAsync();
        

        public async Task<Subscription> Save(Subscription subscription)
        {
            _movieDbContext.Subscriptions.Add(subscription);
            await _movieDbContext.SaveChangesAsync();
            return subscription;
        }



        public async Task<Subscription> Update(int subscriptionId, Subscription subscription)
        {
            var saveSubscription = await _movieDbContext.Subscriptions.Where(s => s.Id == subscriptionId).FirstOrDefaultAsync();
            
            if(saveSubscription != null)
            {
                saveSubscription.Name = subscription.Name;
                saveSubscription.AtHomeDvd = subscription.AtHomeDvd;
                saveSubscription.Price = subscription.Price;
                saveSubscription.NoDvdperMonth = subscription.NoDvdperMonth;
                await _movieDbContext.SaveChangesAsync();
            }
     
            return subscription;
        }

        public async Task<CustomerSubscription> Subscribe(CustomerSubscription customerSubscription,Payment payment)
        {
            var tran = _movieDbContext.Database.BeginTransaction();
            try
            {   
                _movieDbContext.CustomerSubscriptions.Add(customerSubscription);
                await _movieDbContext.SaveChangesAsync();

                payment.CustomerSubscriptionId = customerSubscription.Id;
                _movieDbContext.Payments.Add(payment);
                await _movieDbContext.SaveChangesAsync();

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }
       
            return customerSubscription;
        }

        public async Task<CustomerSubscription?> GetAvailableScription(int customerId,DateTime date)
            => await _movieDbContext.CustomerSubscriptions
                .Where(s => s.CustomerId == customerId && s.StartDate <= date && date <= s.EndDate).FirstOrDefaultAsync(); 
        

        public async Task<IEnumerable<DvdStatusDTO>> GetCustomerDvdStatus(int customerSubId)
        {
            var list = await (from d in _movieDbContext.Dvdstatuses
                        join cat in _movieDbContext.Dvdcatalogs on d.DvdcatalogId equals cat.Id
                        where d.CustomerSubscriptionId == customerSubId
                        select new { d, cat }).ToListAsync();

            var dtos = new List<DvdStatusDTO>();
            foreach (var d in list)
            {
                var dvd = new DvdStatusDTO();
                dvd.customerSubscriptionId = d.d.CustomerSubscriptionId;
                dvd.DeliveredDate = d.d.DeliveredDate;
                dvd.ReturnedDate = d.d.ReturnedDate;
                dvd.Title = d.cat.Title;
                dtos.Add(dvd);
            }

            return dtos;
        }

    }
}
