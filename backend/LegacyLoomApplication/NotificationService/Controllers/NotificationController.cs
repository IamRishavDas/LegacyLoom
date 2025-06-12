using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;
using NotificationService.EmailTemplates;
using ServiceResponseShared;
using SendGrid;
using Microsoft.AspNetCore.Authorization;

namespace NotificationService.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationSender _notificationSender;

        public NotificationController(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        [HttpPost("send")]
        [Authorize("Admin")]
        public async Task<ActionResult<ServiceResponse<Response>>> SendWelcomeMailAsync([FromBody] EmailSenderModel emailSenderModel)
        {
            var response = await _notificationSender
                .SendNotification(
                    emailSenderModel.ReceiverEmailAddress, 
                    "Welcome to Legacy Loom", 
                    emailSenderModel.UserName, 
                    "", 
                    TemplateName.WELCOME);
            return StatusCode(response.StatusCode, response);
        }
    }
}
