using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MusiciansAPP.BL.Services;
using MusiciansAPP.BL.Services.Tracks;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.Domain;
using NUnit.Framework;

namespace MusiciansAPP.BL.UnitTests.Services.Tracks;

[TestFixture]
public class TracksServiceTests
{
    private const int PageSize = 2;
    private const int Page = 1;

    private const string TracksArtistName = "Tracks Mock Artist Name";
    private const string TrackOneName = "Track One Mock Name";
    private const int TrackOnePlayCount = 1;
    private const string TrackTwoName = "Track Two Mock Name";
    private const int TrackTwoPlayCount = 2;

    private const int TracksAmount = 2;

    private TracksService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Track> _tracks;

    [SetUp]
    public void SetUp()
    {
        _webDataProviderMock = new Mock<IWebDataProvider>();
        SetUpWebDataProviderMockGetArtistTopTracks();
        SetUpWebDataProviderMockGetTracksAmount();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        SetUpUnitOfWorkMockGetArtistTopTracks();
        SetUpUnitOfWorkMockGetArtistDetails();

        _service = new TracksService(_webDataProviderMock.Object, _mapper, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBHasFullData_FullDataReturned()
    {
        // Act
        var result = await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopTracksAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        // Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopTracksAsync_DBHasTracksAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        // Arrange
        _tracks = _tracks.Take(amount).ToList();

        // Act
        var result = await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopTracksAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopTracksAsync_DBHasTracksAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        // Arrange
        _tracks = _tracks.Take(amount).ToList();

        // Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopTracksAsync_DBHasTracksAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        // Arrange
        _tracks = _tracks.Take(amount).ToList();

        // Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Tracks.AddOrUpdateArtistTracksAsync(
                It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Track>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBDoesNotHaveFullTrackData_FullDataReturned()
    {
        // Arrange
        _tracks[0].PlayCount = null;

        // Act
        var result = await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopTracksAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBDoesNotHaveFullTrackData_WebServiceWasCalled()
    {
        // Arrange
        _tracks[0].PlayCount = null;

        // Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page), Times.Once);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBDoesNotHaveFullTrackData_UnitOfWorkSavingWasCalled()
    {
        // Arrange
        _tracks[0].PlayCount = null;

        // Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Tracks.AddOrUpdateArtistTracksAsync(
                It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Track>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetTracksAmountAsync_WebDataProviderHasTracksAmount_TracksAmountReturned()
    {
        // Act
        int result = await _service.GetTracksAmountAsync(TracksArtistName);

        // Assert
        Assert.That(result, Is.EqualTo(TracksAmount));
    }

    [Test]
    public async Task GetTracksAmountAsync_WebDataProviderDoesNotHaveTracksAmount_ZeroReturned()
    {
        // Act
        int result = await _service.GetTracksAmountAsync(string.Empty);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    private static void VerifyTopTracksAreCorrect(IEnumerable<TrackBL> model)
    {
        var tracks = model.ToList();

        Assert.That(tracks[0].Name, Is.EqualTo(TrackOneName));
        Assert.That(tracks[0].PlayCount, Is.EqualTo(TrackOnePlayCount));

        Assert.That(tracks[1].Name, Is.EqualTo(TrackTwoName));
        Assert.That(tracks[1].PlayCount, Is.EqualTo(TrackTwoPlayCount));
    }

    private void SetUpWebDataProviderMockGetArtistTopTracks()
    {
        var artistTracks = new ArtistTracksDAL
        {
            ArtistName = TracksArtistName,
            Tracks = new List<TrackDAL>
            {
                new() { Name = TrackOneName, PlayCount = TrackOnePlayCount },
                new() { Name = TrackTwoName, PlayCount = TrackTwoPlayCount },
            },
        };
        _webDataProviderMock.Setup(wdp => wdp.GetArtistTopTracksAsync(
            TracksArtistName, PageSize, Page)).ReturnsAsync(artistTracks);
    }

    private void SetUpWebDataProviderMockGetTracksAmount()
    {
        _webDataProviderMock.Setup(wdp => wdp.GetTracksAmountAsync(
            TracksArtistName)).ReturnsAsync(TracksAmount);
    }

    private void SetUpUnitOfWorkMockGetArtistTopTracks()
    {
        _tracks = new List<Track>
        {
            new() { Name = TrackOneName, PlayCount = TrackOnePlayCount },
            new() { Name = TrackTwoName, PlayCount = TrackTwoPlayCount },
        };
        _unitOfWorkMock.Setup(uow => uow.Tracks.GetTopTracksForArtistAsync(
            TracksArtistName, PageSize, Page)).ReturnsAsync(() => _tracks);
    }

    private void SetUpUnitOfWorkMockGetArtistDetails()
    {
        _unitOfWorkMock.Setup(uow =>
            uow.Artists.GetArtistDetailsAsync(It.IsAny<string>())).ReturnsAsync(new Artist());
    }
}