using Application.Contracts.Constants.Director;
using Application.Contracts.Requests.Director;
using Application.Contracts.Validations.Director;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class DirectorServiceTest : DirectorMockRepository
{
    private readonly CreateDirectorRequestValidator _createDirectorRequestValidator;
    private readonly UpdateDirectorRequestValidator _updateDirectorRequestValidator;
    private readonly DirectorService _service;

    public DirectorServiceTest(
        DirectorFakeData fakeData,
        CreateDirectorRequestValidator createDirectorRequestValidator,
        UpdateDirectorRequestValidator updateDirectorRequestValidator
    ) : base(fakeData)
    {
        _createDirectorRequestValidator = createDirectorRequestValidator;
        _updateDirectorRequestValidator = updateDirectorRequestValidator;
        _service = new DirectorService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object
        );
    }

    [Fact]
    public void CreateDirectorValidRequestShouldReturnSuccess()
    {
        var request = new CreateDirectorRequest { FirstName = "Test Director" };
        _service.CreateDirector(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Director>()), Times.Once);
    }

    [Fact]
    public void CreateDirectorValidRequestShouldThrowDirectorDateOfBirthIsInTheFutureException()
    {
        var request = new CreateDirectorRequest { DateOfBirth = DateTime.Now.AddDays(1) };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateDirector(request));
        Assert.Equal(DirectorBusinessMessages.DirectorDateOfBirthIsInTheFuture, exception.Message);
    }

    [Fact]
    public void CreateDirectorInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateDirectorRequest { FirstName = "" };
        var result = _createDirectorRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "FirstName")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(DirectorValidationMessages.FirstNameRequired, result);
    }

    [Fact]
    public void UpdateDirectorValidRequestShouldReturnSuccess()
    {
        var request = new UpdateDirectorRequest { FirstName = "Test Director" };
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateDirector(directorId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Director>()), Times.Once);
    }

    [Fact]
    public void UpdateDirectorValidRequestShouldThrowDirectorDateOfBirthIsInTheFutureException()
    {
        var request = new UpdateDirectorRequest { DateOfBirth = DateTime.Now.AddDays(1) };
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateDirector(directorId, request));
        Assert.Equal(DirectorBusinessMessages.DirectorDateOfBirthIsInTheFuture, exception.Message);
    }

    [Fact]
    public void UpdateDirectorValidRequestShouldThrowDirectorNotFoundException()
    {
        var request = new UpdateDirectorRequest { FirstName = "Test Director" };
        var directorId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateDirector(directorId, request));
        Assert.Equal(DirectorBusinessMessages.DirectorNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateDirectorInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateDirectorRequest { FirstName = "" };
        var result = _updateDirectorRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "FirstName")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(DirectorValidationMessages.FirstNameRequired, result);
    }
    
    [Fact]
    public void DeleteDirectorValidRequestShouldReturnSuccess()
    {
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteDirector(directorId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Director>()), Times.Once);
    }
    
    [Fact]
    public void DeleteDirectorValidRequestShouldThrowDirectorNotFoundException()
    {
        var directorId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteDirector(directorId));
        Assert.Equal(DirectorBusinessMessages.DirectorNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetDirectorByIdValidRequestShouldReturnSuccess()
    {
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetDirectorById(directorId);
        Assert.NotNull(result);
        Assert.Equal(directorId, result.Id);
    }
    
    [Fact]
    public void GetDirectorByIdValidRequestShouldThrowDirectorNotFoundException()
    {
        var directorId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetDirectorById(directorId));
        Assert.Equal(DirectorBusinessMessages.DirectorNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetAllDirectorsValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllDirectors();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
    
    [Fact]
    public void GetDirectorEntityByIdValidRequestShouldReturnSuccess()
    {
        var directorId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetDirectorEntityById(directorId);
        Assert.NotNull(result);
        Assert.Equal(directorId, result.Id);
    }
    
    [Fact]
    public void GetDirectorEntityByIdValidRequestShouldThrowDirectorNotFoundException()
    {
        var directorId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetDirectorEntityById(directorId));
        Assert.Equal(DirectorBusinessMessages.DirectorNotFoundById, exception.Message);
    }
}