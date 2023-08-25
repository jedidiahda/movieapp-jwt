using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class DvdcatalogProfile:Profile
    {
        public DvdcatalogProfile()
        {
            CreateMap<Dvdcatalog, DVDCatalogDTO>();
        }
    }
}
