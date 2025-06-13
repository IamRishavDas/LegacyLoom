using MongoDB.Driver;
using NotificationService.DTOs;
using NotificationService.Models;
using NotificationService.RequestFeatures;
using RequestFeatureShared;
using ServiceResponseShared;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        Task<ServiceResponse<Notification>> Create(Notification notification);
        Task<ServiceResponse<DeleteResult>> Delete(string id);
        Task<(ServiceResponse<IEnumerable<NotificationDTO>>, MetaData)> GetAll(NotificationRequestParameters requestParameters);
        Task<ServiceResponse<Notification?>> GetById(string id);
        Task<ServiceResponse<string>> Update(string id, Notification notification);
        Task<(ServiceResponse<IEnumerable<NotificationDTO>>, MetaData)> GetNotificationsByUserId(Guid userId, NotificationRequestParameters requestParameters);
    }
}
