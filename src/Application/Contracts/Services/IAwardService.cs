using Application.Contracts.Requests.Award;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IAwardService
{
    void CreateAward(CreateAwardRequest request);
    void UpdateAward(Guid id, UpdateAwardRequest request);
    void DeleteAward(Guid id);
    AwardResponse GetAwardById(Guid id);
    AwardResponse GetAwardByName(string name);
    List<AwardResponse> GetAllAwards();
    List<AwardResponse> GetAllAwardsOrderedByDateAsc();
    List<AwardResponse> GetAllAwardsOrderedByDateDesc();
    Award GetAwardEntityById(Guid id);
}