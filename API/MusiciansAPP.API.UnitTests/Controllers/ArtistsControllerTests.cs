using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusiciansAPP.API.Controllers;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.DAL.Exceptions;
using NUnit.Framework;

namespace MusiciansAPP.API.UnitTests.Controllers;

[TestFixture]
public class ArtistsControllerTests
{
    private const int PageSize = 2;
    private const int Page = 1;

    private const string ArtistOneName = "Artist One Mock Name";
    private const string ArtistOneImageUrl = "Artist One Mock ImageUrl";
    private const string ArtistTwoName = "Artist Two Mock Name";
    private const string ArtistTwoImageUrl = "Artist Two Mock ImageUrl";

    private const string ArtistDetailsName = "Artist Details Mock Name";
    private const string ArtistDetailsImageUrl = "Artist Details Mock ImageUrl";
    private const string ArtistDetailsBiography = "Artist Details Mock Biography";

    private const string ArtistNameForSimilar = "Mock Artist Name For Similar";

    private ArtistsController _controller;
    private Mock<IErrorHandler> _errorHandlerMock;
    private Mock<IArtistsService> _artistsServiceMock;
    private Mock<IPagingHelper> _pagingHelperMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _errorHandlerMock = new Mock<IErrorHandler>();

        _pagingHelperMock = new Mock<IPagingHelper>();
        _pagingHelperMock.Setup(p => p.GetCorrectPageSize(PageSize)).Returns(PageSize);

        _artistsServiceMock = new Mock<IArtistsService>();

        SetUpArtistsServiceMockGetTopArtists();
        SetUpArtistsServiceMockGeArtistDetails();
        SetUpArtistsServiceMockGetSimilarArtists();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.API")));

        _controller = new ArtistsController(
            _errorHandlerMock.Object,
            _artistsServiceMock.Object,
            _mapper,
            _pagingHelperMock.Object);
    }

    [Test]
    public async Task GetTopArtists_ArtistsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetTopArtists(PageSize, Page);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetTopArtists_ArtistsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetTopArtists(PageSize, Page);
        var value = (result.Result as ObjectResult).Value as IEnumerable<ArtistUI>;

        // Assert
        VerifyTopArtistsAreCorrect(value);
    }

    [Test]
    public async Task GetTopArtists_ArtistsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowNotFoundExceptionOnGetTopArtists();

        // Act
        var result = await _controller.GetTopArtists(default, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetTopArtists_ArtistsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetTopArtists();

        // Act
        await _controller.GetTopArtists(default, default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetAlbumsByDate_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetTopArtists();

        // Act
        var result = await _controller.GetTopArtists(default, default);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task GetArtistDetails_ArtistsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetArtistDetails(ArtistDetailsName);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetArtistDetails_ArtistsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetArtistDetails(ArtistDetailsName);
        var value = (result.Result as ObjectResult).Value as ArtistUI;

        // Assert
        VerifyArtistDetailsAreCorrect(value);
    }

    [Test]
    public async Task GetArtistDetails_ArtistsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowNotFoundExceptionOnGetArtistDetails();

        // Act
        var result = await _controller.GetArtistDetails(default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetArtistDetails_ArtistsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetArtistDetails();

        // Act
        await _controller.GetArtistDetails(default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetArtistDetails_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetArtistDetails();

        // Act
        var result = await _controller.GetArtistDetails(default);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task GetSimilarArtists_ArtistsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetSimilarArtists(ArtistNameForSimilar);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetSimilarArtists_ArtistsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetSimilarArtists(ArtistNameForSimilar);
        var value = (result.Result as ObjectResult).Value as IEnumerable<ArtistUI>;

        // Assert
        VerifyTopArtistsAreCorrect(value);
    }

    [Test]
    public async Task GetSimilarArtists_ArtistsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowNotFoundExceptionOnGetSimilarArtists();

        // Act
        var result = await _controller.GetSimilarArtists(ArtistNameForSimilar);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetSimilarArtists_ArtistsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetSimilarArtists();

        // Act
        await _controller.GetSimilarArtists(ArtistNameForSimilar);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetSimilarArtists_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetSimilarArtists();

        // Act
        var result = await _controller.GetSimilarArtists(ArtistNameForSimilar);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    private static void VerifyTopArtistsAreCorrect(IEnumerable<ArtistUI> model)
    {
        var artists = model.ToList();
        Assert.That(artists[0].Name, Is.EqualTo(ArtistOneName));
        Assert.That(artists[0].ImageUrl, Is.EqualTo(ArtistOneImageUrl));

        Assert.That(artists[1].Name, Is.EqualTo(ArtistTwoName));
        Assert.That(artists[1].ImageUrl, Is.EqualTo(ArtistTwoImageUrl));
    }

    private static void VerifyArtistDetailsAreCorrect(ArtistUI artist)
    {
        Assert.That(artist.Name, Is.EqualTo(ArtistDetailsName));
        Assert.That(artist.ImageUrl, Is.EqualTo(ArtistDetailsImageUrl));
        Assert.That(artist.Biography, Is.EqualTo(ArtistDetailsBiography));
    }

    private void SetUpArtistsServiceMockGetTopArtists()
    {
        var artists = new List<ArtistBL>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl },
        };
        _artistsServiceMock.Setup(a => a.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(artists);
    }

    private void SetUpArtistsServiceMockGeArtistDetails()
    {
        var artistDetails = new ArtistBL
        {
            Name = ArtistDetailsName,
            ImageUrl = ArtistDetailsImageUrl,
            Biography = ArtistDetailsBiography,
        };
        _artistsServiceMock
            .Setup(a => a.GetArtistDetailsAsync(ArtistDetailsName))
            .ReturnsAsync(artistDetails);
    }

    private void SetUpArtistsServiceMockGetSimilarArtists()
    {
        var similarArtists = new List<ArtistBL>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl },
        };
        _artistsServiceMock.Setup(a => a.GetSimilarArtistsAsync(
            ArtistNameForSimilar, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(similarArtists);
    }

    private void SetUpArtistsServiceToThrowNotFoundExceptionOnGetTopArtists()
    {
        _artistsServiceMock.Setup(a => a.GetTopArtistsAsync(
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<NotFoundException>();
    }

    private void SetUpArtistsServiceToThrowExceptionOnGetTopArtists()
    {
        _artistsServiceMock.Setup(a => a.GetTopArtistsAsync(
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<Exception>();
    }

    private void SetUpArtistsServiceToThrowNotFoundExceptionOnGetArtistDetails()
    {
        _artistsServiceMock.Setup(a => a.GetArtistDetailsAsync(It.IsAny<string>()))
            .Throws<NotFoundException>();
    }

    private void SetUpArtistsServiceToThrowExceptionOnGetArtistDetails()
    {
        _artistsServiceMock.Setup(a => a.GetArtistDetailsAsync(It.IsAny<string>()))
            .Throws<Exception>();
    }

    private void SetUpArtistsServiceToThrowNotFoundExceptionOnGetSimilarArtists()
    {
        _artistsServiceMock.Setup(a => a.GetSimilarArtistsAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<NotFoundException>();
    }

    private void SetUpArtistsServiceToThrowExceptionOnGetSimilarArtists()
    {
        _artistsServiceMock.Setup(a => a.GetSimilarArtistsAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<Exception>();
    }
}
