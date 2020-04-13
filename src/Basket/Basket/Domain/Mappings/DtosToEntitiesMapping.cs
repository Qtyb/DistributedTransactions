using AutoMapper;
using BasketApi.Data.Entites;
using BasketApi.Domain.Dtos.Request;

namespace BasketApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
            CreateMap<BasketRequestDto, Basket>();
        }
    }
}