using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public static class PrepDb
    {
        public static void PrepPolulation(WebApplication app, bool isProduction)
        {
            using var serviceScope = app.Services.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProduction);

        }

        private static void SeedData(AppDbContext context,bool isProduction)
        {
            if(isProduction)
            {
                Console.WriteLine("---> Attemtimp to apply migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--->It's occured an error while seeding data");
                }
             
            }
            if(!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding data...");
                context.Platforms.AddRange(
                    new Platform(){Name="Dot net",Publisher="Microsoft",Cost="Free"},
                    new Platform(){Name="SQL Server Express",Publisher="Microsoft",Cost="Free"},
                    new Platform(){Name="Kubernetes",Publisher="Cloud Native Computing Foundation",Cost="Free"}
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}