using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest =>
                    dest.Gender,
                    opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest =>
                    dest.Address,
                    opt => opt.MapFrom(src => src.Address))
                .ForMember(dest =>
                    dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.Id)).ReverseMap();

        }
    }
}
