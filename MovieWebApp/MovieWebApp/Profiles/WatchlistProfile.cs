using AutoMapper;
using MovieWebApp.DTO;
using MovieWebApp.Models;

namespace MovieWebApp.Profiles
{
    public class WatchlistProfile:Profile
    {
        public WatchlistProfile()
        {
            CreateMap<Watchlist, WatchlistDTO>();
        }
    }
}
