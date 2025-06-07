using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Service : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; }
        
        [Range(1, 480)] // Max 8 hours
        public int EstimatedDurationMinutes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Foreign key
        public int ServiceCategoryId { get; set; }
        
        // Navigation properties
        public ServiceCategory ServiceCategory { get; set; } = null!;
        public ICollection<WorkOrderServiceEnt> WorkOrderServices { get; set; } = new List<WorkOrderServiceEnt>();
    }