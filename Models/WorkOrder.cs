using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WorkOrder : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string WorkOrderNumber { get; set; } = string.Empty;
        
        [Required]
        public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Created;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        [StringLength(2000)]
        public string? DiagnosisNotes { get; set; }
        
        [StringLength(2000)]
        public string? CompletionNotes { get; set; }
        
        public DateTime? StartedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal LaborHours { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }
        
        // Foreign keys
        [Required]
        public int VehicleId { get; set; }
        
        public int? AppointmentId { get; set; }
        
        public int? AssignedEmployeeId { get; set; }
        
        // Navigation properties
        public Vehicle Vehicle { get; set; } = null!;
        public Appointment? Appointment { get; set; }
        public Employee? AssignedEmployee { get; set; }
        public ICollection<WorkOrderServiceEnt> WorkOrderServices { get; set; } = new List<WorkOrderServiceEnt>();
        public ICollection<WorkOrderPart> WorkOrderParts { get; set; } = new List<WorkOrderPart>();
        public Invoice? Invoice { get; set; }
    }
