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
        public DbSet<CompanyCategory> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Trucker> Truckers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<TrailerType> TrailerTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Trucker>(x =>
            {
                x.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Trucker>(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Truck>(x =>
            {
                x.HasOne(x => x.Trailer)
                .WithOne(x => x.Truck)
                .HasForeignKey<Truck>(x => x.TrailerId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Trailer>(x =>
            {
                x.HasOne(x => x.Truck)
                .WithOne(x => x.Trailer)
                .HasForeignKey<Trailer>(x => x.TruckId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            base.OnModelCreating(builder);
        }
    }
}