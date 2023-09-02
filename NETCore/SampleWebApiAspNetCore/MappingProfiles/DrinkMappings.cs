using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class DrinkMappings : Profile
    {
        public DrinkMappings()
        {
            CreateMap<DrinkEntity, DrinkDto>().ReverseMap();
            CreateMap<DrinkEntity, DrinkUpdateDto>().ReverseMap();
            CreateMap<DrinkEntity, DrinkCreateDto>().ReverseMap();
        }
    }
}
