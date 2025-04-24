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
using MusiciansAPP.BL.Services.Albums;
using MusiciansAPP.BL.Services.Tracks;
using MusiciansAPP.DAL.Exceptions;
using NUnit.Framework;

namespace MusiciansAPP.API.UnitTests.Controllers;

[TestFixture]
public class AlbumsControllerTests
{
    private const int PageSize = 2;
    private const int Page = 1;

    private const string AlbumOneName = "Album One Mock Name";
    private const int AlbumOnePlayCount = 1;
    private const string AlbumOneImageUrl = "Album One Mock ImageUrl";
    private const string AlbumTwoName = "Album Two Mock Name";
    private const int AlbumTwoPlayCount = 2;
    private const string AlbumTwoImageUrl = "Album Two Mock ImageUrl";

    private const string TopAlbumsArtistName = "Top Albums Mock Artist Name";

    private const string AlbumDetailsArtistName = "Album Details Mock Artist Name";
    private const string AlbumDetailsName = "Album Details Mock Name";
    private const string AlbumDetailsImageUrl = "Album Details Mock ImageUrl";
    private const string AlbumDetailsDescription = "Album Details Mock Description";

    private const string AlbumTrackOneName = "Album Track One Mock Name";
    private const int AlbumTrackOneDurationInSeconds = 1;
    private const string AlbumTrackTwoName = "Album Track Two Mock Name";
    private const int AlbumTrackTwoDurationInSeconds = 2;

    private const int AlbumsAmount = 2;
    private readonly DateOnly _albumOneDateCreated = new(2022, 1, 1);
    private readonly DateOnly _albumTwoDateCreated = new(2022, 1, 1);
    private readonly DateOnly _startDate = new(2022, 1, 1);
    private readonly DateOnly _endDate = new(2022, 1, 2);

    private List<AlbumBL> _albums;
    private AlbumsController _controller;
    private Mock<IErrorHandler> _errorHandlerMock;
    private Mock<IAlbumsService> _albumsServiceMock;
    private Mock<IPagingHelper> _pagingHelperMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _errorHandlerMock = new Mock<IErrorHandler>();

        _pagingHelperMock = new Mock<IPagingHelper>();
        _pagingHelperMock.Setup(p => p.GetCorrectPageSize(PageSize)).Returns(PageSize);

        _albumsServiceMock = new Mock<IAlbumsService>();
        SetUpData();
        SetUpAlbumsServiceMockGetGetAlbumsByDates();
        SetUpAlbumsServiceMockGetGetAlbumsByDateAmount();
        SetUpAlbumsServiceMockGetArtistTopAlbums();
        SetUpAlbumsServiceMockGetAlbumDetails();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.API")));

