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

        private static StoryLookupDTO ToStoryLookupDTO(Story story)
        {
            var storyLookupDTO = new StoryLookupDTO()
            {
                Title = story.Title,
                Content = new String(story.Content.Take(150).ToArray()),
                Medias = ToMediasLookupDTO(story.Medias)
            };
            return storyLookupDTO;
        }

        private static MediasLookupDTO? ToMediasLookupDTO(Medias? medias)
        {
            if (medias == null) return null;
            if (medias.Images == null || medias.Images.Count == 0) return null;
            var mediasLookupDTO = new MediasLookupDTO()
            {
                Images = new List<ImageLookupDTO>()
            };

            for(int i=0; i<medias.Images.Count; i++)
            {
                var imageLookupDTO = new ImageLookupDTO()
                {
                    Name = medias.Images[i].Name,
                    Data = medias.Images[i].Data
                };
                mediasLookupDTO.Images.Add(imageLookupDTO);
            }
            return mediasLookupDTO;
        }

        public ApplicationMappingConfig()
        {
            CreateMap<Image, ImageDTO>();
            CreateMap<Medias, MediasDTO>();
            CreateMap<Story, StoryDTO>();

            CreateMap<Image, ImageLookupDTO>();
            CreateMap<Medias, MediasLookupDTO>();
            CreateMap<Story, StoryLookupDTO>();

            CreateMap<Timeline, TimelineDTO>()
                .ForMember(dest => dest.Visibility,
                    s => s.MapFrom(src => src.Visibility.ToString())
                )
                .ForMember(dest => dest.StoryDTO,
                    s => s.MapFrom(src => ToStoryDTO(src.Story))
                )
                .ForMember(dest=> dest.Likes,
                    s => s.MapFrom(src => src.Likes.Count)
                )
                .ForMember(dest=> dest.Dislikes,
                    s => s.MapFrom(src => src.Dislikes.Count)
                );

            CreateMap<Timeline, TimelineLookupDTO>()
                .ForMember(dest => dest.StoryDTO,
                    s => s.MapFrom(src => ToStoryLookupDTO(src.Story)))
                .ForMember(dest => dest.Likes,
                    s => s.MapFrom(src => src.Likes.Count)
                )
                .ForMember(dest => dest.Dislikes,
                    s => s.MapFrom(src => src.Dislikes.Count)
                );
        }
    }
}
