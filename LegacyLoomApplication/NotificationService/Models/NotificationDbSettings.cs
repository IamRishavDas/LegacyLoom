namespace NotificationService.Models
{
    public class NotificationDbSettings
    {
        public string CollectionName {get; set;} = null!;
        public string ConnectionString {get; set;} = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
