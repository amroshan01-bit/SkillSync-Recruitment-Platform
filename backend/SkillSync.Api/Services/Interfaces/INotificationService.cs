using SkillSync.Api.DTOs.Notification;

namespace SkillSync.Api.Services.Interfaces;

public interface INotificationService
{
    Task<List<NotificationResponse>> GetByUserIdAsync(Guid userId);
}
