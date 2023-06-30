using Application.Contracts.Constants.Actor;
using Application.Contracts.Constants.Award;
using Application.Contracts.Requests.Actor;
using Application.Contracts.Services;
using Application.Contracts.Validations.Actor;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class ActorServiceTest : ActorMockRepository
{
    private readonly Mock<IAwardService> _awardService = new();

    private readonly CreateActorRequestValidator _createActorRequestValidator;
    private readonly UpdateActorRequestValidator _updateActorRequestValidator;
    private readonly ActorService _service;

    public ActorServiceTest(
        ActorFakeData fakeData,
        CreateActorRequestValidator createActorRequestValidator,
        UpdateActorRequestValidator updateActorRequestValidator
    ) : base(fakeData)
    {
        _createActorRequestValidator = createActorRequestValidator;
        _updateActorRequestValidator = updateActorRequestValidator;
        _service = new ActorService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object,
            _awardService.Object
        );
    }

    [Fact]
    public void CreateActorValidRequestShouldReturnSuccess()
    {
        var request = new CreateActorRequest { FirstName = "Test Actor" };
        _service.CreateActor(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Actor>()), Times.Once);
    }

    [Fact]
    public void CreateActorValidRequestShouldThrowDateOfBirthIsInTheFutureException()
    {
        var request = new CreateActorRequest { FirstName = "Test Actor", DateOfBirth = DateTime.Now.AddDays(1) };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateActor(request));
        Assert.Equal(ActorBusinessMessages.ActorDateOfBirthIsInTheFuture, exception.Message);
    }

    [Fact]
    public void CreateActorInValidRequestShouldThrowValidationException()
    {
        var request = new CreateActorRequest { FirstName = "" };
        var result = _createActorRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "FirstName")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(ActorValidationMessages.FirstNameRequired, result);
    }

    [Fact]
    public void UpdateActorValidRequestShouldReturnSuccess()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateActorRequest { FirstName = "Test Actor" };
        _service.UpdateActor(actorId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Actor>()), Times.Once);
    }

    [Fact]
    public void UpdateActorValidRequestShouldThrowDateOfBirthIsInTheFutureException()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new UpdateActorRequest { FirstName = "Test Actor", DateOfBirth = DateTime.Now.AddDays(1) };
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateActor(actorId, request));
        Assert.Equal(ActorBusinessMessages.ActorDateOfBirthIsInTheFuture, exception.Message);
    }

    [Fact]
    public void UpdateActorValidRequestShouldThrowActorNotFoundException()
    {
        var actorId = Guid.Empty;
        var request = new UpdateActorRequest { FirstName = "Test Actor" };
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateActor(actorId, request));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateActorInValidRequestShouldThrowValidationException()
    {
        var request = new UpdateActorRequest { FirstName = "" };
        var result = _updateActorRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "FirstName")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(ActorValidationMessages.FirstNameRequired, result);
    }

    [Fact]
    public void AddActorAwardValidRequestShouldReturnSuccess()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddActorAwardRequest
        {
            AwardIds = new List<Guid> { new("22222222-2222-2222-2222-222222222222") }
        };
        _awardService.Setup(x => x.GetAwardEntityById(It.IsAny<Guid>())).Returns(new Award());
        _service.AddActorAward(actorId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Actor>()), Times.Once);
    }

    [Fact]
    public void AddActorAwardValidRequestShouldThrowAwardNotFoundException()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new AddActorAwardRequest
        {
            AwardIds = new List<Guid> { new("11111111-1111-1111-1111-111111111111") }
        };
        _awardService.Setup(x => x.GetAwardEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(AwardBusinessMessages.AwardNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.AddActorAward(actorId, request));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundById, exception.Message);
    }

    [Fact]
    public void AddActorAwardValidRequestShouldThrowActorNotFoundException()
    {
        var actorId = Guid.Empty;
        var request = new AddActorAwardRequest
        {
            AwardIds = new List<Guid> { new("11111111-1111-1111-1111-111111111111") }
        };
        _awardService.Setup(x => x.GetAwardEntityById(It.IsAny<Guid>())).Returns(new Award());
        var exception = Assert.Throws<NotFoundException>(() => _service.AddActorAward(actorId, request));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveActorAwardValidRequestShouldReturnSuccess()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveActorAwardRequest
        {
            AwardIds = new List<Guid> { new("22222222-2222-2222-2222-222222222222") }
        };
        _awardService.Setup(x => x.GetAwardEntityById(It.IsAny<Guid>())).Returns(new Award());
        _service.RemoveActorAward(actorId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Actor>()), Times.Once);
    }

    [Fact]
    public void RemoveActorAwardValidRequestShouldThrowAwardNotFoundException()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var request = new RemoveActorAwardRequest
        {
            AwardIds = new List<Guid> { new("11111111-1111-1111-1111-111111111111") }
        };
        _awardService.Setup(x => x.GetAwardEntityById(It.IsAny<Guid>()))
            .Throws(new NotFoundException(AwardBusinessMessages.AwardNotFoundById));
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveActorAward(actorId, request));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundById, exception.Message);
    }

    [Fact]
    public void RemoveActorAwardValidRequestShouldThrowActorNotFoundException()
    {
        var actorId = Guid.Empty;
        var request = new RemoveActorAwardRequest
        {
            AwardIds = new List<Guid> { new("11111111-1111-1111-1111-111111111111") }
        };
        _awardService.Setup(x => x.GetAwardEntityById(It.IsAny<Guid>())).Returns(new Award());
        var exception = Assert.Throws<NotFoundException>(() => _service.RemoveActorAward(actorId, request));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }
    
    [Fact]
    public void DeleteActorValidRequestShouldReturnSuccess()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteActor(actorId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Actor>()), Times.Once);
    }

    [Fact]
    public void DeleteActorValidRequestShouldThrowActorNotFoundException()
    {
        var actorId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteActor(actorId));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetActorByIdValidRequestShouldReturnSuccess()
    {
        var actorId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetActorById(actorId);
        Assert.NotNull(result);
        Assert.Equal(actorId, result.Id);
    }
    
    [Fact]
    public void GetActorByIdValidRequestShouldThrowActorNotFoundException()
    {
        var actorId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetActorById(actorId));
        Assert.Equal(ActorBusinessMessages.ActorNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetAllActorsValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllActors();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
}