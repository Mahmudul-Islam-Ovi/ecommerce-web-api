using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce_Web_Application.Controllers;
using Ecommerce_Web_Application.data;
using Ecommerce_Web_Application.DTOs;
using Ecommerce_Web_Application.Enums;
using Ecommerce_Web_Application.Helpers;
using Ecommerce_Web_Application.Interfaces;
using Ecommerce_Web_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Web_Application.Services
{
    public class CategoryServices : ICategoryService
    {
        // private static readonly List<Category> _categories = new List<Category>();

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategoryServices(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CategoryReadDto>> GetAllCategories(QueryParameters queryParameters)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                var formattedSearch = $"%{queryParameters.Search.Trim()}%";
                query = query.Where(x => EF.Functions.ILike(x.Name!, formattedSearch) || EF.Functions.ILike(x.Description!, formattedSearch));
            }

            if (string.IsNullOrWhiteSpace(queryParameters.SortOrder))
            {
                query = query.OrderBy(x => x.Name);
            }
            else
            {
                var formattedSortOrder = queryParameters.SortOrder.Trim().ToLower();
                if (Enum.TryParse<SortOrder>(formattedSortOrder, true, out var parsedSortOrder))
                {
                    query = parsedSortOrder switch
                    {
                        SortOrder.Name => query.OrderBy(x => x.Name),
                        SortOrder.NameDesc => query.OrderByDescending(x => x.Name),
                        SortOrder.Description => query.OrderBy(x => x.Description),
                        SortOrder.CreatedAt => query.OrderBy(x => x.CreatedAt),
                        _ => query.OrderBy(x => x.Name),
                    };
                }
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize).ToListAsync();

            var result = _mapper.Map<List<CategoryReadDto>>(items);

            return new PaginatedResult<CategoryReadDto>
            {
                Items = result,
                TotalCount = totalCount,
                PageIndex = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }
        public async Task<CategoryReadDto?> GetCategoriesById(Guid catID)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(catID);
            return foundCategory == null ? null : _mapper.Map<CategoryReadDto>(foundCategory);
        }
        public async Task<CategoryReadDto> CreateCategories(CategoryCreateDto categoryData)
        {
            var newCategory = _mapper.Map<Category>(categoryData);
            await _appDbContext.Categories.AddAsync(newCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(newCategory);
        }

        public async Task<CategoryReadDto?> UpdateByIdCategories(Guid catID, CategoryUpdateDto categoryData)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(catID);
            if (foundCategory == null)
            {
                return null;
            }
            _mapper.Map(categoryData, foundCategory);
            _appDbContext.Categories.Update(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(foundCategory);
        }

        public async Task<bool> DeleteByIdCategories(Guid catID)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(catID);
            if (foundCategory == null)
            {
                return false;
            }
            _appDbContext.Categories.Remove(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}