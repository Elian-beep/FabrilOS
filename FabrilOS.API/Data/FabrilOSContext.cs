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
  public DbSet<ChecklistItem> ChecklistItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    modelBuilder.Entity<ServiceOrder>()
      .HasOne(s => s.User)
      .WithMany()
      .HasForeignKey(s => s.UserId);

    modelBuilder.Entity<ChecklistItem>().HasData(
          new ChecklistItem { Id = 1, Label = "Verificar nível de óleo da máquina", IsActive = true },
          new ChecklistItem { Id = 2, Label = "Inspecionar correias e engrenagens", IsActive = true },
          new ChecklistItem { Id = 3, Label = "Limpeza dos filtros de ar", IsActive = true },
          new ChecklistItem { Id = 4, Label = "Verificar vazamentos hidráulicos", IsActive = true },
          new ChecklistItem { Id = 5, Label = "Testar painel de controle elétrico", IsActive = true }
      );
  }
}