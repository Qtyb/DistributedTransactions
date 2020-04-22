using AutoMapper;
using OrderApi.Data.Entities;
using OrderApi.Domain.Dtos.Request;

namespace OrderApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<OrderRequestDto, Order>();
        }
    }
}