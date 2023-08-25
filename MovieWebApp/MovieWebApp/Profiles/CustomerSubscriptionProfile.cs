using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class CustomerSubscriptionProfile:Profile
    {
        public CustomerSubscriptionProfile()
        {
            CreateMap<CustomerSubscription, CustomerSubscriptionDTO>();
        }
    }
}
