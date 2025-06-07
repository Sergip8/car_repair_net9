
using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }