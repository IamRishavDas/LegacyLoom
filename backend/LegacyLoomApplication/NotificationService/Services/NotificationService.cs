using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NotificationService.Models;
using RequestFeatureShared;
using ServiceResponseShared;
using System.Net;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {

        private readonly IMongoCollection<Notification> _notificationCollection;

        public NotificationService(IOptions<NotificationDbSettings> options, IMongoClient client)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            _notificationCollection = db.GetCollection<Notification>(options.Value.CollectionName);
        }

        public async Task<ServiceResponse<Notification>> Create(Notification notification)
        {
            try
            {
                notification.Id = ObjectId.GenerateNewId().ToString();
                await _notificationCollection.InsertOneAsync(notification);
                return ServiceResponse<Notification>.SuccessResult(notification, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Notification>.Failure("Error while registering the notificatio record", ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<DeleteResult>> Delete(string id)
        {
            try
            {
                var result = await _notificationCollection.DeleteOneAsync(n => n.Id == id);
                return ServiceResponse<DeleteResult>.SuccessResult(result, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<DeleteResult>.Failure($"Error while deleting the Notification, Id: {id}", ex.Message, (int)HttpStatusCode.BadRequest);
            }
        }

        public Task<ServiceResponse<PagedList<Notification>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<Notification?>> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<PagedList<Notification>>> GetNotificationsByUserId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ReplaceOneResult>> Update(string id, Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
