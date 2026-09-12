namespace SkillSync.Api.DTOs.Notification;

public class NotificationResponse
{
    public Guid Id { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}