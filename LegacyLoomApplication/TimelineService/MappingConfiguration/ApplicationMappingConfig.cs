using AutoMapper;
using TimelineService.DTOs;
using TimelineService.Models;

namespace TimelineService.MappingConfiguration
{
    public class ApplicationMappingConfig: Profile
    {

        private static StoryDTO ToStoryDTO(Story story)
        {
            var storyDTO = new StoryDTO()
            {
                Title = story.Title,
                Content = story.Content,
                Medias = ToMediasDTO(story.Medias)
            };
            return storyDTO;
        }

        private static MediasDTO? ToMediasDTO(Medias? medias)
        {
            if (medias == null) return null;
            if (medias.Images == null || medias.Images.Count == 0) return null;
            var mediasDTO = new MediasDTO()
            {
                Images = new List<ImageDTO>()
            };
            for (int i = 0; i < medias.Images.Count; i++)
            {
                var imageDTO = new ImageDTO()
                {
                    Name = medias.Images[i].Name,
                    Notation = medias.Images[i].Notation,
                    Data = medias.Images[i].Data
                };
                mediasDTO.Images.Add(imageDTO);
            }
            return mediasDTO;
        }
        public ApplicationMappingConfig()
        {
            CreateMap<Image, ImageDTO>();
            CreateMap<Medias, MediasDTO>();
            CreateMap<Story, StoryDTO>();
            CreateMap<Timeline, TimelineDTO>()
                .ForMember(dest => dest.Visibility,
                    s => s.MapFrom(src => src.Visibility.ToString())
                )
                .ForMember(dest => dest.StoryDTO,
                    s => s.MapFrom(src => ToStoryDTO(src.Story))
                );
        }
    }
}
