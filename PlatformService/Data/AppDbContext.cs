using Microsoft.EntityFrameworkCore;
using Models;

namespace Data{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
        {
            
        }
        public DbSet<Platform> Platforms {get;set;}

    }
}