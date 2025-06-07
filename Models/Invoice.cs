using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Invoice : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;
        
        [Required]
        public DateTime InvoiceDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SubTotal { get; set; }
        
        [Column(TypeName = "decimal(8,4)")]
        public decimal TaxRate { get; set; } = 0;
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxAmount { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal AmountPaid { get; set; } = 0;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        // Foreign keys
        [Required]
        public int CustomerId { get; set; }
        
        public int? WorkOrderId { get; set; }
        
        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public WorkOrder? WorkOrder { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }