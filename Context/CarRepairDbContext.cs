using Microsoft.EntityFrameworkCore;

public class CarRepairDbContext : DbContext
    {
        public CarRepairDbContext(DbContextOptions<CarRepairDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderServiceEnt> WorkOrderServices { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<WorkOrderPart> WorkOrderParts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<Brand> Brands { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships and constraints
        ConfigureEntityRelationships(modelBuilder);

        // Configure decimal precision
        ConfigureDecimalPrecision(modelBuilder);

        // Configure indexes
        ConfigureIndexes(modelBuilder);

        // Seed data
        SeedData(modelBuilder);
    }

    private void ConfigureEntityRelationships(ModelBuilder modelBuilder)
    {
        // Customer -> Vehicle (One-to-Many)
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Service -> ServiceCategory (Many-to-One)
        modelBuilder.Entity<Service>()
            .HasOne(s => s.ServiceCategory)
            .WithMany(sc => sc.Services)
            .HasForeignKey(s => s.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Appointment relationships
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Customer)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Vehicle)
            .WithMany(v => v.Appointments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.AssignedEmployee)
            .WithMany(e => e.Appointments)
            .HasForeignKey(a => a.AssignedEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // WorkOrder relationships
        modelBuilder.Entity<WorkOrder>()
            .HasOne(wo => wo.Vehicle)
            .WithMany(v => v.WorkOrders)
            .HasForeignKey(wo => wo.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkOrder>()
            .HasOne(wo => wo.Appointment)
            .WithOne(a => a.WorkOrder)
            .HasForeignKey<WorkOrder>(wo => wo.AppointmentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkOrder>()
            .HasOne(wo => wo.AssignedEmployee)
            .WithMany(e => e.WorkOrders)
            .HasForeignKey(wo => wo.AssignedEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // WorkOrderService (Many-to-Many junction)
        modelBuilder.Entity<WorkOrderServiceEnt>()
            .HasOne(wos => wos.WorkOrder)
            .WithMany(wo => wo.WorkOrderServices)
            .HasForeignKey(wos => wos.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkOrderServiceEnt>()
            .HasOne(wos => wos.Service)
            .WithMany(s => s.WorkOrderServices)
            .HasForeignKey(wos => wos.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // WorkOrderPart (Many-to-Many junction)
        modelBuilder.Entity<WorkOrderPart>()
            .HasOne(wop => wop.WorkOrder)
            .WithMany(wo => wo.WorkOrderParts)
            .HasForeignKey(wop => wop.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkOrderPart>()
            .HasOne(wop => wop.Part)
            .WithMany(p => p.WorkOrderParts)
            .HasForeignKey(wop => wop.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        // Invoice relationships
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.WorkOrder)
            .WithOne(wo => wo.Invoice)
            .HasForeignKey<Invoice>(i => i.WorkOrderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Payment -> Invoice (Many-to-One)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Invoice)
            .WithMany(i => i.Payments)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
                

                // User -> Role (Many-to-One)
modelBuilder.Entity<User>()
    .HasOne(u => u.Role)
    .WithMany(r => r.Users)
    .HasForeignKey(u => u.RoleId)
    .OnDelete(DeleteBehavior.Restrict);

// User -> Customer (One-to-One, optional)
modelBuilder.Entity<User>()
    .HasOne(u => u.Customer)
    .WithOne()
    .HasForeignKey<User>(u => u.CustomerId)
    .OnDelete(DeleteBehavior.SetNull);

// User -> Employee (One-to-One, optional)
modelBuilder.Entity<User>()
    .HasOne(u => u.Employee)
    .WithOne()
    .HasForeignKey<User>(u => u.EmployeeId)
    .OnDelete(DeleteBehavior.SetNull);
        }
        

        private void ConfigureDecimalPrecision(ModelBuilder modelBuilder)
    {
        // Service prices
        modelBuilder.Entity<Service>()
            .Property(s => s.BasePrice)
            .HasPrecision(10, 2);

        // Employee rates
        modelBuilder.Entity<Employee>()
            .Property(e => e.HourlyRate)
            .HasPrecision(8, 2);

        // WorkOrder costs
        modelBuilder.Entity<WorkOrder>()
            .Property(wo => wo.LaborHours)
            .HasPrecision(5, 2);

        modelBuilder.Entity<WorkOrder>()
            .Property(wo => wo.TotalCost)
            .HasPrecision(10, 2);

        // WorkOrderService pricing
        modelBuilder.Entity<WorkOrderServiceEnt>()
            .Property(wos => wos.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<WorkOrderServiceEnt>()
            .Property(wos => wos.Quantity)
            .HasPrecision(4, 2);

        // Part pricing
        modelBuilder.Entity<Part>()
            .Property(p => p.Cost)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Part>()
            .Property(p => p.SellPrice)
            .HasPrecision(10, 2);

        // WorkOrderPart pricing
        modelBuilder.Entity<WorkOrderPart>()
            .Property(wop => wop.UnitPrice)
            .HasPrecision(10, 2);

        // Invoice amounts
        modelBuilder.Entity<Invoice>()
            .Property(i => i.SubTotal)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TaxRate)
            .HasPrecision(5, 4);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TaxAmount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalAmount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.AmountPaid)
            .HasPrecision(10, 2);

        // Payment amounts
        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(10, 2);
    }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Customer indexes
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Vehicle indexes
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.VIN)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.LicensePlate);

            // Employee indexes
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Part indexes
            modelBuilder.Entity<Part>()
                .HasIndex(p => p.PartNumber)
                .IsUnique();

            // Invoice indexes
            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();

            // WorkOrder indexes
            modelBuilder.Entity<WorkOrder>()
                .HasIndex(wo => wo.WorkOrderNumber)
                .IsUnique();
        }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var createdAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Seed Service Categories
        modelBuilder.Entity<ServiceCategory>().HasData(
            new ServiceCategory { Id = 1, Name = "Engine Services", Description = "Engine maintenance and repair services", CreatedAt = createdAt },
            new ServiceCategory { Id = 2, Name = "Brake Services", Description = "Brake system maintenance and repair", CreatedAt = createdAt },
            new ServiceCategory { Id = 3, Name = "Transmission Services", Description = "Transmission maintenance and repair", CreatedAt = createdAt },
            new ServiceCategory { Id = 4, Name = "Electrical Services", Description = "Electrical system diagnostics and repair", CreatedAt = createdAt },
            new ServiceCategory { Id = 5, Name = "Suspension & Steering", Description = "Suspension and steering system services", CreatedAt = createdAt },
            new ServiceCategory { Id = 6, Name = "General Maintenance", Description = "Routine maintenance services", CreatedAt = createdAt }
        );

        // Seed Services
        modelBuilder.Entity<Service>().HasData(
            // Engine Services
            new Service { Id = 1, Name = "Oil Change", Description = "Engine oil and filter replacement", BasePrice = 39.99m, EstimatedDurationMinutes = 30, ServiceCategoryId = 1, CreatedAt = createdAt },
            new Service { Id = 2, Name = "Engine Tune-Up", Description = "Complete engine tune-up service", BasePrice = 149.99m, EstimatedDurationMinutes = 120, ServiceCategoryId = 1, CreatedAt = createdAt },
            new Service { Id = 3, Name = "Engine Diagnostics", Description = "Computer engine diagnostics", BasePrice = 89.99m, EstimatedDurationMinutes = 60, ServiceCategoryId = 1, CreatedAt = createdAt },

            // Brake Services
            new Service { Id = 4, Name = "Brake Pad Replacement", Description = "Front or rear brake pad replacement", BasePrice = 179.99m, EstimatedDurationMinutes = 90, ServiceCategoryId = 2, CreatedAt = createdAt },
            new Service { Id = 5, Name = "Brake Inspection", Description = "Complete brake system inspection", BasePrice = 49.99m, EstimatedDurationMinutes = 45, ServiceCategoryId = 2, CreatedAt = createdAt },
            new Service { Id = 6, Name = "Brake Fluid Change", Description = "Brake fluid replacement and system flush", BasePrice = 79.99m, EstimatedDurationMinutes = 45, ServiceCategoryId = 2, CreatedAt = createdAt },

            // Transmission Services
            new Service { Id = 7, Name = "Transmission Service", Description = "Transmission fluid and filter change", BasePrice = 199.99m, EstimatedDurationMinutes = 90, ServiceCategoryId = 3, CreatedAt = createdAt },
            new Service { Id = 8, Name = "Transmission Diagnostics", Description = "Transmission system diagnostics", BasePrice = 99.99m, EstimatedDurationMinutes = 60, ServiceCategoryId = 3, CreatedAt = createdAt },

            // General Maintenance
            new Service { Id = 9, Name = "Tire Rotation", Description = "Tire rotation and pressure check", BasePrice = 29.99m, EstimatedDurationMinutes = 30, ServiceCategoryId = 6, CreatedAt = createdAt },
            new Service { Id = 10, Name = "Multi-Point Inspection", Description = "Comprehensive vehicle inspection", BasePrice = 59.99m, EstimatedDurationMinutes = 45, ServiceCategoryId = 6, CreatedAt = createdAt }
        );

        // Seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "John", LastName = "Smith", Email = "john.smith@carrepair.com", PhoneNumber = "555-0101", Position = "Senior Technician", HourlyRate = 35.00m, HireDate = new DateTime(2020, 3, 15), CreatedAt = createdAt },
            new Employee { Id = 2, FirstName = "Sarah", LastName = "Johnson", Email = "sarah.johnson@carrepair.com", PhoneNumber = "555-0102", Position = "Technician", HourlyRate = 28.00m, HireDate = new DateTime(2021, 7, 1), CreatedAt = createdAt },
            new Employee { Id = 3, FirstName = "Mike", LastName = "Wilson", Email = "mike.wilson@carrepair.com", PhoneNumber = "555-0103", Position = "Brake Specialist", HourlyRate = 32.00m, HireDate = new DateTime(2019, 11, 20), CreatedAt = createdAt },
            new Employee { Id = 4, FirstName = "Lisa", LastName = "Davis", Email = "lisa.davis@carrepair.com", PhoneNumber = "555-0104", Position = "Service Advisor", HourlyRate = 22.00m, HireDate = new DateTime(2022, 1, 10), CreatedAt = createdAt }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "Robert", LastName = "Anderson", Email = "robert.anderson@email.com", PhoneNumber = "555-1001", Address = "123 Main St", City = "Springfield", State = "IL", PostalCode = "62701", CreatedAt = createdAt },
            new Customer { Id = 2, FirstName = "Jennifer", LastName = "Brown", Email = "jennifer.brown@email.com", PhoneNumber = "555-1002", Address = "456 Oak Ave", City = "Springfield", State = "IL", PostalCode = "62702", CreatedAt = createdAt },
            new Customer { Id = 3, FirstName = "Michael", LastName = "Taylor", Email = "michael.taylor@email.com", PhoneNumber = "555-1003", Address = "789 Pine St", City = "Springfield", State = "IL", PostalCode = "62703", CreatedAt = createdAt },
            new Customer { Id = 4, FirstName = "Emily", LastName = "Garcia", Email = "emily.garcia@email.com", PhoneNumber = "555-1004", Address = "321 Elm Dr", City = "Springfield", State = "IL", PostalCode = "62704", CreatedAt = createdAt },
            new Customer { Id = 5, FirstName = "David", LastName = "Martinez", Email = "david.martinez@email.com", PhoneNumber = "555-1005", Address = "654 Maple Ln", City = "Springfield", State = "IL", PostalCode = "62705", CreatedAt = createdAt }
        );

        // Seed Vehicles
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle { Id = 1, Make = "Toyota", Model = "Camry", Year = 2018, VIN = "1HGBH41JXMN109186", LicensePlate = "ABC123", Color = "Silver", Engine = "2.5L I4", Mileage = 45000, Transmission = "Automatic", CustomerId = 1, CreatedAt = createdAt },
            new Vehicle { Id = 2, Make = "Honda", Model = "Civic", Year = 2020, VIN = "2HGFC2F59LH542913", LicensePlate = "XYZ789", Color = "Blue", Engine = "1.5L Turbo", Mileage = 28000, Transmission = "CVT", CustomerId = 2, CreatedAt = createdAt },
            new Vehicle { Id = 3, Make = "Ford", Model = "F-150", Year = 2019, VIN = "1FTFW1ET5KFC65842", LicensePlate = "TRK456", Color = "Black", Engine = "5.0L V8", Mileage = 52000, Transmission = "Automatic", CustomerId = 3, CreatedAt = createdAt },
            new Vehicle { Id = 4, Make = "Chevrolet", Model = "Equinox", Year = 2021, VIN = "3GNAXHEV5ML234567", LicensePlate = "SUV789", Color = "White", Engine = "1.5L Turbo", Mileage = 15000, Transmission = "Automatic", CustomerId = 4, CreatedAt = createdAt },
            new Vehicle { Id = 5, Make = "Nissan", Model = "Altima", Year = 2017, VIN = "1N4AL3AP8HC123456", LicensePlate = "NIS001", Color = "Red", Engine = "2.5L I4", Mileage = 68000, Transmission = "CVT", CustomerId = 5, CreatedAt = createdAt }
        );

        // Seed Parts
        modelBuilder.Entity<Part>().HasData(
            new Part { Id = 1, PartNumber = "OF001", Name = "Oil Filter - Standard", Brand = "ACDelco", Cost = 8.99m, SellPrice = 14.99m, QuantityInStock = 50, MinimumStock = 10, Location = "A1-B2", CreatedAt = createdAt },
            new Part { Id = 2, PartNumber = "OF002", Name = "Oil Filter - Premium", Brand = "Mobil 1", Cost = 12.99m, SellPrice = 19.99m, QuantityInStock = 30, MinimumStock = 5, Location = "A1-B3", CreatedAt = createdAt },
            new Part { Id = 3, PartNumber = "BP001", Name = "Brake Pads - Front Set", Brand = "Wagner", Cost = 45.99m, SellPrice = 79.99m, QuantityInStock = 25, MinimumStock = 5, Location = "B2-C1", CreatedAt = createdAt },
            new Part { Id = 4, PartNumber = "BP002", Name = "Brake Pads - Rear Set", Brand = "Wagner", Cost = 39.99m, SellPrice = 69.99m, QuantityInStock = 20, MinimumStock = 5, Location = "B2-C2", CreatedAt = createdAt },
            new Part { Id = 5, PartNumber = "SP001", Name = "Spark Plugs - Set of 4", Brand = "NGK", Cost = 24.99m, SellPrice = 39.99m, QuantityInStock = 40, MinimumStock = 8, Location = "C1-D1", CreatedAt = createdAt },
            new Part { Id = 6, PartNumber = "AF001", Name = "Air Filter", Brand = "K&N", Cost = 18.99m, SellPrice = 29.99m, QuantityInStock = 35, MinimumStock = 10, Location = "C2-D2", CreatedAt = createdAt },
            new Part { Id = 7, PartNumber = "BF001", Name = "Brake Fluid - DOT 3", Brand = "Valvoline", Cost = 4.99m, SellPrice = 8.99m, QuantityInStock = 60, MinimumStock = 15, Location = "D1-E1", CreatedAt = createdAt },
            new Part { Id = 8, PartNumber = "EO001", Name = "Engine Oil - 5W30", Brand = "Castrol", Cost = 22.99m, SellPrice = 34.99m, QuantityInStock = 80, MinimumStock = 20, Location = "E1-F1", CreatedAt = createdAt }
        );

        // Seed Appointments
        var appointmentDate = DateTime.UtcNow.AddDays(1);
        modelBuilder.Entity<Appointment>().HasData(
            new Appointment { Id = 1, ScheduledDateTime = appointmentDate.AddHours(9), EstimatedDurationMinutes = 30, Status = AppointmentStatus.Scheduled, Description = "Oil change service", CustomerId = 1, VehicleId = 1, AssignedEmployeeId = 1, CreatedAt = createdAt },
            new Appointment { Id = 2, ScheduledDateTime = appointmentDate.AddHours(10), EstimatedDurationMinutes = 90, Status = AppointmentStatus.Confirmed, Description = "Brake inspection and possible pad replacement", CustomerId = 2, VehicleId = 2, AssignedEmployeeId = 3, CreatedAt = createdAt },
            new Appointment { Id = 3, ScheduledDateTime = appointmentDate.AddHours(14), EstimatedDurationMinutes = 45, Status = AppointmentStatus.Scheduled, Description = "Multi-point inspection", CustomerId = 4, VehicleId = 4, AssignedEmployeeId = 2, CreatedAt = createdAt }
        );

        // Seed Work Orders
        modelBuilder.Entity<WorkOrder>().HasData(
            new WorkOrder { Id = 1, WorkOrderNumber = "WO-2024-001", Status = WorkOrderStatus.Completed, Description = "Oil change and filter replacement", DiagnosisNotes = "Vehicle due for routine maintenance", CompletionNotes = "Service completed successfully", StartedAt = createdAt.AddDays(-5).AddHours(9), CompletedAt = createdAt.AddDays(-5).AddHours(9.5d), LaborHours = 0.5m, TotalCost = 54.98m, VehicleId = 5, AssignedEmployeeId = 1, CreatedAt = createdAt.AddDays(-5) }
        );

        // Seed Work Order Services
        modelBuilder.Entity<WorkOrderServiceEnt>().HasData(
            new WorkOrderServiceEnt { Id = 1, WorkOrderId = 1, ServiceId = 1, Price = 39.99m, Quantity = 1, CreatedAt = createdAt.AddDays(-5) }
        );

        // Seed Work Order Parts
        modelBuilder.Entity<WorkOrderPart>().HasData(
            new WorkOrderPart { Id = 1, WorkOrderId = 1, PartId = 1, QuantityUsed = 1, UnitPrice = 14.99m, CreatedAt = createdAt.AddDays(-5) }
        );

        // Seed Invoices
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice { Id = 1, InvoiceNumber = "INV-2024-001", InvoiceDate = createdAt.AddDays(-5), DueDate = createdAt.AddDays(-5).AddDays(30), Status = InvoiceStatus.Paid, SubTotal = 54.98m, TaxRate = 0.0875m, TaxAmount = 4.81m, TotalAmount = 59.79m, AmountPaid = 59.79m, CustomerId = 5, WorkOrderId = 1, CreatedAt = createdAt.AddDays(-5) }
        );

        // Seed Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment { Id = 1, Amount = 59.79m, PaymentDate = createdAt.AddDays(-5), PaymentMethod = PaymentMethod.CreditCard, ReferenceNumber = "CC-123456789", InvoiceId = 1, CreatedAt = createdAt.AddDays(-5) }
        );

        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Administrator" },
            new Role { Id = 2, Name = "Customer", Description = "Customer user" },
            new Role { Id = 3, Name = "Employee", Description = "Employee user" }
        );

        // Seed Users (example, with hashed passwords)
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@carrepair.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"), RoleId = 1 },
            new User { Id = 2, Username = "customer1", Email = "robert.anderson@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"), RoleId = 2, CustomerId = 1 },
            new User { Id = 3, Username = "employee1", Email = "john.smith@carrepair.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"), RoleId = 3, EmployeeId = 1 }
        );

        modelBuilder.Entity<EmailTemplate>().HasData(
        new EmailTemplate
        {
            Id = 1,
            Name = "Bienvenida Usuario",
            Subject = "¡Bienvenido a nuestra plataforma, {{Username}}!",
            Category = "Onboarding",
            HtmlContent = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #2c3e50;'>¡Hola {{Username}}!</h1>
                        <p>Nos complace darte la bienvenida a nuestra plataforma.</p>
                        <p>Aquí tienes algunos recursos para comenzar:</p>
                        <ul>
                            <li><a href='{{GuideUrl}}'>Guía de inicio rápido</a></li>
                            <li><a href='{{SupportUrl}}'>Centro de ayuda</a></li>
                            <li><a href='{{CommunityUrl}}'>Comunidad</a></li>
                        </ul>
                        <p>¡Esperamos que tengas una excelente experiencia!</p>
                        <p>Saludos,<br>El equipo de {{CompanyName}}</p>
                    </div>",
            PlainTextContent = @"¡Hola {{Username}}!
                
                Nos complace darte la bienvenida a nuestra plataforma.
                
                Aquí tienes algunos recursos para comenzar:
                - Guía de inicio rápido: {{GuideUrl}}
                - Centro de ayuda: {{SupportUrl}}
                - Comunidad: {{CommunityUrl}}
                
                ¡Esperamos que tengas una excelente experiencia!
                
                Saludos,
                El equipo de {{CompanyName}}",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
            
            // Template 2: Confirmación de Pedido
            new EmailTemplate
            {
                Id = 2,
                Name = "Confirmación Pedido",
                Subject = "Confirmación de tu pedido #{{OrderNumber}}",
                Category = "Transaccional",
                HtmlContent = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #27ae60;'>¡Pedido Confirmado!</h1>
                        <p>Hola {{CustomerName}},</p>
                        <p>Tu pedido #{{OrderNumber}} ha sido confirmado exitosamente.</p>
                        
                        <div style='background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin: 20px 0;'>
                            <h3>Detalles del pedido:</h3>
                            <p><strong>Total:</strong> ${{OrderTotal}}</p>
                            <p><strong>Método de pago:</strong> {{PaymentMethod}}</p>
                            <p><strong>Dirección de envío:</strong> {{ShippingAddress}}</p>
                            <p><strong>Fecha estimada de entrega:</strong> {{DeliveryDate}}</p>
                        </div>
                        
                        <p><a href='{{TrackingUrl}}' style='background-color: #27ae60; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Rastrear Pedido</a></p>
                        
                        <p>Gracias por tu compra.</p>
                    </div>",
                PlainTextContent = @"¡Pedido Confirmado!
                
                Hola {{CustomerName}},
                
                Tu pedido #{{OrderNumber}} ha sido confirmado exitosamente.
                
                Detalles del pedido:
                - Total: ${{OrderTotal}}
                - Método de pago: {{PaymentMethod}}
                - Dirección de envío: {{ShippingAddress}}
                - Fecha estimada de entrega: {{DeliveryDate}}
                
                Rastrear pedido: {{TrackingUrl}}
                
                Gracias por tu compra.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            
            // Template 3: Recuperación de Contraseña
            new EmailTemplate
            {
                Id = 3,
                Name = "Recuperar Contraseña",
                Subject = "Restablece tu contraseña",
                Category = "Seguridad",
                HtmlContent = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #e74c3c;'>Restablece tu contraseña</h1>
                        <p>Hola {{Username}},</p>
                        <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta.</p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{ResetUrl}}' style='background-color: #e74c3c; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Restablecer Contraseña</a>
                        </div>
                        
                        <p>Este enlace expirará en {{ExpirationHours}} horas.</p>
                        <p>Si no solicitaste este cambio, puedes ignorar este mensaje.</p>
                        
                        <p>Por seguridad, nunca compartas este enlace con nadie.</p>
                    </div>",
                PlainTextContent = @"Restablece tu contraseña
                
                Hola {{Username}},
                
                Recibimos una solicitud para restablecer la contraseña de tu cuenta.
                
                Haz clic en el siguiente enlace para restablecer tu contraseña:
                {{ResetUrl}}
                
                Este enlace expirará en {{ExpirationHours}} horas.
                
                Si no solicitaste este cambio, puedes ignorar este mensaje.
                
                Por seguridad, nunca compartas este enlace con nadie.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            
            // Template 4: Newsletter
            new EmailTemplate
            {
                Id = 4,
                Name = "Newsletter Mensual",
                Subject = "{{CompanyName}} - Novedades del mes {{Month}}",
                Category = "Marketing",
                HtmlContent = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <header style='background-color: #3498db; color: white; padding: 20px; text-align: center;'>
                            <h1>{{CompanyName}}</h1>
                            <p>Newsletter - {{Month}} {{Year}}</p>
                        </header>
                        
                        <div style='padding: 20px;'>
                            <h2>¡Hola {{SubscriberName}}!</h2>
                            
                            <section style='margin-bottom: 30px;'>
                                <h3 style='color: #2c3e50;'>🎉 Novedades del mes</h3>
                                <p>{{MonthlyNews}}</p>
                            </section>
                            
                            <section style='margin-bottom: 30px;'>
                                <h3 style='color: #2c3e50;'>🔥 Ofertas especiales</h3>
                                <p>{{SpecialOffers}}</p>
                            </section>
                            
                            <section style='margin-bottom: 30px;'>
                                <h3 style='color: #2c3e50;'>📚 Recursos útiles</h3>
                                <ul>
                                    <li><a href='{{Resource1Url}}'>{{Resource1Title}}</a></li>
                                    <li><a href='{{Resource2Url}}'>{{Resource2Title}}</a></li>
                                    <li><a href='{{Resource3Url}}'>{{Resource3Title}}</a></li>
                                </ul>
                            </section>
                        </div>
                        
                        <footer style='background-color: #ecf0f1; padding: 20px; text-align: center; font-size: 12px;'>
                            <p>{{CompanyName}} | {{CompanyAddress}}</p>
                            <p><a href='{{UnsubscribeUrl}}'>Cancelar suscripción</a></p>
                        </footer>
                    </div>",
                PlainTextContent = @"{{CompanyName}} - Newsletter
                {{Month}} {{Year}}
                
                ¡Hola {{SubscriberName}}!
                
                NOVEDADES DEL MES
                {{MonthlyNews}}
                
                OFERTAS ESPECIALES
                {{SpecialOffers}}
                
                RECURSOS ÚTILES
                - {{Resource1Title}}: {{Resource1Url}}
                - {{Resource2Title}}: {{Resource2Url}}
                - {{Resource3Title}}: {{Resource3Url}}
                
                {{CompanyName}} | {{CompanyAddress}}
                Cancelar suscripción: {{UnsubscribeUrl}}",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            
            // Template 5: Notificación de Factura
            new EmailTemplate
            {
                Id = 5,
                Name = "Nueva Factura",
                Subject = "Nueva factura disponible - {{InvoiceNumber}}",
                Category = "Transaccional",
                HtmlContent = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #2c3e50;'>Nueva Factura Disponible</h1>
                        <p>Estimado/a {{CustomerName}},</p>
                        <p>Se ha generado una nueva factura para tu cuenta.</p>
                        
                        <div style='background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin: 20px 0;'>
                            <h3>Detalles de la factura:</h3>
                            <p><strong>Número:</strong> {{InvoiceNumber}}</p>
                            <p><strong>Fecha:</strong> {{InvoiceDate}}</p>
                            <p><strong>Monto:</strong> ${{Amount}}</p>
                            <p><strong>Fecha de vencimiento:</strong> {{DueDate}}</p>
                        </div>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{InvoiceUrl}}' style='background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; margin-right: 10px;'>Ver Factura</a>
                            <a href='{{PaymentUrl}}' style='background-color: #27ae60; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px;'>Pagar Ahora</a>
                        </div>
                        
                        <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
                    </div>",
                PlainTextContent = @"Nueva Factura Disponible
                
                Estimado/a {{CustomerName}},
                
                Se ha generado una nueva factura para tu cuenta.
                
                Detalles de la factura:
                - Número: {{InvoiceNumber}}
                - Fecha: {{InvoiceDate}}
                - Monto: ${{Amount}}
                - Fecha de vencimiento: {{DueDate}}
                
                Ver factura: {{InvoiceUrl}}
                Pagar ahora: {{PaymentUrl}}
                
                Si tienes alguna pregunta, no dudes en contactarnos.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    
        // Seed Brands
        modelBuilder.Entity<Brand>().HasData(
            new Brand { Id = 1, Title = "Toyota", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e7/Toyota.svg" },
            new Brand { Id = 2, Title = "Ford", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/3e/Ford_logo_flat.svg" },
            new Brand { Id = 3, Title = "BMW", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/BMW.svg" },
            new Brand { Id = 4, Title = "Mercedes-Benz", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/9/9e/Mercedes-Benz_Logo_2010.svg" },
            new Brand { Id = 5, Title = "Audi", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/9/92/Audi-Logo_2016.svg" },
            new Brand { Id = 6, Title = "Volkswagen", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6d/Volkswagen_logo_2019.svg" },
            new Brand { Id = 7, Title = "Honda", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7b/Honda_Logo.svg" },
            new Brand { Id = 8, Title = "Nissan", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/2/23/Nissan_2020_logo.svg" },
            new Brand { Id = 9, Title = "Chevrolet", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7c/Logo_Chevrolet.svg" },
            new Brand { Id = 10, Title = "Hyundai", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Hyundai_Motor_Company_logo.svg" }
        );
    }
    }
