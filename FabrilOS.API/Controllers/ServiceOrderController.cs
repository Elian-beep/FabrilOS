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

  [HttpPost("{id}/images")]
  public async Task<IActionResult> UploadImage(int id, IFormFile file)
  {
    if (file == null || file.Length == 0)
      return BadRequest("Arquivo inválido.");

    var userId = GetUserId();
    var success = await _service.AddImageAsync(id, userId, file);

    if (!success) return NotFound("OS não encontrada.");

    return Ok("Imagem enviada com sucesso!");
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var userId = GetUserId();
    var list = await _service.GetAllAsync(userId);
    return Ok(list);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var userId = GetUserId();
    var order = await _service.GetByIdAsync(id, userId);

    if (order == null) return NotFound("Ordem de serviço não encontrada.");

    return Ok(order);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceOrderDto dto)
  {
    var userId = GetUserId();
    var success = await _service.UpdateAsync(id, userId, dto);

    if (!success) return NotFound("Não foi possível atualizar (OS não encontrada).");

    return Ok("Ordem de serviço atualizada com sucesso.");
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var userId = GetUserId();
    var success = await _service.DeleteAsync(id, userId);

    if (!success) return NotFound("OS não encontrada.");

    return Ok("Ordem de serviço excluída.");
  }

  private int GetUserId()
  {
    var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    int.TryParse(idClaim, out int userId);
    return userId;
  }
}