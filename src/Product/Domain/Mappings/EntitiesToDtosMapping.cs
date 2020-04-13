using AutoMapper;
using ProductApi.Data.Entities;
using ProductApi.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Domain.Mappings
{
    public class EntitiesToDtosMapping : Profile
    {
        public EntitiesToDtosMapping()
        {
            CreateMap<Product, ProductResponseDto>();
        }
    }
}