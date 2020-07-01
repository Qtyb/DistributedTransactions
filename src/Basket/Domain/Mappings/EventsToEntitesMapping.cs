using AutoMapper;
using BasketApi.Data.Entites;
using BasketApi.Domain.Events.Product;
using System;

namespace BasketApi.Domain.Mappings
{
    public class EventsToEntitesMapping : Profile
    {
        public EventsToEntitesMapping()
        {
            CreateMap<ProductCreated, Product>()
                .ForMember(entity => entity.Id, opt => opt.Ignore())
                .ForMember(entity => entity.Date, opt => opt.MapFrom((_, dest) => dest.Date == default(DateTime) ? DateTime.Now : dest.Date))
                ;
        }
    }
}