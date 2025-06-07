using System.ComponentModel.DataAnnotations;

namespace car_repair.Models.DTO
{
    public class VehicleResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2030)]
        public int Year { get; set; }

        [Required]
        [StringLength(17)]
        public string VIN { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Engine { get; set; }

        public int? Mileage { get; set; }

        [StringLength(20)]
        public string? Transmission { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
