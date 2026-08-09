using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShelterApi.Models;

namespace ShelterApi.date;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Shelter> Shelters => Set<Shelter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shelter>()
            .HasOne(e => e.Area)
            .WithMany(e => e.Shelters)
            .HasForeignKey(e => e.AreaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Inspection>()
           .HasOne(e => e.Shelter)
           .WithMany(e => e.Inspections)
           .HasForeignKey(e => e.ShelterId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);
    

    }
    
}
