using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tirajii.Data.Models;

namespace Tirajii.Data
{
    public class TruckersDbContext : IdentityDbContext<User>
    {
        public TruckersDbContext()
        {

        }
        public TruckersDbContext(DbContextOptions<TruckersDbContext> options)
            : base(options)
        {
        }
        public DbSet<CompanyCategory> CompanyCategories { get; set; }
        public DbSet<CompanyRatings> CompanyRatings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<TruckOffer> TruckOffers { get; set; }
        public DbSet<Trucker> Truckers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<TruckingCategory> TruckingCategories { get; set; }
        public DbSet<TruckClass> TruckClasses { get; set; }
        public DbSet<Wallet> Wallets { get; init; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TruckOffer>(x =>
            {
                x.HasOne(x => x.Truck)
                .WithOne(x => x.TruckOffer)
                .HasForeignKey<TruckOffer>(x => x.TruckId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<CompanyRatings>(x =>
            {
                x.HasKey(x => new { x.RaterId, x.CompanyId });

                x.HasOne(x => x.Rater)
                .WithMany(x => x.CompaniesRated)
                .HasForeignKey(x => x.RaterId)
                .OnDelete(DeleteBehavior.Restrict);
                
                x.HasOne(x => x.Company)
                .WithMany(x => x.CompanyRatings)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            //builder
            //.Entity<TruckClass>()
            //     .HasData(new TruckClass()
            //     {
            //         Id = 1,
            //         Name = "BE"
            //     },
            //     new TruckClass()
            //     {
            //         Id = 2,
            //         Name = "C1"
            //     },
            //     new TruckClass()
            //     {
            //         Id = 3,
            //         Name = "C"
            //     },
            //     new TruckClass()
            //     {
            //         Id = 4,
            //         Name = "C1E"
            //     },
            //     new TruckClass()
            //     {
            //         Id = 5,
            //         Name = "CE"
            //     });
            //builder
            //.Entity<CompanyCategory>()
            //     .HasData(new CompanyCategory()
            //     {
            //         Id = 1,
            //         Name = "OfferProvider"
            //     },
            //     new CompanyCategory()
            //     {
            //         Id = 2,
            //         Name = "TruckProvider"
            //     });
            //builder
            //.Entity<TruckingCategory>()
            //     .HasData(new TruckingCategory()
            //     {
            //         Id = 1,
            //         Name = "Livestock"
            //     },
            //     new TruckingCategory()
            //     {
            //         Id = 2,
            //         Name = "Food"
            //     },
            //     new TruckingCategory()
            //     {
            //         Id = 3,
            //         Name = "Items"
            //     });
            base.OnModelCreating(builder);
        }
    }
}