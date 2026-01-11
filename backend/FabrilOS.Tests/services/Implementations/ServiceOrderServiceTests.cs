using FabrilOS.API.Data;
using FabrilOS.API.DTOs;
using FabrilOS.API.Entities;
using FabrilOS.API.Services.Implementations;
using FabrilOS.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FabrilOS.Tests;

public class ServiceOrderServiceTests
{
  private readonly FabrilOSContext _context;
  private readonly Mock<IFileStorageService> _fileStorageMock;
  private readonly ServiceOrderService _service;

  public ServiceOrderServiceTests()
  {
    var options = new DbContextOptionsBuilder<FabrilOSContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;

    _context = new FabrilOSContext(options);
    _fileStorageMock = new Mock<IFileStorageService>();
    _service = new ServiceOrderService(_context, _fileStorageMock.Object);

    SeedDatabase();
  }

  private void SeedDatabase()
  {
    _context.ChecklistItems.AddRange(
        new ChecklistItem { Id = 1, Label = "Verificar Óleo", IsActive = true },
        new ChecklistItem { Id = 2, Label = "Limpar Filtro", IsActive = true },
        new ChecklistItem { Id = 3, Label = "Item Velho (Inativo)", IsActive = false }
    );
    _context.SaveChanges();
  }

  [Fact(DisplayName = "CreateAsync deve incluir TODOS itens ativos e marcar APENAS os selecionados")]
  public async Task CreateAsync_ShouldIncludeAllActiveItems_AndCheckSelectedOnes()
  {
    int userId = 10;
    var dto = new CreateServiceOrderDto
    {
      Title = "OS de Teste",
      Description = "Testando InMemory",
      ChecklistItemIds = new List<int> { 2 }
    };

    var result = await _service.CreateAsync(dto, userId);

    result.Should().NotBeNull();
    result.Id.Should().BeGreaterThan(0);

    result.Checklists.Should().HaveCount(2);

    var item2 = result.Checklists.First(c => c.ChecklistItemId == 2);
    item2.IsChecked.Should().BeTrue();

    var item1 = result.Checklists.First(c => c.ChecklistItemId == 1);
    item1.IsChecked.Should().BeFalse();

    result.Checklists.Any(c => c.ChecklistItemId == 3).Should().BeFalse();
  }

  [Fact(DisplayName = "UpdateAsync deve atualizar o status dos itens existentes")]
  public async Task UpdateAsync_ShouldUpdateStatus_OfExistingItems()
  {
    int userId = 10;
    var existingOrder = new ServiceOrder
    {
      Id = 50,
      UserId = userId,
      Title = "Original",
      Checklists = new List<ServiceOrderChecklist>
        {
          new ServiceOrderChecklist { ChecklistItemId = 1, IsChecked = true },
          new ServiceOrderChecklist { ChecklistItemId = 2, IsChecked = false }
        }
    };
    _context.ServiceOrders.Add(existingOrder);
    await _context.SaveChangesAsync();

    var updateDto = new UpdateServiceOrderDto
    {
      Title = "Titulo Novo",
      Description = "Desc Nova",
      ChecklistItemIds = new List<int> { 2 }
    };

    var success = await _service.UpdateAsync(50, userId, updateDto);

    success.Should().BeTrue();

    var updatedOrder = await _context.ServiceOrders
        .Include(o => o.Checklists)
        .FirstAsync(o => o.Id == 50);

    updatedOrder.Title.Should().Be("Titulo Novo");

    updatedOrder.Checklists.First(c => c.ChecklistItemId == 1).IsChecked.Should().BeFalse();
    updatedOrder.Checklists.First(c => c.ChecklistItemId == 2).IsChecked.Should().BeTrue();
  }
}