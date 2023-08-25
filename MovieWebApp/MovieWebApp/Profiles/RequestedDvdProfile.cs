using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class RequestedDvdProfile:Profile
    {
        public RequestedDvdProfile()
        {
            CreateMap<RequestedDvd, RequestedDvdDTO>();
        }
    }
}
