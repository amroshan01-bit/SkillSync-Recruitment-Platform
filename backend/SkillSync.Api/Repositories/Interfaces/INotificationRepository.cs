using SkillSync.Api.Models;

namespace SkillSync.Api.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserIdAsync(Guid userId);
}
