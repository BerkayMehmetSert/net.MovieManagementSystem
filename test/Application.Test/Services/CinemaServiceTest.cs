using Application.Contracts.Constants.Cinema;
using Application.Contracts.Requests.Cinema;
using Application.Contracts.Validations.Cinema;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class CinemaServiceTest : CinemaMockRepository
{
    private readonly CreateCinemaRequestValidator _createCinemaRequestValidator;
    private readonly UpdateCinemaAddressRequestValidator _updateCinemaAddressRequestValidator;
    private readonly UpdateCinemaNameRequestValidator _updateCinemaNameRequestValidator;
    private readonly CinemaService _service;

    public CinemaServiceTest(
        CinemaFakeData fakeData,
        CreateCinemaRequestValidator createCinemaRequestValidator,
        UpdateCinemaAddressRequestValidator updateCinemaAddressRequestValidator,
        UpdateCinemaNameRequestValidator updateCinemaNameRequestValidator
    ) : base(fakeData)
    {
        _createCinemaRequestValidator = createCinemaRequestValidator;
        _updateCinemaAddressRequestValidator = updateCinemaAddressRequestValidator;
        _updateCinemaNameRequestValidator = updateCinemaNameRequestValidator;
        _service = new CinemaService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object
        );
    }

    [Fact]
    public void CreateCinemaValidRequestShouldReturnSuccess()
    {
        var request = new CreateCinemaRequest { Name = "Test Cinema" };
        _service.CreateCinema(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Cinema>()), Times.Once);
    }

    [Fact]
    public void CreateCinemaValidRequestShouldThrowCinemaAlreadyExistsException()
    {
        var request = new CreateCinemaRequest { Name = "Cinema 1" };
        var exception = Assert.Throws<BusinessException>(() => _service.CreateCinema(request));
        Assert.Equal(CinemaBusinessMessages.CinemaAlreadyExists, exception.Message);
    }

    [Fact]
    public void CreateCinemaInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateCinemaRequest { Name = "" };
        var result = _createCinemaRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(CinemaValidationMessages.NameRequired, result);
    }
    
    [Fact]
    public void UpdateCinemaAddressValidRequestShouldReturnSuccess()
    {
        var request = new UpdateCinemaAddressRequest { Address = "Test Address" };
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateCinemaAddress(cinemaId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Cinema>()), Times.Once);
    }
    
    [Fact]
    public void UpdateCinemaAddressValidRequestShouldThrowCinemaNotFoundException()
    {
        var request = new UpdateCinemaAddressRequest { Address = "Test Address" };
        var cinemaId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateCinemaAddress(cinemaId, request));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }
    
    [Fact]
    public void UpdateCinemaAddressInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateCinemaAddressRequest { Address = "" };
        var result = _updateCinemaAddressRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Address")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(CinemaValidationMessages.AddressRequired, result);
    }
    
    [Fact]
    public void UpdateCinemaNameValidRequestShouldReturnSuccess()
    {
        var request = new UpdateCinemaNameRequest { Name = "Test Cinema" };
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateCinemaName(cinemaId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Cinema>()), Times.Once);
    }

    [Fact]
    public void UpdateCinemaNameValidRequestShouldThrowCinemaAlreadyExistsException()
    {
        var request = new UpdateCinemaNameRequest { Name = "Cinema 2" };
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var exception = Assert.Throws<BusinessException>(() => _service.UpdateCinemaName(cinemaId, request));
        Assert.Equal(CinemaBusinessMessages.CinemaAlreadyExists, exception.Message);
    }
    
    [Fact]
    public void UpdateCinemaNameValidRequestShouldThrowCinemaNotFoundException()
    {
        var request = new UpdateCinemaNameRequest { Name = "Test Cinema" };
        var cinemaId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateCinemaName(cinemaId, request));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }
    
    [Fact]
    public void UpdateCinemaNameInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateCinemaNameRequest { Name = "" };
        var result = _updateCinemaNameRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Name")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(CinemaValidationMessages.NameRequired, result);
    }
    
    [Fact]
    public void DeleteCinemaValidRequestShouldReturnSuccess()
    {
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteCinema(cinemaId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Cinema>()), Times.Once);
    }

    [Fact]
    public void DeleteCinemaValidRequestShouldThrowCinemaHasMoviesException()
    {
        var cinemaId = new Guid("22222222-2222-2222-2222-222222222222");
        var exception = Assert.Throws<BusinessException>(() => _service.DeleteCinema(cinemaId));
        Assert.Equal(CinemaBusinessMessages.CinemaHasMovies, exception.Message);
    }
    
    [Fact]
    public void DeleteCinemaValidRequestShouldThrowCinemaNotFoundException()
    {
        var cinemaId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteCinema(cinemaId));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetCinemaByIdValidRequestShouldReturnSuccess()
    {
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetCinemaById(cinemaId);
        Assert.NotNull(result);
        Assert.Equal(cinemaId, result.Id);
    }
    
    [Fact]
    public void GetCinemaByIdValidRequestShouldThrowCinemaNotFoundException()
    {
        var cinemaId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetCinemaById(cinemaId));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }
    
    [Fact]
    public void GetCinemaByNameValidRequestShouldReturnSuccess()
    {
        const string cinemaName = "Cinema 1";
        var result = _service.GetCinemaByName(cinemaName);
        Assert.NotNull(result);
        Assert.Equal(cinemaName, result.Name);
    }
    
    [Fact]
    public void GetCinemaByNameValidRequestShouldThrowCinemaNotFoundException()
    {
        const string cinemaName = "Test Cinema";
        var exception = Assert.Throws<NotFoundException>(() => _service.GetCinemaByName(cinemaName));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundByName, exception.Message);
    }
    
    [Fact]
    public void GetAllCinemasValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllCinemas();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void GetCinemaEntityByIdValidRequestShouldReturnSuccess()
    {
        var cinemaId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetCinemaEntityById(cinemaId);
        Assert.NotNull(result);
        Assert.Equal(cinemaId, result.Id);
    }
    
    [Fact]
    public void GetCinemaEntityByIdValidRequestShouldThrowCinemaNotFoundException()
    {
        var cinemaId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetCinemaEntityById(cinemaId));
        Assert.Equal(CinemaBusinessMessages.CinemaNotFoundById, exception.Message);
    }
}