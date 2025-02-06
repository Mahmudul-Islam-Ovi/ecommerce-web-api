using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_Application.DTOs;
using Ecommerce_Web_Application.Helpers;
using Ecommerce_Web_Application.Interfaces;
using Ecommerce_Web_Application.Models;
using Ecommerce_Web_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Web_Application.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET REQUEST
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] QueryParameters queryParameters)
        {
            queryParameters.Validate();
            var categoryList = await _categoryService.GetAllCategories(queryParameters);
            return Ok(ApiResponse<PaginatedResult<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Success"));
        }

        // GET REQUEST BY ID
        [HttpGet("{catID:guid}")]
        public async Task<IActionResult> GetCategoryById(Guid catID)
        {
            var category = await _categoryService.GetCategoriesById(catID);
            if (category == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Category with this id not found" }, 404, "Validation Failed"));
            }
            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(category, 200, "Success"));
        }
        // POST  REQUEST
        [HttpPost]
        public async Task<IActionResult> CreateCategories([FromBody] CategoryCreateDto categoryData)
        {
            var categoryReadDto = await _categoryService.CreateCategories(categoryData);
            return Created(nameof(GetCategoryById), ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 201, "Category  Created Success"));
        }

        // Delete  REQUEST
        [HttpDelete("{catID:guid}")]
        public async Task<IActionResult> DeleteCategories(Guid catID)
        {
            var foundCategory = await _categoryService.DeleteByIdCategories(catID);
            if (!foundCategory)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Category with this id not found" }, 404, "Validation Failed"));
            }
            return Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category Deleted Successfully"));
        }
        // UPDATE  REQUEST
        [HttpPut("{catID:guid}")]
        public async Task<IActionResult> UpdateCategories(Guid catID, [FromBody] CategoryUpdateDto categoryData)
        {
            var updateCategory = await _categoryService.UpdateByIdCategories(catID, categoryData);
            if (updateCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Category with this id not found" }, 404, "Validation Failed"));
            }
            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(updateCategory, 200, "Category Update Successfully"));
        }
    }
}