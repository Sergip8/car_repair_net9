using Microsoft.EntityFrameworkCore;

public class DbSeederService : IDbSeederService
    {
        private readonly CarRepairDbContext _context;
        
        public DbSeederService(CarRepairDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            
            // Additional seeding logic can be added here
            // This method is called after migrations
            
            await SeedAdditionalDataAsync();
        }

        public async Task SeedAdditionalDataAsync()
        {
            // Add any additional seed data that needs to be inserted after initial setup
            
            // Example: Add more sample work orders if none exist beyond the seeded ones
            if (!await _context.WorkOrders.AnyAsync(wo => wo.Status == WorkOrderStatus.InProgress))
            {
                var inProgressWorkOrder = new WorkOrder
                {
                    WorkOrderNumber = GenerateWorkOrderNumber(),
                    Status = WorkOrderStatus.InProgress,
                    Description = "Brake pad replacement and rotor resurfacing",
                    DiagnosisNotes = "Front brake pads worn, rotors need resurfacing",
                    StartedAt = DateTime.UtcNow.AddHours(-2),
                    LaborHours = 0,
                    TotalCost = 0,
                    VehicleId = 2, // Honda Civic
                    AssignedEmployeeId = 3 // Mike Wilson (Brake Specialist)
                };

                _context.WorkOrders.Add(inProgressWorkOrder);
                await _context.SaveChangesAsync();
            }
        }

        private string GenerateWorkOrderNumber()
        {
            var year = DateTime.Now.Year;
            var count = _context.WorkOrders.Count() + 1;
            return $"WO-{year}-{count:D3}";
        }
    }