using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabrilOS.API.Entities;

public class ServiceOrderImage
{
  [Key]
  public int Id { get; set; }
  public string FileName { get; set; } = string.Empty;
  [NotMapped]
  public string? PresignedUrl { get; set; }
  public int ServiceOrderId { get; set; }
  [ForeignKey("ServiceOrderId")]
  public ServiceOrder? ServiceOrder { get; set; }
}