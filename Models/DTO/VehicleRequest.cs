using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace car_repair.Models.DTO
{
    public class VehicleRequest
    {
        [Required]
        [StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2030)]
        public int Year { get; set; }

        public IFormFile Image { get; set; }

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

        public VehicleRequest(string jsonData, IFormFile image)
        {
            var data = JsonSerializer.Deserialize<VehicleRequestData>(jsonData)
                       ?? throw new ArgumentException("Invalid vehicle data JSON");

            Make = data.Make;
            Model = data.Model;
            Year = data.Year;
            VIN = data.VIN;
            LicensePlate = data.LicensePlate;
            Color = data.Color;
            Engine = data.Engine;
            Mileage = data.Mileage;
            Transmission = data.Transmission;
            Notes = data.Notes;
            CustomerId = data.CustomerId;
            Image = image;
        }
    }
    public class VehicleRequestData
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string VIN { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Engine { get; set; }
        public int? Mileage { get; set; }
        public string? Transmission { get; set; }
        public string? Notes { get; set; }
        public int CustomerId { get; set; }
    }

    public class VehicleRequestForm
    {
        public IFormFile Image { get; set; }

        public string Data { get; set; } = string.Empty;
    }
}
