
using Grpc.Core;
using GrpcNotificationService.Protos;
using ServiceResponseShared;

namespace NotificationService.Services
{
    public interface INotificationSender
    {
        Task<ServiceResponse<string>> SendWelcomeNotificationAsync(string toEmail, string userName);
        Task<SendOtpResponse> SendOtp(SendOtpRequest request, ServerCallContext context);
    }
}