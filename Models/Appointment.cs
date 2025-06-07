using System.ComponentModel.DataAnnotations;

public class Appointment : BaseEntity
    {
        [Required]
        public DateTime ScheduledDateTime { get; set; }
        
        [Range(15, 480)] // 15 minutes to 8 hours
        public int EstimatedDurationMinutes { get; set; }
        
        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(1000)]
        public string? CustomerNotes { get; set; }
        
        [StringLength(1000)]
        public string? InternalNotes { get; set; }
        
        // Foreign keys
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public int VehicleId { get; set; }
        
        public int? AssignedEmployeeId { get; set; }
        
        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
        public Employee? AssignedEmployee { get; set; }
        public WorkOrder? WorkOrder { get; set; }
    }