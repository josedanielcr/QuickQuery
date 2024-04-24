using QuickqueryDataGatewayAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace QuickqueryDataGatewayAPI.Database
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