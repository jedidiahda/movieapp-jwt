using AutoMapper;
using MovieWebApp.Models;
using MovieWebApp.DTO;

namespace MovieWebApp.Profiles
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDTO, Account>()
                .ForMember(dest => 
                    dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest =>
                    dest.Role,
                    opt => opt.MapFrom(src => src.Role ?? ""))
                .ForMember(dest =>
                    dest.Active,
                    opt => opt.MapFrom(src => src.Active));

            CreateMap<Account, AccountDTO>()
                .ForMember(dest =>
                    dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                //.ForMember(dest =>
                //    dest.Password,
                //    opt => opt.MapFrom(src => src.Password))
                .ForMember(dest =>
                    dest.Role,
                    opt => opt.MapFrom(src=>src.Role))
                .ForMember(dest =>
                    dest.Active,
                    opt => opt.MapFrom(scr => scr.Active));
        }
    }
}
