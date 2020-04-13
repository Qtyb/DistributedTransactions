using AutoMapper;
using OrderApi.Data.Entities;
using OrderApi.Domain.Dtos.Response;

namespace OrderApi.Domain.Mappings
{
    public class EntitiesToDtosMapping : Profile
    {
        public EntitiesToDtosMapping()
        {
            CreateMap<Order, OrderResponseDto>();
        }
    }
}