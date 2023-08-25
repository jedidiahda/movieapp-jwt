using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDTO>();
        }
    }
}
