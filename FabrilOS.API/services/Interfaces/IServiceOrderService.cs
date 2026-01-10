using FabrilOS.API.DTOs;
using FabrilOS.API.Entities;

namespace FabrilOS.API.Services.Interfaces;

public interface IServiceOrderService
{
  Task<ServiceOrder> CreateAsync(CreateServiceOrderDto dto, int userId);
}