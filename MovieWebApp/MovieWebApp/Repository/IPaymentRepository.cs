using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public interface IPaymentRepository
    {
        public Task<Payment> Save(Payment payment);
    }
}
