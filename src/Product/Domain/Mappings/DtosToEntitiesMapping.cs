using AutoMapper;
using ProductApi.Data.Entities;
using ProductApi.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<ProductRequestDto, Product>();
        }
    }
}