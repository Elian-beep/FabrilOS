using FabrilOS.API.Data;
using FabrilOS.API.DTOs;
using FabrilOS.API.Entities;
using FabrilOS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FabrilOS.API.Services.Implementations;

public class ServiceOrderService : IServiceOrderService
{
  private readonly FabrilOSContext _context;
  private readonly IFileStorageService _storageService;

  public ServiceOrderService(FabrilOSContext context, IFileStorageService storageService)
  {
    _context = context;
    _storageService = storageService;
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

    var allChecklistItems = await _context.ChecklistItems
      .Where(c => c.IsActive)
      .ToListAsync();

    if (allChecklistItems.Any())
    {
      foreach (var item in allChecklistItems)
      {
        bool isSelected = dto.ChecklistItemIds != null && dto.ChecklistItemIds.Contains(item.Id);

        serviceOrder.Checklists.Add(new ServiceOrderChecklist
        {
          ChecklistItemId = item.Id,
          IsChecked = isSelected
        });
      }
    }

    _context.ServiceOrders.Add(serviceOrder);
    await _context.SaveChangesAsync();

    return serviceOrder;
  }

  public async Task<bool> AddImageAsync(int serviceOrderId, int userId, IFormFile file)
  {
    var os = await _context.ServiceOrders.FirstOrDefaultAsync(x => x.Id == serviceOrderId && x.UserId == userId);
    if (os == null) return false;

    var fileName = await _storageService.UploadFileAsync(file);

    var imageEntity = new ServiceOrderImage
    {
      ServiceOrderId = serviceOrderId,
      FileName = fileName
    };

    _context.ServiceOrderImages.Add(imageEntity);
    await _context.SaveChangesAsync();

    return true;
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
        .Include(x => x.Images)
        .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

    if (os == null) return null;

    var response = new ServiceOrderResponseDto
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
      }).ToList(),
      ImageUrls = new List<string>()
    };

    if (os.Images != null)
    {
      foreach (var image in os.Images)
      {
        var url = await _storageService.GetPresignedUrlAsync(image.FileName);
        response.ImageUrls.Add(url);
      }
    }

    return response;
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
      foreach (var existingLink in os.Checklists)
      {
        bool shouldBeChecked = dto.ChecklistItemIds.Contains(existingLink.ChecklistItemId);

        existingLink.IsChecked = shouldBeChecked;
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