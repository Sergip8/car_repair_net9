using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkOrderServiceEnt : BaseEntity
    {
        [Required]
        public int WorkOrderId { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        [Column(TypeName = "decimal(4,2)")]
        public decimal Quantity { get; set; } = 1;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Navigation properties
        public WorkOrder WorkOrder { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }