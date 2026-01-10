using FabrilOS.API.Data;
using FabrilOS.API.DTOs;
using FabrilOS.API.Entities;
using FabrilOS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FabrilOS.API.Services.Implementations;

public class ServiceOrderService : IServiceOrderService
{
  private readonly FabrilOSContext _context;

  public ServiceOrderService(FabrilOSContext context)
  {
    _context = context;
  }

  public async Task<ServiceOrder> CreateAsync(CreateServiceOrderDto dto, int userId)
  {
    var serviceOrder = new ServiceOrder
    {
      Title = dto.Title,
      Description = dto.Description,
      UserId = userId,
      CreatedAt = DateTime.UtcNow,
    };

    if (dto.ChecklistItemIds.Any())
    {
      foreach (var itemId in dto.ChecklistItemIds)
      {
        var itemExists = await _context.ChecklistItems.AnyAsync(c => c.Id == itemId);

        if (itemExists)
        {
          serviceOrder.Checklists.Add(new ServiceOrderChecklist
          {
            ChecklistItemId = itemId,
            IsChecked = false
          });
        }
      }
    }

    _context.ServiceOrders.Add(serviceOrder);
    await _context.SaveChangesAsync();

    return serviceOrder;
  }
}