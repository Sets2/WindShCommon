using Microsoft.EntityFrameworkCore;
using Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace DataAccess
{
    public class DataContext: IdentityDbContext<UserWind, IdentityRole<Guid>, Guid>

    {
        public DbSet<UserWind> UsersWind { get; set; } = null!;
        public DbSet<Activity> Activities { get; set; } = null!;
        public DbSet<UserSpot> UserSpots { get; set; } = null!;
        public DbSet<Spot> Spots { get; set; } = null!;
        public DbSet<SpotPhoto> SpotPhotos { get; set; } = null!;
 public DbSet<Log> Log { get; set; } = null!;

        public DataContext() {}
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SpotPhoto>()
                .HasOne(s => s.Spot)
                .WithMany(f => f.SpotPhotos)
                .HasForeignKey(sp => sp.SpotId);

            builder.Entity<UserSpot>()
                .HasOne(y => y.UserWind)
                .WithMany(x => x.UserSpots)
                .HasForeignKey(sp => sp.UserWindId);

            builder.Entity<UserSpot>()
                .HasOne(y => y.Spot)
                .WithMany(x => x.UserSpots)
                .HasForeignKey(sp => sp.SpotId);
            builder.Entity<UserSpot>().HasIndex(u => new { u.SpotId, u.UserWindId })
                .IsUnique();

            builder.Entity<Spot>()
                .HasOne(x => x.Activity)
                .WithMany(y => y.Spots)
                .HasForeignKey(sp => sp.ActivityId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
    }
}
