using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce_Web_Application.DTOs;
using Ecommerce_Web_Application.Models;

namespace Ecommerce_Web_Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto,Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}