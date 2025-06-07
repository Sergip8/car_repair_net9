using System.ComponentModel.DataAnnotations;

public class ServiceCategory : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        // Navigation properties
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }