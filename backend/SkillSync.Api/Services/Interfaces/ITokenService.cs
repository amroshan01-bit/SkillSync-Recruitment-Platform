using SkillSync.Api.Models;

namespace SkillSync.Api.Services.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(User user);
}
