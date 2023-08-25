using MovieWebApp.Models;

namespace MovieWebApp.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public PaymentRepository(MovieDbContext movieDbContext)
        {
            this._movieDbContext = movieDbContext;
        }

        public async Task<Payment> Save(Payment payment)
        {
            _movieDbContext.Payments.Add(payment);
            await _movieDbContext.SaveChangesAsync();
            return payment;
        }
    }
}
