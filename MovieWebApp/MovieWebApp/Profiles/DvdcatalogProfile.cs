using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class DvdcatalogProfile:Profile
    {
        public DvdcatalogProfile()
        {
            CreateMap<Dvdcatalog, DVDCatalogDTO>()
                .ForMember(des => des.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(des => des.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(des => des.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(des => des.StockQty, opt => opt.MapFrom((src) => src.StockQty))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => (src.Description)))
                .ForMember(des => des.fileName, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.NoDisk, opt => opt.MapFrom(src => src.NoDisk))
                .ForMember(des => des.ReleasedDate, opt => opt.MapFrom(src => src.ReleasedDate))
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.Title))
                .ReverseMap();
        }
    }
}
