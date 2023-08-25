using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class Dvdcopy​Profile:Profile
    {
        public DvdcopyProfile()
        {
            CreateMap<Dvdcopy​, Dvdcopy​DTO>();
        }
    }
}
