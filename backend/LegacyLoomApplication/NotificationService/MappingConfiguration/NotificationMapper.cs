using AutoMapper;
using NotificationService.DTOs;
using NotificationService.Models;

namespace NotificationService.MappingConfiguration
{
    public class NotificationMapper: Profile
    {
        public NotificationMapper()
        {
            CreateMap<Notification, NotificationDTO>().ReverseMap();
        }
    }
}
