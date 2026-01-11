namespace FabrilOS.API.DTOs;

public class UpdateServiceOrderDto
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public List<int>? ChecklistItemIds { get; set; }
}