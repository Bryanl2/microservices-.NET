using AutoMapper;
using Dtos;
using Models;
using PlatformService.Dtos;

namespace Profiles
{
    public class PlatformProfile:Profile
    {
        public PlatformProfile()
        {
            //Source -> target
            CreateMap<Platform,PlatformReadDto>();
            CreateMap<PlatformCreateDto,Platform>();
            CreateMap<PlatformReadDto,PlatformPublishedDto>();
        }
    }
}