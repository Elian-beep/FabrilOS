using System.ComponentModel.DataAnnotations;

namespace FabrilOS.API.Entities;

public class ChecklistItem
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string Label { get; set; } = string.Empty;

  public bool IsActive { get; set; } = true;
}