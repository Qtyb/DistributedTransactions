using AutoMapper;
using System;
using UserApi.Data.Entities;
using UserApi.Domain.Dtos.Request;

namespace UserApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<ProductRequestDto, Product>()
                .ForMember(entity => entity.Guid, opt => opt.MapFrom((source, dest) => dest.Guid == default(Guid) ? source.Guid : dest.Guid))
                .ForMember(entity => entity.Date, opt => opt.MapFrom((_, dest) => dest.Date == default(DateTime) ? DateTime.Now : dest.Date))
                ;
        }
    }
}