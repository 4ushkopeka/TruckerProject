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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Trucker> Truckers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Trucker>(x =>
            {
                x.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Trucker>(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            base.OnModelCreating(builder);
        }
    }
}