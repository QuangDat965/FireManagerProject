using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Database
{
    public class FireDbContext : DbContext
    {
        public FireDbContext(DbContextOptions<FireDbContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<RuleEntity> Rules { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<ApartmentNeighbour> ApartmentNeighbours { get; set; }
        public DbSet<TopicThreshhold> TopicThreshholds { get; set; }
        public DbSet<HistoryData> HistoryDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RuleEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<UserEntity>().HasKey(e => e.UserId);
            modelBuilder.Entity<Apartment>().HasKey(e => e.Id);
            modelBuilder.Entity<Building>().HasKey(e => e.Id);
            modelBuilder.Entity<Module>().HasKey(e => e.Id);
            modelBuilder.Entity<DeviceEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<ApartmentNeighbour>().HasKey(e => new { e.ApartmentId, e.NeighbourId });
            modelBuilder.Entity<TopicThreshhold>().HasKey(e => new { e.DeviceId, e.RuleId });
            modelBuilder.Entity<HistoryData>().HasKey(e => e.Id);


            modelBuilder.Entity<UserEntity>()
                .HasOne(p => p.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Building>()
                .HasOne(p => p.User)
                .WithMany(p => p.Buildings)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Apartment>()
                .HasOne(p => p.Building)
                .WithMany(p => p.Apartments)
                .HasForeignKey(p => p.BuldingId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApartmentNeighbour>()
                .HasOne(e => e.Apartment1)
                .WithMany(p => p.ApartmentNeighbours)
                .HasForeignKey(e => e.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApartmentNeighbour>()
                .HasOne(e => e.Apartment2)
                .WithMany(p => p.ApartmentNeighbours2)
                .HasForeignKey(e => e.NeighbourId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Module>()
                .HasOne(e => e.Apartment)
                .WithMany(p => p.Modules)
                .HasForeignKey(e => e.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Module>()
                .HasOne(e => e.User)
                .WithMany(p => p.Modules)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DeviceEntity>()
                .HasOne(e => e.Module)
                .WithMany(p => p.Devices)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TopicThreshhold>()
                .HasOne(e => e.Device)
                .WithMany(p => p.TopicThreshholds)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TopicThreshhold>()
                .HasOne(e => e.Rule)
                .WithMany(p => p.TopicThreshholds)
                .HasForeignKey(e => e.RuleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HistoryData>()
    .HasOne(e => e.DeviceEntity)
    .WithMany(p => p.HistoryDatas)
    .HasForeignKey(e => e.DeviceId)
    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HistoryData>()
    .HasOne(e => e.User)
    .WithMany(p => p.HistoryDatas)
    .HasForeignKey(e => e.UserId)
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
