using AutoMapper;
using ShippingApi.Data.Entities;
using ShippingApi.Domain.Dtos.Response;

namespace ShippingApi.Domain.Mappings
{
    public class EntitiesToDtosMapping : Profile
    {
        public EntitiesToDtosMapping()
        {
            CreateMap<Shipping, ShippingResponseDto>();
        }
    }
}