using AutoMapper;
using UserApi.Data.Entities;
using UserApi.Domain.Dtos.Response;

namespace UserApi.Domain.Mappings
{
    public class EntitiesToDtosMapping : Profile
    {
        public EntitiesToDtosMapping()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}