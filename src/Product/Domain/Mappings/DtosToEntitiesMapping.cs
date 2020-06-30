using AutoMapper;
using ProductApi.Data.Entities;
using ProductApi.Domain.Dtos.Request;
using System;

namespace ProductApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<ProductRequestDto, Product>()
                .ForMember(entity => entity.Guid, opt => opt.MapFrom((_, dest) => dest.Guid == default(Guid) ? Guid.NewGuid() : dest.Guid))
                .ForMember(entity => entity.Date, opt => opt.MapFrom((_, dest) => dest.Date == default(DateTime) ? DateTime.Now : dest.Date))
                ;
        }
    }
}