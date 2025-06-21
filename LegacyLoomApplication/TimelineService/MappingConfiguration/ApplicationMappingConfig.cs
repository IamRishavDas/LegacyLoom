using AutoMapper;
using TimelineService.DTOs;
using TimelineService.Models;

namespace TimelineService.MappingConfiguration
{
    public class ApplicationMappingConfig: Profile
    {
        public ApplicationMappingConfig()
        {
            CreateMap<Image, ImageDTO>();
            CreateMap<Medias, MediasDTO>();
            CreateMap<Story, StoryDTO>();
            CreateMap<Timeline, TimelineDTO>();
        }
    }
}
