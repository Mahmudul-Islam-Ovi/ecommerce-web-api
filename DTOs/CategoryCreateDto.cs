using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_Web_Application.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100,MinimumLength = 2,ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string? Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Category Description CANNOT EXCEED 500 characters")]
        public string? Description { get; set; } = string.Empty;
    }
}