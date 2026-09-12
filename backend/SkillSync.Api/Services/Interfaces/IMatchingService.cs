using SkillSync.Api.DTOs.Matching;

namespace SkillSync.Api.Services.Interfaces;

public interface IMatchingService
{
    Task<List<string>> GetSkillsAsync(Guid userId);

    Task<bool> UpdateSkillsAsync(
        Guid userId,
        UpdateJobSeekerSkillsRequest request);

    Task<List<MatchResultDto>> GetMatchesAsync(
        Guid userId);
}