namespace FabrilOS.API.DTOs;

public class ServiceOrderResponseDto
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public string UserName { get; set; } = string.Empty;

  public List<ChecklistItemDto> Checklists { get; set; } = new();
  public List<string> ImageUrls { get; set; }
}

public class ChecklistItemDto
{
  public int Id { get; set; }
  public int ChecklistItemId { get; set; }
  public string Label { get; set; } = string.Empty;
  public bool IsChecked { get; set; }
}