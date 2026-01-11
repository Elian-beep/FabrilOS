using FabrilOS.API.DTOs;
using FabrilOS.API.Entities;

namespace FabrilOS.API.Services.Interfaces;

public interface IServiceOrderService
{
  Task<ServiceOrder> CreateAsync(CreateServiceOrderDto dto, int userId);
  Task<List<ServiceOrderResponseDto>> GetAllAsync(int userId);
  Task<ServiceOrderResponseDto?> GetByIdAsync(int id, int userId);
  Task<bool> UpdateAsync(int id, int userId, UpdateServiceOrderDto dto);
  Task<bool> DeleteAsync(int id, int userId);
  Task<bool> AddImageAsync(int serviceOrderId, int userId, IFormFile file);
  Task<List<ChecklistItem>> GetChecklistCatalogAsync();
}