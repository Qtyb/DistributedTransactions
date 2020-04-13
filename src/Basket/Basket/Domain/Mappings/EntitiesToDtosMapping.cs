using AutoMapper;
using BasketApi.Data.Entites;
using BasketApi.Domain.Dtos.Response;

namespace BasketApi.Domain.Mappings
{
    public class EntitiesToDtosMapping : Profile
    {
        public EntitiesToDtosMapping()
        {
            CreateMap<Basket, BasketResponseDto>();
        }
    }
}