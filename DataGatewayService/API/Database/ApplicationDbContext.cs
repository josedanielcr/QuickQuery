using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Database
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountrySearchLog> CountriesSearchLog { get; set; }
    }
}