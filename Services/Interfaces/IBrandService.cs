using System.Collections.Generic;
using System.Threading.Tasks;
using car_repair.Models;
using car_repair.Models.DTO;

public interface IBrandService
{
    Task<List<Brand>> GetAllAsync();
    Task<Brand?> GetByIdAsync(int id);
    Task<Brand> CreateAsync(Brand brand);
    Task<Brand> UpdateAsync(int id, Brand brand);
    Task<bool> DeleteAsync(int id);
    Task<List<BrandResponse>> GetBrandsHome();
} 