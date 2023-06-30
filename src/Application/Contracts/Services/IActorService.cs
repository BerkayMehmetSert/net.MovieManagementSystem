using Application.Contracts.Requests.Actor;
using Application.Contracts.Responses;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface IActorService
{
    void CreateActor(CreateActorRequest request);
    void UpdateActor(Guid id, UpdateActorRequest request);
    void AddActorAward(Guid id, AddActorAwardRequest request);
    void RemoveActorAward(Guid id, RemoveActorAwardRequest request);
    void DeleteActor(Guid id);
    ActorResponse GetActorById(Guid id);
    List<ActorResponse> GetAllActors();
    Actor GetActorEntityById(Guid id);
}