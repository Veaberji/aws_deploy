using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusiciansAPP.API.Controllers;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Tracks;
using MusiciansAPP.DAL.Exceptions;
using NUnit.Framework;

namespace MusiciansAPP.API.UnitTests.Controllers;

[TestFixture]
public class TracksControllerTests
{
    private const int PageSize = 2;
    private const int Page = 1;

    private const string TracksArtistName = "Tracks Mock Artist Name";
    private const string TrackOneName = "Track One Mock Name";
    private const int TrackOnePlayCount = 1;
    private const string TrackTwoName = "Track Two Mock Name";
    private const int TrackTwoPlayCount = 2;

    private const int TracksAmount = 2;

    private TracksController _controller;
    private Mock<IErrorHandler> _errorHandlerMock;
    private Mock<ITracksService> _tracksServiceMock;
    private Mock<IPagingHelper> _pagingHelperMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _errorHandlerMock = new Mock<IErrorHandler>();

        _pagingHelperMock = new Mock<IPagingHelper>();
        _pagingHelperMock.Setup(p => p.GetCorrectPageSize(PageSize)).Returns(PageSize);

        _tracksServiceMock = new Mock<ITracksService>();
        SetUpTracksServiceMockGetArtistTopTracks();
        SetUpTracksServiceMockGetTracksAmount();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.API")));

        _controller = new TracksController(
            _errorHandlerMock.Object,
            _tracksServiceMock.Object,
            _mapper,
            _pagingHelperMock.Object);
    }

    [Test]
    public async Task GetArtistTopTracks_TracksServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetArtistTopTracks(TracksArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetArtistTopTracks_TracksServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetArtistTopTracks(TracksArtistName, PageSize, Page);
        var value = (result.Result as ObjectResult).Value as IEnumerable<TrackUI>;

        // Assert
        VerifyTopTracksAreCorrect(value);
    }

    [Test]
    public async Task GetArtistTopTracks_TracksServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowNotFoundExceptionOnGetArtistTopTracks();

        // Act
        var result = await _controller.GetArtistTopTracks(default, default, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetArtistTopTracks_TracksServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetArtistTopTracks();

        // Act
        await _controller.GetArtistTopTracks(default, default, default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetArtistTopTracksAmount_TracksServiceReturnsData_OkResponseReturned()
    {
        // Act
        var result = await _controller.GetArtistTopTracksAmount(TracksArtistName);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetArtistTopTracksAmount_TracksServiceReturnsData_CorrectDataReturned()
    {
        // Act
        var result = await _controller.GetArtistTopTracksAmount(TracksArtistName);
        var value = (int)(result.Result as ObjectResult).Value;

        // Assert
        Assert.That(value, Is.EqualTo(TracksAmount));
    }

    [Test]
    public async Task GetArtistTopTracksAmount_TracksServiceThrowsArgumentException_NotFoundResponseReturned()
    {
        // Arrange
        SetUpArtistsServiceToThrowNotFoundExceptionOnGetTracksAmount();

        // Act
        var result = await _controller.GetArtistTopTracksAmount(default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetArtistTopTracksAmount_TracksServiceThrowsException_ErrorHandlerCalled()
    {
        // Arrange
        SetUpArtistsServiceToThrowExceptionOnGetTracksAmount();

        // Act
        await _controller.GetArtistTopTracksAmount(default);

        // Assert
        _errorHandlerMock.Verify(
           e => e.HandleError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
    }

    private static void VerifyTopTracksAreCorrect(IEnumerable<TrackUI> model)
    {
        var tracks = model.ToList();

        Assert.That(tracks[0].Name, Is.EqualTo(TrackOneName));
        Assert.That(tracks[0].PlayCount, Is.EqualTo(TrackOnePlayCount));

        Assert.That(tracks[1].Name, Is.EqualTo(TrackTwoName));
        Assert.That(tracks[1].PlayCount, Is.EqualTo(TrackTwoPlayCount));
    }

    private void SetUpTracksServiceMockGetArtistTopTracks()
    {
        var artists = new List<TrackBL>
        {
                new() { Name = TrackOneName, PlayCount = TrackOnePlayCount },
                new() { Name = TrackTwoName, PlayCount = TrackTwoPlayCount },
        };
        _tracksServiceMock.Setup(a => a.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page))
            .ReturnsAsync(artists);
    }

    private void SetUpTracksServiceMockGetTracksAmount()
    {
        _tracksServiceMock.Setup(wdp => wdp.GetTracksAmountAsync(TracksArtistName))
            .ReturnsAsync(TracksAmount);
    }

    private void SetUpArtistsServiceToThrowNotFoundExceptionOnGetArtistTopTracks()
    {
        _tracksServiceMock.Setup(a => a.GetArtistTopTracksAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<NotFoundException>();
    }

    private void SetUpArtistsServiceToThrowExceptionOnGetArtistTopTracks()
    {
        _tracksServiceMock.Setup(a => a.GetArtistTopTracksAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .Throws<Exception>();
    }

    private void SetUpArtistsServiceToThrowNotFoundExceptionOnGetTracksAmount()
    {
        _tracksServiceMock.Setup(a => a.GetTracksAmountAsync(It.IsAny<string>()))
            .Throws<NotFoundException>();
    }

    private void SetUpArtistsServiceToThrowExceptionOnGetTracksAmount()
    {
        _tracksServiceMock.Setup(a => a.GetTracksAmountAsync(It.IsAny<string>()))
            .Throws<Exception>();
    }
}
