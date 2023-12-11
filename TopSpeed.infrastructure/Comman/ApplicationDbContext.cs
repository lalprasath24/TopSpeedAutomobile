using Microsoft.EntityFrameworkCore;
using TopSpeed.Domain.Models;

namespace TopSpeed.infrastructure.Comman
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Brand> Brand { get; set; }


    }
}
