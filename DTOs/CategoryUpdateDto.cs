using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_Web_Application.DTOs
{
    public class CategoryUpdateDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string? Name { get; set; }
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Category Description must be between 2 and 500 characters")]
        public string? Description { get; set; } = string.Empty;
    }
}