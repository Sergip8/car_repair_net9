using AutoMapper;
using car_repair.Models;
using car_repair.Models.DTO;
using System;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        // Vehicle
        CreateMap<VehicleRequest, Vehicle>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());
        CreateMap<VehicleResponse, Vehicle>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        // ... Vehicle -> VehicleResponse mapping if needed ...
        CreateMap<Vehicle, VehicleResponse>();
            
        // Appointment
        CreateMap<CreateAppointmentRequest, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedEmployee, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrder, opt => opt.Ignore());
        CreateMap<Appointment, AppointmentResponse>();

        // Customer
        CreateMap<CreateCustomerRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Customer, CustomerResponse>();

        // Employee
        CreateMap<CreateEmployeeRequest, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Employee, EmployeeResponse>();

        // Invoice
        CreateMap<CreateInvoiceRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrder, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());
        CreateMap<Invoice, InvoiceResponse>();

        // Part
        CreateMap<CreatePartRequest, Part>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Part, PartResponse>();

        // Payment
        CreateMap<CreatePaymentRequest, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore());
        CreateMap<Payment, PaymentResponse>();

        // ServiceCategory
        CreateMap<CreateServiceCategoryRequest, ServiceCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<ServiceCategory, ServiceCategoryResponse>();

        // Service
        CreateMap<CreateServiceRequest, Service>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ServiceCategory, opt => opt.Ignore());
        CreateMap<Service, ServiceResponse>();

        // WorkOrder
        CreateMap<CreateWorkOrderRequest, WorkOrder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
            .ForMember(dest => dest.Appointment, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedEmployee, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrderServices, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrderParts, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore());
        CreateMap<WorkOrder, WorkOrderResponse>();

        // WorkOrderPart
        CreateMap<CreateWorkOrderPartRequest, WorkOrderPart>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrder, opt => opt.Ignore())
            .ForMember(dest => dest.Part, opt => opt.Ignore());
        CreateMap<WorkOrderPart, WorkOrderPartResponse>();

        // WorkOrderService
        CreateMap<CreateWorkOrderServiceRequest, WorkOrderServiceEnt>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrder, opt => opt.Ignore())
            .ForMember(dest => dest.Service, opt => opt.Ignore());
        CreateMap<WorkOrderServiceEnt, WorkOrderServiceResponse>();

        // User
        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));

        CreateMap<Service, ServiceWithCategoryResponse>()
            .ForMember(dest => dest.ServiceCategory, opt => opt.MapFrom(src => src.ServiceCategory));

        CreateMap<Brand, BrandResponse>();
    }
}
