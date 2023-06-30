using Application.Contracts.Constants.Language;
using Application.Contracts.Requests.Language;
using Application.Contracts.Validations.Language;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class LanguageServiceTest : LanguageMockRepository
{
    private readonly Mock<ICacheService> _cacheService = new();
    private readonly CreateLanguageRequestValidator _createLanguageRequestValidator;
    private readonly UpdateLanguageRequestValidator _updateLanguageRequestValidator;
    private readonly LanguageService _service;

    public LanguageServiceTest(
        LanguageFakeData fakeData,
        CreateLanguageRequestValidator createLanguageRequestValidator,
        UpdateLanguageRequestValidator updateLanguageRequestValidator) : base(fakeData)
    {
        _createLanguageRequestValidator = createLanguageRequestValidator;
        _updateLanguageRequestValidator = updateLanguageRequestValidator;
        _service = new LanguageService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object,
            _cacheService.Object
        );
    }

    [Fact]
    public void CreateLanguageValidRequestShouldReturnSuccess()
    {
        var request = new CreateLanguageRequest { Name = "Test Language" };
        _service.CreateLanguage(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Language>()), Times.Once);
    }

    [Fact]
    public void CreateLanguageValidRequestRemoveCacheShouldReturnSuccess()
    {
        var request = new CreateLanguageRequest { Name = "Test Language" };
        _service.CreateLanguage(request);
        _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void CreateLanguageValidRequestShouldThrowLanguageAlreadyExistsException()
    {
        var request = new CreateLanguageRequest { Name = "Language 1" };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateLanguage(request));
        Assert.Equal(LanguageBusinessMessages.LanguageAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void CreateLanguageInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateLanguageRequest { Name = "" };
        var result = _createLanguageRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(LanguageValidationMessages.NameRequired, result);
    }

    [Fact]
    public void UpdateLanguageValidRequestShouldReturnSuccess()
    {
        var request = new UpdateLanguageRequest { Name = "Test Language" };
        _service.UpdateLanguage(new Guid("11111111-1111-1111-1111-111111111111"), request);
        MockRepository.Verify(x => x.Update(It.IsAny<Language>()), Times.Once);
    }

    [Fact]
    public void UpdateLanguageValidRequestRemoveCacheShouldReturnSuccess()
    {
        var request = new UpdateLanguageRequest { Name = "Test Language" };
        _service.UpdateLanguage(new Guid("11111111-1111-1111-1111-111111111111"), request);
        _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void UpdateLanguageValidRequestShouldThrowLanguageAlreadyExistsException()
    {
        var request = new UpdateLanguageRequest { Name = "Language 2" };
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateLanguage(languageId, request));
        Assert.Equal(LanguageBusinessMessages.LanguageAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void UpdateLanguageInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateLanguageRequest { Name = "" };
        var result = _updateLanguageRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(LanguageValidationMessages.NameRequired, result);
    }
    
    [Fact]
    public void DeleteLanguageValidRequestShouldReturnSuccess()
    {
        _service.DeleteLanguage(new Guid("11111111-1111-1111-1111-111111111111"));
        MockRepository.Verify(x => x.Delete(It.IsAny<Language>()), Times.Once);
    }
    
    [Fact]
    public void DeleteLanguageValidRequestRemoveCacheShouldReturnSuccess()
    {
        _service.DeleteLanguage(new Guid("11111111-1111-1111-1111-111111111111"));
        _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void DeleteLanguageValidRequestShouldThrowLanguageNotFoundException()
    {
        var languageId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteLanguage(languageId));
        Assert.Equal(LanguageBusinessMessages.LanguageNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetLanguageByIdValidRequestShouldReturnSuccess()
    {
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetLanguageById(languageId);
        Assert.NotNull(result);
        Assert.Equal(languageId, result.Id);
    }
    
    [Fact]
    public void GetLanguageByIdValidRequestShouldThrowLanguageNotFoundException()
    {
        var languageId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetLanguageById(languageId));
        Assert.Equal(LanguageBusinessMessages.LanguageNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetLanguageByNameValidRequestShouldReturnSuccess()
    {
        const string languageName = "Language 1";
        var result = _service.GetLanguageByName(languageName);
        Assert.NotNull(result);
        Assert.Equal(languageName, result.Name);
    }
    
    [Fact]
    public void GetLanguageByNameValidRequestShouldThrowLanguageNotFoundException()
    {
        const string languageName = "Test Language";
        var exception = Assert.Throws<NotFoundException>(() => _service.GetLanguageByName(languageName));
        Assert.Equal(LanguageBusinessMessages.LanguageNotFoundByName, exception.Message);
    }
    
    [Fact]
    public void GetAllLanguagesValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllLanguages();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetLanguageEntityByIdValidRequestShouldReturnSuccess()
    {
        var languageId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetLanguageEntityById(languageId);
        Assert.NotNull(result);
        Assert.Equal(languageId, result.Id);
    }
    
    [Fact]
    public void GetLanguageEntityByIdValidRequestShouldThrowLanguageNotFoundException()
    {
        var languageId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetLanguageEntityById(languageId));
        Assert.Equal(LanguageBusinessMessages.LanguageNotFoundById, exception.Message);
    }
}