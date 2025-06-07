using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkOrderPart : BaseEntity
    {
        [Required]
        public int WorkOrderId { get; set; }
        
        [Required]
        public int PartId { get; set; }
        
        [Required]
        public int QuantityUsed { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Navigation properties
        public WorkOrder WorkOrder { get; set; } = null!;
        public Part Part { get; set; } = null!;
    }