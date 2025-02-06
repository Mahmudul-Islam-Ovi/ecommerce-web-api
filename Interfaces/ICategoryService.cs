using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_Application.Controllers;
using Ecommerce_Web_Application.DTOs;
using Ecommerce_Web_Application.Helpers;

namespace Ecommerce_Web_Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedResult<CategoryReadDto>> GetAllCategories(QueryParameters queryParameters);
        Task<CategoryReadDto?> GetCategoriesById(Guid catID);
        Task<CategoryReadDto> CreateCategories(CategoryCreateDto categoryData);
        Task<CategoryReadDto?> UpdateByIdCategories(Guid catID, CategoryUpdateDto categoryData);
        Task<bool> DeleteByIdCategories(Guid catID);
    }
}