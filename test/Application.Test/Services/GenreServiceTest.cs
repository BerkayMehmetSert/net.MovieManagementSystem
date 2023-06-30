using Application.Contracts.Constants.Genre;
using Application.Contracts.Requests.Genre;
using Application.Contracts.Validations.Genre;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.Application.Caching;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class GenreServiceTest : GenreMockRepository
{
    private readonly Mock<ICacheService> _cacheService = new();
    private readonly CreateGenreRequestValidator _createGenreRequestValidator;
    private readonly UpdateGenreRequestValidator _updateGenreRequestValidator;
    private readonly GenreService _service;

    public GenreServiceTest(
        GenreFakeData fakeData,
        CreateGenreRequestValidator createGenreRequestValidator,
        UpdateGenreRequestValidator updateGenreRequestValidator
    ) : base(fakeData)
    {
        _createGenreRequestValidator = createGenreRequestValidator;
        _updateGenreRequestValidator = updateGenreRequestValidator;
        _service = new GenreService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object,
            _cacheService.Object
        );
    }

    [Fact]
    public void CreateGenreValidRequestShouldReturnSuccess()
    {
        var request = new CreateGenreRequest { Name = "Test Genre" };
        _service.CreateGenre(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Genre>()), Times.Once);
    }

    [Fact]
    public void CreateGenreValidRequestRemoveCacheShouldReturnSuccess()
    {
        var request = new CreateGenreRequest { Name = "Test Genre" };
        _service.CreateGenre(request);
        _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void CreateGenreValidRequestShouldThrowGenreAlreadyExistsException()
    {
        var request = new CreateGenreRequest { Name = "Genre 2" };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateGenre(request));
        Assert.Equal(GenreBusinessMessages.GenreAlreadyExistsByName, exception.Message);
    }

    [Fact]
    public void CreateGenreInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateGenreRequest { Name = "" };
        var result = _createGenreRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(GenreValidationMessages.NameRequired, result);
    }

    [Fact]
    public void UpdateGenreValidRequestShouldReturnSuccess()
    {
        var request = new UpdateGenreRequest { Name = "Test Genre" };
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateGenre(genreId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Genre>()), Times.Once);
    }
    
    [Fact]
    public void UpdateGenreValidRequestRemoveCacheShouldReturnSuccess()
    {
        var request = new UpdateGenreRequest { Name = "Test Genre" };
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateGenre(genreId, request);
        _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void UpdateGenreValidRequestShouldThrowGenreAlreadyExistsException()
    {
        var request = new UpdateGenreRequest { Name = "Genre 2" };
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateGenre(genreId, request));
        Assert.Equal(GenreBusinessMessages.GenreAlreadyExistsByName, exception.Message);
    }
    
    [Fact]
    public void UpdateGenreValidRequestShouldThrowGenreNotFoundException()
    {
        var request = new UpdateGenreRequest { Name = "Test Genre" };
        var genreId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateGenre(genreId, request));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundById, exception.Message);
    }
    
    [Fact]
    public void UpdateGenreInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateGenreRequest { Name = "" };
        var result = _updateGenreRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(GenreValidationMessages.NameRequired, result);
    }
    
    [Fact]
    public void DeleteGenreValidRequestShouldReturnSuccess()
    {
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteGenre(genreId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Genre>()), Times.Once);
    }
    
    [Fact]
    public void DeleteGenreValidRequestRemoveCacheShouldReturnSuccess()
    {
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteGenre(genreId);
        _cacheService.Verify(x => x.Remove(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public void DeleteGenreValidRequestShouldThrowGenreNotFoundException()
    {
        var genreId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteGenre(genreId));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetGenreByIdValidRequestShouldReturnSuccess()
    {
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetGenreById(genreId);
        Assert.NotNull(result);
        Assert.Equal(genreId, result.Id);
    }
    
    [Fact]
    public void GetGenreByIdValidRequestGetCacheShouldReturnSuccess()
    {
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var cacheKey = $"Genre:{genreId}";
        _cacheService.Setup(x => x.TryGet(cacheKey, out It.Ref<object>.IsAny)).Returns(true);
        _service.GetGenreById(genreId);
        _cacheService.Verify(x => x.TryGet(cacheKey, out It.Ref<object>.IsAny), Times.Once);
    }
    
    [Fact]
    public void GetGenreByIdValidRequestSetCacheShouldReturnSuccess()
    {
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var cacheKey = $"Genre:{genreId}";
        _service.GetGenreById(genreId);
        _cacheService.Verify(x => x.Set(cacheKey, It.IsAny<object>()), Times.Once);
    }
    
    [Fact]
    public void GetGenreByIdValidRequestShouldThrowGenreNotFoundException()
    {
        var genreId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetGenreById(genreId));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetGenreByNameValidRequestShouldReturnSuccess()
    {
        const string genreName = "Genre 1";
        var result = _service.GetGenreByName(genreName);
        Assert.NotNull(result);
        Assert.Equal(genreName, result.Name);
    }
    
    [Fact]
    public void GetGenreByNameValidRequestGetCacheShouldReturnSuccess()
    {
        const string genreName = "Genre 1";
        const string cacheKey = $"Genre:{genreName}";
        _service.GetGenreByName(genreName);
        _cacheService.Verify(x => x.TryGet(cacheKey, out It.Ref<object>.IsAny), Times.Once);
    }
    
    [Fact]
    public void GetGenreByNameValidRequestSetCacheShouldReturnSuccess()
    {
        const string genreName = "Genre 1";
        const string cacheKey = $"Genre:{genreName}";
        _service.GetGenreByName(genreName);
        _cacheService.Verify(x => x.Set(cacheKey, It.IsAny<object>()), Times.Once);
    }
    
    [Fact]
    public void GetGenreByNameValidRequestShouldThrowGenreNotFoundException()
    {
        const string genreName = "Test Genre";
        var exception = Assert.Throws<NotFoundException>(() => _service.GetGenreByName(genreName));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundByName, exception.Message);
    }
    
    [Fact]
    public void GetAllGenresValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllGenres();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void GetAllGenresValidRequestGetCacheShouldReturnSuccess()
    {
        const string cacheKey = "Genre:All";
        _cacheService.Setup(x => x.TryGet(cacheKey, out It.Ref<object>.IsAny)).Returns(true);
        var result = _service.GetAllGenres();
        _cacheService.Verify(x => x.TryGet(cacheKey, out It.Ref<object>.IsAny), Times.Once);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void GetGenreEntityByIdValidRequestShouldReturnSuccess()
    {
        var genreId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetGenreEntityById(genreId);
        Assert.NotNull(result);
        Assert.Equal(genreId, result.Id);
    }
    
    [Fact]
    public void GetGenreEntityByIdValidRequestShouldThrowGenreNotFoundException()
    {
        var genreId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetGenreEntityById(genreId));
        Assert.Equal(GenreBusinessMessages.GenreNotFoundById, exception.Message);
    }
}