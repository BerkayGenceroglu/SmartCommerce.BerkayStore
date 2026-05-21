using Microsoft.EntityFrameworkCore;
using ProductApi.Context;
using ProductApi.Dtos;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ProductContext _context;

        public CategoryService(ProductContext context)
        {
            _context = context;
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            var existing = await _context.Categories
            .FirstOrDefaultAsync(x => x.Name == dto.Name);

            if (existing != null)
                throw new Exception("Bu kategori zaten mevcut!");

            var category = new Category
            {
                Name = dto.Name,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                throw new Exception("Kategori bulunamadı!");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var values = await _context.Categories.Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt
            }).ToListAsync();
            return values;
        }

        public async Task<CategoryResponseDto> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                throw new Exception("Kategori bulunamadı!");

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
        }
    }
}
