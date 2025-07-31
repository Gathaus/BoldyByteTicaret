using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Services;

namespace ECommerceApp.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _brandRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _brandRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Brand>> GetActiveBrandsAsync()
        {
            return await _brandRepository.GetActiveAsync();
        }

        public async Task<IEnumerable<Brand>> GetPopularBrandsAsync(int count)
        {
            var brands = await _brandRepository.GetActiveAsync();
            return brands.Take(count);
        }

        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            return await _brandRepository.CreateAsync(brand);
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            await _brandRepository.UpdateAsync(brand);
        }

        public async Task DeleteBrandAsync(int id)
        {
            await _brandRepository.DeleteAsync(id);
        }
    }
} 