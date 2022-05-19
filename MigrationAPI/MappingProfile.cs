using AutoMapper;
using MigrationAPI.Data.DTO;
using MigrationAPI.Data.Entities;

namespace MigrationAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cities, CitiesDTO>();
        }
        
    }
}
