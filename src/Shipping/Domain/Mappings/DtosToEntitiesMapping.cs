using AutoMapper;
using ShippingApi.Data.Entities;
using ShippingApi.Domain.Dtos.Request;

namespace ShippingApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<ShippingRequestDto, Shipping>();
        }
    }
}