using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment : BaseEntity
    {
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime PaymentDate { get; set; }
        
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        
        [StringLength(100)]
        public string? ReferenceNumber { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Foreign key
        [Required]
        public int InvoiceId { get; set; }
        
        // Navigation property
        public Invoice Invoice { get; set; } = null!;
    }
