using FabrilOS.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FabrilOS.API.Data;

public class FabrilOSContext : DbContext
{
  public FabrilOSContext(DbContextOptions<FabrilOSContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; }
  public DbSet<ServiceOrder> ServiceOrders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    modelBuilder.Entity<ServiceOrder>()
      .HasOne(s => s.User)
      .WithMany()
      .HasForeignKey(s => s.UserId);
  }
}