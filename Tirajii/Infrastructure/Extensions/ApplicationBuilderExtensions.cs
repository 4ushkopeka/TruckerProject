using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tirajii.Data;
using Tirajii.Data.Models;

namespace Tirajii.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);
            SeedRoles(services);
            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<TruckersDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<TruckersDbContext>();

            if (data.TruckClasses.Any() && data.TruckingCategories.Any() && data.CompanyCategories.Any())
            {
                return;
            }
            data.TruckClasses.AddRange(new[]
            {
                new TruckClass { Name = "BE" },
                new TruckClass { Name = "C" },
                new TruckClass { Name = "CE" },
                new TruckClass { Name = "C1" },
                new TruckClass { Name = "C1E" }
            });

            data.CompanyCategories.AddRange(new[]
            {
                new CompanyCategory { Name = "OfferProvider"},
                new CompanyCategory { Name = "TruckProvider"}
            });

            data.TruckingCategories.AddRange(new[]
            {
                new TruckingCategory { Name = "Livestock"},
                new TruckingCategory { Name = "Food"},
                new TruckingCategory { Name = "Items"}
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(
            IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync("Administrator"))
                {
                    return;
                }

                var role = new IdentityRole { Name = "Administrator" };

                await roleManager.CreateAsync(role);

                const string adminEmail = "admin@bs.com";
                const string adminUserName = "Admin404";
                const string adminPassword = "Admin69!";

                var user = new User
                {
                    Email = adminEmail,
                    UserName = adminUserName,
                    IsOfferCompanyOwner = false,
                    IsTrucker = false,
                    IsTruckerCompanyOwner = false,
                    HasWallet = false,
                };

                var result = userManager.CreateAsync(user, adminPassword).Result;

                if (result.Succeeded) await userManager.AddToRoleAsync(user, role.Name);
            })
                .GetAwaiter()
                .GetResult();
        } 
        private static void SeedRoles(
            IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync("Trucker"))
                {
                    return;
                }

                var tRole = new IdentityRole { Name = "Trucker" };
                var tcRole = new IdentityRole { Name = "TruckCompany" };
                var cRole = new IdentityRole { Name = "OfferCompany" };

                await roleManager.CreateAsync(tRole);
                await roleManager.CreateAsync(tcRole);
                await roleManager.CreateAsync(cRole);
            })
                .GetAwaiter()
                .GetResult();
        }
    }
}
