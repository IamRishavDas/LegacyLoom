using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NotificationService.DTOs;
using NotificationService.Models;
using NotificationService.RequestFeatures;
using RequestFeatureShared;
using RequestFeatureShared.SortHelper;
using ServiceResponseShared;
using System.Net;

namespace NotificationService.Services
{
    public class NotificationService : INotificationService
    {

        private readonly IMongoCollection<Notification> _notificationCollection;
        private readonly IMapper _mapper;
        private readonly ISortHelper<Notification> _sortHelper;

        public NotificationService(IOptions<NotificationDbSettings> options, IMongoClient client, ISortHelper<Notification> sortHelper, IMapper mapper)
        {
            var db = client.GetDatabase(options.Value.DatabaseName);
            _notificationCollection = db.GetCollection<Notification>(options.Value.CollectionName);
            _sortHelper = sortHelper;
            _mapper = mapper;
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

        public async Task<(ServiceResponse<IEnumerable<NotificationDTO>>, MetaData)> GetAll(NotificationRequestParameters requestParameters)
        {
            try
            {
                List<Notification> notifications = await _notificationCollection.Find(s => true).ToListAsync();
                var orderedNotfications = _sortHelper.ApplySort(notifications.AsQueryable(), requestParameters.OrderBy);

                var count = orderedNotfications.Count();
                var result = orderedNotfications.Skip((requestParameters.PageNumber - 1 * requestParameters.PageSize)).ToList();

                var pagedListNotifications = PagedList<Notification>.ToPagedList(result, count, requestParameters.PageNumber, requestParameters.PageSize);
                return
                    (
                        ServiceResponse<IEnumerable<NotificationDTO>>.SuccessResult(_mapper.Map<IEnumerable<NotificationDTO>>(pagedListNotifications), (int)HttpStatusCode.OK),
                        pagedListNotifications.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<NotificationDTO>>.Failure("Error while retrieving notification details", ex.Message, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<ServiceResponse<Notification?>> GetById(string id)
        {
            try
            {
                var notification = await _notificationCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
                return ServiceResponse<Notification?>.SuccessResult(notification, (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Notification?>.Failure($"Error while retrieving the notification, Id: {id}", ex.Message, (int)HttpStatusCode.NotFound);
            }
        }

        public async Task<(ServiceResponse<IEnumerable<NotificationDTO>>, MetaData)> GetNotificationsByUserId(Guid userId, NotificationRequestParameters requestParameters)
        {
            try
            {
                var notificationsByUserId = await _notificationCollection.Find(n => n.SendToUserId == userId.ToString()).ToListAsync();
                var sortedNotifications = _sortHelper.ApplySort(notificationsByUserId.AsQueryable(), requestParameters.OrderBy);

                var count = sortedNotifications.Count();
                var result = sortedNotifications.Skip((requestParameters.PageNumber - 1) * requestParameters.PageSize).ToList();
                var pagedListNotifications = PagedList<Notification>.ToPagedList(result, count, requestParameters.PageNumber, requestParameters.PageSize);
                return
                    (
                        ServiceResponse<IEnumerable<NotificationDTO>>.SuccessResult(_mapper.Map<IEnumerable<NotificationDTO>>(pagedListNotifications), (int)HttpStatusCode.OK),
                        pagedListNotifications.MetaData
                    );
            }
            catch (Exception ex)
            {
                return
                    (
                        ServiceResponse<IEnumerable<NotificationDTO>>.Failure("Error while retrieving notification details", ex.Message, (int)HttpStatusCode.InternalServerError),
                        new MetaData()
                    );
            }
        }

        public async Task<ServiceResponse<string>> Update(string id, Notification notification)
        {
            //await _notificationCollection.ReplaceOneAsync(s => s.Id == id, notification);
            return ServiceResponse<string>.SuccessResult("This feature is not available currently", (int)HttpStatusCode.OK);
        }
    }
}
