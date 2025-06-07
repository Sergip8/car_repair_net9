using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Part : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string PartNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Brand { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SellPrice { get; set; }
        
        [Required]
        public int QuantityInStock { get; set; }
        
        public int MinimumStock { get; set; } = 0;
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public ICollection<WorkOrderPart> WorkOrderParts { get; set; } = new List<WorkOrderPart>();
    }