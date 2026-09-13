using SkillSync.Api.DTOs.Notification;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<NotificationResponse>> GetByUserIdAsync(Guid userId)
    {
        var notifications = await _repository.GetByUserIdAsync(userId);

        return notifications
            .Select(notification => new NotificationResponse
            {
                Id = notification.Id,
                Type = notification.Type,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAtUtc = notification.CreatedAtUtc
            })
            .ToList();
    }
}