        _controller = new AlbumsController(
            _errorHandlerMock.Object,
            _albumsServiceMock.Object,
            _mapper,
            _pagingHelperMock.Object);
    }

    [Test]
    public async Task GetAlbumsByDate_AlbumsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetAlbumsByDate(
            ConvertDate(_startDate),
            ConvertDate(_endDate),
            PageSize,
            Page);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAlbumsByDate_AlbumsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetAlbumsByDate(
            ConvertDate(_startDate),
            ConvertDate(_endDate),
            PageSize,
            Page);
        var value = (result.Result as ObjectResult).Value as IEnumerable<AlbumUI>;

        // Assert
        VerifyAlbumsAreCorrect(value);
    }

    [Test]
    public async Task GetAlbumsByDate_AlbumsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowNotFoundExceptionOnGetAlbumsByDates();

        // Act
        var result = await _controller.GetAlbumsByDate(default, default, default, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetAlbumsByDate_AlbumsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetAlbumsByDates();

        // Act
        await _controller.GetAlbumsByDate(default, default, default, default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetAlbumsByDate_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetAlbumsByDates();

        // Act
        var result = await _controller.GetAlbumsByDate(default, default, default, default);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task GetAlbumsByDateAmount_AlbumsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetAlbumsByDateAmount(
            ConvertDate(_startDate),
            ConvertDate(_endDate));

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAlbumsByDateAmount_AlbumsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetAlbumsByDateAmount(
            ConvertDate(_startDate),
            ConvertDate(_endDate));
        int value = (int)(result.Result as ObjectResult).Value;

        // Assert
        Assert.That(value, Is.EqualTo(AlbumsAmount));
    }

    [Test]
    public async Task GetAlbumsByDateAmount_AlbumsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowNotFoundExceptionOnGetAlbumsAmount();

        // Act
        var result = await _controller.GetAlbumsByDateAmount(default, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetAlbumsByDateAmount_AlbumsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetAlbumsAmount();

        // Act
        await _controller.GetAlbumsByDateAmount(default, default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetAlbumsByDateAmount_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetAlbumsAmount();

        // Act
        var result = await _controller.GetAlbumsByDateAmount(default, default);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task GetArtistTopAlbums_AlbumsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetArtistTopAlbums(TopAlbumsArtistName);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetArtistTopAlbums_AlbumsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetArtistTopAlbums(TopAlbumsArtistName);
        var value = (result.Result as ObjectResult).Value as IEnumerable<AlbumUI>;

        // Assert
        VerifyAlbumsAreCorrect(value);
    }

    [Test]
    public async Task GetArtistTopAlbums_AlbumsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowNotFoundExceptionOnGetArtistTopAlbums();

        // Act
        var result = await _controller.GetArtistTopAlbums(default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetArtistTopAlbums_AlbumsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetArtistTopAlbums();

        // Act
        await _controller.GetArtistTopAlbums(default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetArtistTopAlbums_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetArtistTopAlbums();

        // Act
        var result = await _controller.GetArtistTopAlbums(default);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public async Task GetAlbumDetails_AlbumsServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetAlbumDetails(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetAlbumDetails_AlbumsServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetAlbumDetails(AlbumDetailsArtistName, AlbumDetailsName);
        var value = (result.Result as ObjectResult).Value as AlbumUI;

        // Assert
        VerifyAlbumDetailsAreCorrect(value);
    }

    [Test]
    public async Task GetAlbumDetails_AlbumsServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowNotFoundExceptionOnGetArtistAlbumDetails();

        // Act
        var result = await _controller.GetAlbumDetails(default, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetAlbumDetails_AlbumsServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetArtistAlbumDetails();

        // Act
        await _controller.GetAlbumDetails(default, default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetAlbumDetails_AlbumsServiceThrowsException_InternalServerErrorCodeReturned()
    {
        // Arrange
        SetUpAlbumsServiceToThrowExceptionOnGetArtistAlbumDetails();

        // Act
        var result = await _controller.GetAlbumDetails(default, default);
        var code = (result.Result as ObjectResult).StatusCode;

        // Assert
        Assert.That(code, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    private static DateTime ConvertDate(DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }

    private static DateOnly ConvertDate(DateTime date)
    {
        return DateOnly.FromDateTime(date);
    }

    private static void VerifyAlbumDetailsAreCorrect(AlbumUI album)
    {
        Assert.That(album.Name, Is.EqualTo(AlbumDetailsName));
        Assert.That(album.ArtistName, Is.EqualTo(AlbumDetailsArtistName));
        Assert.That(album.ImageUrl, Is.EqualTo(AlbumDetailsImageUrl));

        var tracks = album.Tracks.ToList();
        Assert.That(tracks[0].Name, Is.EqualTo(AlbumTrackOneName));
        Assert.That(tracks[0].DurationInSeconds, Is.EqualTo(AlbumTrackOneDurationInSeconds));

        Assert.That(tracks[1].Name, Is.EqualTo(AlbumTrackTwoName));
        Assert.That(tracks[1].DurationInSeconds, Is.EqualTo(AlbumTrackTwoDurationInSeconds));
    }

    private void SetUpData()
    {
        _albums = new List<AlbumBL>
            {
                new()
                {
                    Name = AlbumOneName,
                    PlayCount = AlbumOnePlayCount,
                    ImageUrl = AlbumOneImageUrl,
                    DateCreated = _albumOneDateCreated,
                },
                new()
                {
                    Name = AlbumTwoName,
                    PlayCount = AlbumTwoPlayCount,
                    ImageUrl = AlbumTwoImageUrl,
                    DateCreated = _albumTwoDateCreated,
                },
            };
    }

    private void SetUpAlbumsServiceMockGetGetAlbumsByDates()
    {
        _albumsServiceMock.Setup(a => a.GetAlbumsByDatesAsync(
            _startDate,
            _endDate,
            PageSize,
            Page))
            .ReturnsAsync(_albums);
    }

    private void SetUpAlbumsServiceMockGetGetAlbumsByDateAmount()
    {
        _albumsServiceMock.Setup(a => a.GetAlbumsAmountAsync(_startDate, _endDate))
            .ReturnsAsync(AlbumsAmount);
    }

    private void SetUpAlbumsServiceMockGetArtistTopAlbums()
    {
        _albumsServiceMock.Setup(a => a.GetArtistTopAlbumsAsync(
            TopAlbumsArtistName,
            It.IsAny<int>(),
            It.IsAny<int>()))
            .ReturnsAsync(_albums);
    }

    private void SetUpAlbumsServiceMockGetAlbumDetails()
    {
        var album = new AlbumBL()
        {
            Name = AlbumDetailsName,
            ArtistName = AlbumDetailsArtistName,
            ImageUrl = AlbumDetailsImageUrl,
            Description = AlbumDetailsDescription,
            Tracks = new List<TrackBL>
            {
                new()
                {
                    Name = AlbumTrackOneName,
                    DurationInSeconds = AlbumTrackOneDurationInSeconds,
                },
                new()
                {
                    Name = AlbumTrackTwoName,
                    DurationInSeconds = AlbumTrackTwoDurationInSeconds,
                },
            },
        };

        _albumsServiceMock.Setup(a => a.GetArtistAlbumDetailsAsync(
            AlbumDetailsArtistName,
            AlbumDetailsName))
            .ReturnsAsync(album);
    }

    private void SetUpAlbumsServiceToThrowNotFoundExceptionOnGetAlbumsByDates()
    {
        _albumsServiceMock.Setup(a => a.GetAlbumsByDatesAsync(
            It.IsAny<DateOnly>(),
            It.IsAny<DateOnly>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<NotFoundException>();
    }

    private void SetUpAlbumsServiceToThrowExceptionOnGetAlbumsByDates()
    {
        _albumsServiceMock.Setup(a => a.GetAlbumsByDatesAsync(
            It.IsAny<DateOnly>(),
            It.IsAny<DateOnly>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<Exception>();
    }

    private void SetUpAlbumsServiceToThrowNotFoundExceptionOnGetAlbumsAmount()
    {
        _albumsServiceMock.Setup(a => a.GetAlbumsAmountAsync(
            It.IsAny<DateOnly>(),
            It.IsAny<DateOnly>()))
            .Throws<NotFoundException>();
    }

    private void SetUpAlbumsServiceToThrowExceptionOnGetAlbumsAmount()
    {
        _albumsServiceMock.Setup(a => a.GetAlbumsAmountAsync(
            It.IsAny<DateOnly>(),
            It.IsAny<DateOnly>()))
            .Throws<Exception>();
    }

    private void SetUpAlbumsServiceToThrowNotFoundExceptionOnGetArtistTopAlbums()
    {
        _albumsServiceMock.Setup(a => a.GetArtistTopAlbumsAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<NotFoundException>();
    }

    private void SetUpAlbumsServiceToThrowExceptionOnGetArtistTopAlbums()
    {
        _albumsServiceMock.Setup(a => a.GetArtistTopAlbumsAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<Exception>();
    }

    private void SetUpAlbumsServiceToThrowNotFoundExceptionOnGetArtistAlbumDetails()
    {
        _albumsServiceMock.Setup(a => a.GetArtistAlbumDetailsAsync(
            It.IsAny<string>(),
            It.IsAny<string>()))
            .Throws<NotFoundException>();
    }

    private void SetUpAlbumsServiceToThrowExceptionOnGetArtistAlbumDetails()
    {
        _albumsServiceMock.Setup(a => a.GetArtistAlbumDetailsAsync(
            It.IsAny<string>(),
            It.IsAny<string>()))
            .Throws<Exception>();
    }

    private void VerifyAlbumsAreCorrect(IEnumerable<AlbumUI> model)
    {
        var albums = model.ToList();

        Assert.That(albums[0].Name, Is.EqualTo(AlbumOneName));
        Assert.That(albums[0].PlayCount, Is.EqualTo(AlbumOnePlayCount));
        Assert.That(albums[0].ImageUrl, Is.EqualTo(AlbumOneImageUrl));
        Assert.That(ConvertDate(albums[0].DateCreated), Is.EqualTo(_albumOneDateCreated));

        Assert.That(albums[1].Name, Is.EqualTo(AlbumTwoName));
        Assert.That(albums[1].PlayCount, Is.EqualTo(AlbumTwoPlayCount));
        Assert.That(albums[1].ImageUrl, Is.EqualTo(AlbumTwoImageUrl));
        Assert.That(ConvertDate(albums[1].DateCreated), Is.EqualTo(_albumTwoDateCreated));
    }
}
