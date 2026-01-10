using System.Security.Claims;
using FabrilOS.API.DTOs;
using FabrilOS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FabrilOS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ServiceOrderController : ControllerBase
{
  private readonly IServiceOrderService _service;

  public ServiceOrderController(IServiceOrderService service)
  {
    _service = service;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateServiceOrderDto dto)
  {
    var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
    {
      return Unauthorized("Token inválido ou ID do usuário não encontrado.");
    }

    var createdOrder = await _service.CreateAsync(dto, userId);

    return CreatedAtAction(nameof(Create), new { id = createdOrder.Id }, createdOrder);
  }
}