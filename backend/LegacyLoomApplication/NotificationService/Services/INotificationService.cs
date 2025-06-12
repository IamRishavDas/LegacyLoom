using MongoDB.Driver;
using NotificationService.Models;
using RequestFeatureShared;
using ServiceResponseShared;

namespace NotificationService.Services
{
    public interface INotificationService
    {
        Task<ServiceResponse<Notification>> Create(Notification notification);
        Task<ServiceResponse<DeleteResult>> Delete(string id);
        Task<ServiceResponse<PagedList<Notification>>> GetAll();
        Task<ServiceResponse<Notification?>> GetById(string id);
        Task<ServiceResponse<ReplaceOneResult>> Update(string id, Notification notification);
        Task<ServiceResponse<PagedList<Notification>>> GetNotificationsByUserId(Guid id);
    }
}
