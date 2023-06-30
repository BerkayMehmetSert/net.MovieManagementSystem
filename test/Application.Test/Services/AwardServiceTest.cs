using Application.Contracts.Constants.Award;
using Application.Contracts.Requests.Award;
using Application.Contracts.Validations.Award;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class AwardServiceTest : AwardMockRepository
{
    private readonly CreateAwardRequestValidator _createAwardRequestValidator;
    private readonly UpdateAwardRequestValidator _updateAwardRequestValidator;
    private readonly AwardService _service;

    public AwardServiceTest(
        AwardFakeData fakeData,
        CreateAwardRequestValidator createAwardRequestValidator,
        UpdateAwardRequestValidator updateAwardRequestValidator
    ) : base(fakeData)
    {
        _createAwardRequestValidator = createAwardRequestValidator;
        _updateAwardRequestValidator = updateAwardRequestValidator;
        _service = new AwardService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object
        );
    }

    [Fact]
    public void CreateAwardValidRequestShouldReturnSuccess()
    {
        var request = new CreateAwardRequest { Name = "Test Award" };
        _service.CreateAward(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Award>()), Times.Once);
    }

    [Fact]
    public void CreateAwardValidRequestShouldThrowAwardAlreadyExistsException()
    {
        var request = new CreateAwardRequest { Name = "Award 1" };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateAward(request));
        Assert.Equal(AwardBusinessMessages.AwardAlreadyExistsByName, exception.Message);
    }
    
    [Fact]
    public void CreateAwardInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateAwardRequest{Name = ""};
        var result = _createAwardRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(AwardValidationMessages.NameRequired, result);
    }

    [Fact]
    public void UpdateAwardValidRequestShouldReturnSuccess()
    {
        var request = new UpdateAwardRequest { Name = "Test Award" };
        var awardId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateAward(awardId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Award>()), Times.Once);
    }

    [Fact]
    public void UpdateAwardValidRequestShouldThrowAwardAlreadyExistsException()
    {
        var request = new UpdateAwardRequest { Name = "Award 2" };
        var awardId = new Guid("11111111-1111-1111-1111-111111111111");
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateAward(awardId, request));
        Assert.Equal(AwardBusinessMessages.AwardAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void UpdateAwardValidRequestShouldThrowAwardNotFoundException()
    {
        var request = new UpdateAwardRequest { Name = "Test Award" };
        var awardId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateAward(awardId, request));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateAwardInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateAwardRequest { Name = "" };
        var result = _updateAwardRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(AwardValidationMessages.NameRequired, result);
    }

    [Fact]
    public void DeleteAwardValidRequestShouldReturnSuccess()
    {
        var awardId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteAward(awardId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Award>()), Times.Once);
    }

    [Fact]
    public void DeleteAwardValidRequestShouldThrowAwardHasActorsException()
    {
        var awardId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _service.DeleteAward(awardId));
        Assert.Equal(AwardBusinessMessages.AwardHasActors, exception.Message);
    }

    [Fact]
    public void DeleteAwardValidRequestShouldThrowAwardNotFoundException()
    {
        var awardId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteAward(awardId));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetAwardByIdValidRequestShouldReturnSuccess()
    {
        var awardId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetAwardById(awardId);
        Assert.NotNull(result);
        Assert.Equal(awardId, result.Id);
    }
    
    [Fact]
    public void GetAwardByIdValidRequestShouldThrowAwardNotFoundException()
    {
        var awardId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetAwardById(awardId));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetAwardByNameValidRequestShouldReturnSuccess()
    {
        const string name = "Award 1";
        var result = _service.GetAwardByName(name);
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
    }
    
    [Fact]
    public void GetAwardByNameValidRequestShouldThrowAwardNotFoundException()
    {
        const string name = "Award 3";
        var exception = Assert.Throws<NotFoundException>(() => _service.GetAwardByName(name));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundByName, exception.Message);
    }
    
    [Fact]
    public void GetAllAwardsValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllAwards();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void GetAllAwardsOrderedByDateAscValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllAwardsOrderedByDateAsc();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Award 1", result[0].Name);
        Assert.Equal("Award 2", result[1].Name);
    }

    [Fact]
    public void GetAllAwardsOrderedByDateDescValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllAwardsOrderedByDateDesc();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Award 2", result[0].Name);
        Assert.Equal("Award 1", result[1].Name);
    }
    
    [Fact]
    public void GetAwardEntityByIdValidRequestShouldReturnSuccess()
    {
        var awardId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetAwardEntityById(awardId);
        Assert.NotNull(result);
        Assert.Equal(awardId, result.Id);
    }
    
    [Fact]
    public void GetAwardEntityByIdValidRequestShouldThrowAwardNotFoundException()
    {
        var awardId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetAwardEntityById(awardId));
        Assert.Equal(AwardBusinessMessages.AwardNotFoundById, exception.Message);
    }
}