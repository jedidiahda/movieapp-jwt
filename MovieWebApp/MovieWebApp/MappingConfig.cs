using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Customer, CustomerRequestDTO>().ReverseMap();
                config.CreateMap<Customer, CustomerDTO>().ReverseMap();
                config.CreateMap<Account, AccountDTO>().ReverseMap();
                config.CreateMap<CustomerSubscription,CustomerSubscriptionDTO>().ReverseMap();
                config.CreateMap<Dvdcatalog,DVDCatalogDTO>().ReverseMap();
                config.CreateMap<Dvdcopy, DvdcopyDTO>().ReverseMap();
                config.CreateMap<Payment, PaymentDTO>().ReverseMap();
                config.CreateMap<RequestedDvd,RequestedDvdDTO>().ReverseMap();
                config.CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
                config.CreateMap<Watchlist, WatchlistDTO>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
