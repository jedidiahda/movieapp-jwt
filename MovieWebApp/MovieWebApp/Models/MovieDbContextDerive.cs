using Microsoft.EntityFrameworkCore;
using MovieWebApp.DTO;

namespace MovieWebApp.Models
{
    public class MovieDbContextDerive//:MovieDbContext
    {
        //public MovieDbContextDerive(DbContextOptions<MovieDbContext> options) : base(options)
        //{
        //}

        //public MovieDbContextDerive(DbContextOptions<MovieDbContextDerive> options)
        //    : base(options)
        //{

        //}
        public virtual DbSet<TestDto> TestDto { get; set; }
    }
}
