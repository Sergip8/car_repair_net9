using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using car_repair.Models;
using AutoMapper;
using car_repair.Models.DTO;

public class BrandService : IBrandService
{
    private readonly CarRepairDbContext _context;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly IMapper _mapper;
    public BrandService(CarRepairDbContext context, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<List<Brand>> GetAllAsync()
    {
        return await _context.Brands.AsNoTracking().ToListAsync();
    }

    public async Task<Brand?> GetByIdAsync(int id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public async Task<Brand> CreateAsync(Brand brand)
    {
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand> UpdateAsync(int id, Brand brand)
    {
        var existing = await _context.Brands.FindAsync(id);
        if (existing == null) throw new KeyNotFoundException($"Brand {id} not found");
        existing.Title = brand.Title;
        existing.ImageUrl = brand.ImageUrl;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand == null) return false;
        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<BrandResponse>> GetBrandsHome()
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var brands = await _context.Brands.AsNoTracking().ToListAsync();
            return brands.Select(b => _mapper.Map<BrandResponse>(b)).ToList();
        }, nameof(GetBrandsHome));
    }
} 