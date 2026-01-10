using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabrilOS.API.Entities;

public class ServiceOrder
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public List<ServiceOrderChecklist> Checklists { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}