
namespace NotificationService.DTOs
{
    public class NotificationDTO
    {
        public string? Id { get; set; }
        public required string SendToUserId { get; set; }
        public required string SendToUserEmail { get; set; }
        public required string TemplateUsed { get; set; }
        public DateTime SendedAt { get; private set; } = DateTime.UtcNow;
    }
}
