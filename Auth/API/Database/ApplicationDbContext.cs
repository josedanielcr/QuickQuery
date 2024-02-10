using QuickqueryAuthenticationAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace QuickqueryAuthenticationAPI.Database
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}