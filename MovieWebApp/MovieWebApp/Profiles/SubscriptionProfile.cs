using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class SubscriptionProfile:Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, SubscriptionDTO>();
        }
    }
}
