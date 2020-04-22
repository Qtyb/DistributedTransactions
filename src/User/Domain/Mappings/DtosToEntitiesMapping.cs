using AutoMapper;
using UserApi.Data.Entities;
using UserApi.Domain.Dtos.Request;

namespace UserApi.Domain.Mappings
{
    public class DtosToEntitiesMapping : Profile
    {
        public DtosToEntitiesMapping()
        {
           CreateMap<UserRequestDto, User>();
        }
    }
}