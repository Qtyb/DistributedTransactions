using AutoMapper;
using ShippingApi.Data.Entities;
using ShippingApi.Domain.Dtos.Request;
using System;

namespace ShippingApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<ShippingRequestDto, Shipping>();
            CreateMap<ProductRequestDto, Product>()
                .ForMember(entity => entity.Guid, opt => opt.MapFrom((source, dest) => dest.Guid == default(Guid) ? source.Guid : dest.Guid))
                .ForMember(entity => entity.Date, opt => opt.MapFrom((_, dest) => dest.Date == default(DateTime) ? DateTime.Now : dest.Date))
                ;
        }
    }
}