using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;
using ServiceResponseShared;
using Microsoft.AspNetCore.Authorization;
using NotificationService.RequestFeatures;
using RequestFeatureShared.Constants;
using System.Text.Json;
using MongoDB.Driver;

namespace NotificationService.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationSender _notificationSender;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationSender notificationSender, INotificationService notificationService)
        {
            _notificationSender = notificationSender;
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ServiceResponse<string>>> SendWelcomeMailAsync([FromBody] EmailSenderModel emailSenderModel)
        {
            var response = await _notificationSender
                .SendWelcomeNotificationAsync(
                    emailSenderModel.ReceiverEmailAddress, 
                    emailSenderModel.UserName
                    );
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Notification>>>> GetAllNotifications([FromQuery]NotificationRequestParameters notificationRequestParameters)
        {
            var (notificationsServiceResponse, metadata) = await _notificationService.GetAll(notificationRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(notificationsServiceResponse.StatusCode, notificationsServiceResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Notification>>> GetAllNotificationById([FromRoute]string id)
        {
            var notification = await _notificationService.GetById(id);
            return StatusCode(notification.StatusCode, notification);
        }

        [HttpGet("/users/{userId}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Notification>>>> GetAllNotificationsByUserId([FromRoute]Guid userId, [FromQuery]NotificationRequestParameters notificationRequestParameters)
        {
            var(notificationsServiceResponse, metadata) = await _notificationService.GetNotificationsByUserId(userId, notificationRequestParameters);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(notificationsServiceResponse.StatusCode, notificationsServiceResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<DeleteResult>>> DeleteNotificationById([FromRoute] string id)
        {
            var response = await _notificationService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
