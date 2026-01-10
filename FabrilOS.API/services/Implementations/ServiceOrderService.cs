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

  public async Task<List<ServiceOrderResponseDto>> GetAllAsync(int userId)
  {
    var orders = await _context.ServiceOrders
        .Where(os => os.UserId == userId)
        .Include(os => os.User)
        .OrderByDescending(os => os.CreatedAt)
        .Select(os => new ServiceOrderResponseDto
        {
          Id = os.Id,
          Title = os.Title,
          Description = os.Description,
          CreatedAt = os.CreatedAt,
          UserName = os.User!.Name,
          Checklists = new List<ChecklistItemDto>()
        })
        .ToListAsync();

    return orders;
  }

  public async Task<ServiceOrderResponseDto?> GetByIdAsync(int id, int userId)
  {
    var os = await _context.ServiceOrders
        .Include(x => x.User)
        .Include(x => x.Checklists)
            .ThenInclude(x => x.ChecklistItem)
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

    if (os == null) return null;

    return new ServiceOrderResponseDto
    {
      Id = os.Id,
      Title = os.Title,
      Description = os.Description,
      CreatedAt = os.CreatedAt,
      UserName = os.User!.Name,
      Checklists = os.Checklists.Select(c => new ChecklistItemDto
      {
        Id = c.Id,
        ChecklistItemId = c.ChecklistItemId,
        Label = c.ChecklistItem!.Label,
        IsChecked = c.IsChecked
      }).ToList()
    };
  }

  public async Task<bool> UpdateAsync(int id, int userId, UpdateServiceOrderDto dto)
  {
    var os = await _context.ServiceOrders
        .Include(x => x.Checklists)
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

    if (os == null) return false;

    os.Title = dto.Title;
    os.Description = dto.Description;

    if (dto.ChecklistItemIds != null)
    {
      _context.ServiceOrderChecklists.RemoveRange(os.Checklists);

      foreach (var itemId in dto.ChecklistItemIds)
      {
        var itemExists = await _context.ChecklistItems.AnyAsync(c => c.Id == itemId);

        if (itemExists)
        {
          os.Checklists.Add(new ServiceOrderChecklist
          {
            ChecklistItemId = itemId,
            IsChecked = false
          });
        }
      }
    }

    await _context.SaveChangesAsync();
    return true;
  }

  public async Task<bool> DeleteAsync(int id, int userId)
  {
    var os = await _context.ServiceOrders.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

    if (os == null) return false;

    _context.ServiceOrders.Remove(os);
    await _context.SaveChangesAsync();
    return true;
  }
}