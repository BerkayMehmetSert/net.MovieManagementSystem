using Application.Contracts.Constants.Rating;
using Application.Contracts.Requests.Rating;
using Application.Contracts.Validations.Rating;
using Application.Services;
using Application.Test.Mocks.FakeData;
using Application.Test.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Test.Services;

public class RatingServiceTest : RatingMockRepository
{
    private readonly CreateRatingRequestValidator _createRatingRequestValidator;
    private readonly UpdateRatingRequestValidator _updateRatingRequestValidator;
    private readonly RatingService _service;

    public RatingServiceTest(
        RatingFakeData fakeData,
        CreateRatingRequestValidator createRatingRequestValidator,
        UpdateRatingRequestValidator updateRatingRequestValidator
    ) : base(fakeData)
    {
        _createRatingRequestValidator = createRatingRequestValidator;
        _updateRatingRequestValidator = updateRatingRequestValidator;
        _service = new RatingService(
            MockRepository.Object,
            Mapper,
            UnitOfWork.Object
        );
    }

    [Fact]
    public void CreateRatingValidRequestShouldReturnSuccess()
    {
        var request = new CreateRatingRequest { Score = 5 };
        _service.CreateRating(request);
        MockRepository.Verify(x => x.Add(It.IsAny<Rating>()), Times.Once);
    }

    [Fact]
    public void CreateRatingInvalidRequestShouldThrowValidationException()
    {
        var request = new CreateRatingRequest();
        var result = _createRatingRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Score")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(RatingValidationMessages.ScoreRequired, result);
    }

    [Fact]
    public void UpdateRatingValidRequestShouldReturnSuccess()
    {
        var request = new UpdateRatingRequest { Score = 5 };
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.UpdateRating(ratingId, request);
        MockRepository.Verify(x => x.Update(It.IsAny<Rating>()), Times.Once);
    }

    [Fact]
    public void UpdateRatingValidRequestShouldThrowRatingNotFoundException()
    {
        var request = new UpdateRatingRequest { Score = 5 };
        var ratingId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.UpdateRating(ratingId, request));
        Assert.Equal(RatingBusinessMessages.RatingNotFoundById, exception.Message);
    }

    [Fact]
    public void UpdateRatingInvalidRequestShouldThrowValidationException()
    {
        var request = new UpdateRatingRequest();
        var result = _updateRatingRequestValidator
            .Validate(request)
            .Errors.Where(x => x.PropertyName == "Score")
            .Select(x => x.ErrorMessage)
            .FirstOrDefault();
        Assert.NotNull(result);
        Assert.Equal(RatingValidationMessages.ScoreRequired, result);
    }

    [Fact]
    public void DeleteRatingValidRequestShouldReturnSuccess()
    {
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        _service.DeleteRating(ratingId);
        MockRepository.Verify(x => x.Delete(It.IsAny<Rating>()), Times.Once);
    }

    [Fact]
    public void DeleteRatingValidRequestShouldThrowRatingNotFoundException()
    {
        var ratingId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.DeleteRating(ratingId));
        Assert.Equal(RatingBusinessMessages.RatingNotFoundById, exception.Message);
    }

    [Fact]
    public void GetRatingByIdValidRequestShouldReturnSuccess()
    {
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetRatingById(ratingId);
        Assert.NotNull(result);
        Assert.Equal(ratingId, result.Id);
    }

    [Fact]
    public void GetRatingByIdValidRequestShouldThrowRatingNotFoundException()
    {
        var ratingId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetRatingById(ratingId));
        Assert.Equal(RatingBusinessMessages.RatingNotFoundById, exception.Message);
    }

    [Fact]
    public void GetAllRatingsValidRequestShouldReturnSuccess()
    {
        var result = _service.GetAllRatings();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetRatingEntityByIdValidRequestShouldReturnSuccess()
    {
        var ratingId = new Guid("11111111-1111-1111-1111-111111111111");
        var result = _service.GetRatingEntityById(ratingId);
        Assert.NotNull(result);
        Assert.Equal(ratingId, result.Id);
    }

    [Fact]
    public void GetRatingEntityByIdValidRequestShouldThrowRatingNotFoundException()
    {
        var ratingId = Guid.Empty;
        var exception = Assert.Throws<NotFoundException>(() => _service.GetRatingEntityById(ratingId));
        Assert.Equal(RatingBusinessMessages.RatingNotFoundById, exception.Message);
    }
}