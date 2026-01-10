using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabrilOS.API.Entities;

public class ServiceOrderChecklist
{
    [Key]
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    
    [ForeignKey("ServiceOrderId")]
    public ServiceOrder? ServiceOrder { get; set; }
    public int ChecklistItemId { get; set; }
    
    [ForeignKey("ChecklistItemId")]
    public ChecklistItem? ChecklistItem { get; set; }
    public bool IsChecked { get; set; } = false;
}