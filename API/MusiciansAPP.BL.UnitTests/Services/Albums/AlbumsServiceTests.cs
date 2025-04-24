using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MusiciansAPP.BL.Services;
using MusiciansAPP.BL.Services.Albums;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.Domain;
using NUnit.Framework;

namespace MusiciansAPP.BL.UnitTests.Services.Albums;

[TestFixture]
public class AlbumsServiceTests
{
    private const int PageSize = 2;
    private const int Page = 1;

    private const string AlbumsArtistName = "Albums Mock Artist Name";
    private const string AlbumOneName = "Album One Mock Name";
    private const int AlbumOnePlayCount = 1;
    private const string AlbumOneImageUrl = "Album One Mock ImageUrl";
    private const string AlbumTwoName = "Album Two Mock Name";
    private const int AlbumTwoPlayCount = 2;
    private const string AlbumTwoImageUrl = "Album Two Mock ImageUrl";

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

    private AlbumsService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Album> _albums;
    private Album _albumDetails;

    [SetUp]
    public void SetUp()
    {
        _webDataProviderMock = new Mock<IWebDataProvider>();
        SetUpWebDataProviderMockGetArtistTopAlbums();
        SetUpWebDataProviderMockGetArtistAlbumDetails();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        SetUpUnitOfWorkMockGetArtistDetails();
        SetUpUnitOfWorkMockGetTopAlbumsForArtist();
        SetUpUnitOfWorkMockGetAlbumDetails();
        SetUpUnitOfWorkMockGetAlbumsByDates();
        SetUpUnitOfWorkMockGetAlbumsByDateAmount();

        _service = new AlbumsService(_webDataProviderMock.Object, _mapper, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBHasFullData_FullDataReturned()
    {
        // Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopAlbumsAsync_DBHasAlbumsAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        // Arrange
        _albums = _albums.Take(amount).ToList();

        // Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopAlbumsAsync_DBHasAlbumsAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        // Arrange
        _albums = _albums.Take(amount).ToList();

        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopAlbumsAsync_DBHasAlbumsAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        // Arrange
        _albums = _albums.Take(amount).ToList();

        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Albums.AddOrUpdateArtistAlbumsAsync(
                It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Album>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_FullDataReturned(
        string imageUrl)
    {
        // Arrange
        _albums[0].ImageUrl = imageUrl;

        // Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        // Arrange
        _albums[0].ImageUrl = imageUrl;

        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        // Arrange
        _albums[0].ImageUrl = imageUrl;

        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Albums.AddOrUpdateArtistAlbumsAsync(
                It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Album>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumPlayCount_FullDataReturned()
    {
        // Arrange
        _albums[0].PlayCount = null;

        // Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_WebServiceWasCalled()
    {
        // Arrange
        _albums[0].PlayCount = null;

        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Once);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_UnitOfWorkSavingWasCalled()
    {
        // Arrange
        _albums[0].ImageUrl = null;

        // Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Albums.AddOrUpdateArtistAlbumsAsync(
                It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Album>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBHasFullData_FullDataReturned()
    {
        // Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Never);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumImageUrl_FullDataReturned(
        string imageUrl)
    {
        // Arrange
        _albumDetails.ImageUrl = imageUrl;

        // Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        // Arrange
        _albumDetails.ImageUrl = imageUrl;

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        // Arrange
        _albumDetails.ImageUrl = imageUrl;

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumDescription_FullDataReturned()
    {
        // Arrange
        _albumDetails.Description = null;

        // Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumDescription_WebServiceWasCalled()
    {
        // Arrange
        _albumDetails.Description = null;

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumDescription_UnitOfWorkSavingWasCalled()
    {
        // Arrange
        _albumDetails.Description = null;

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveTrackDurationInSeconds_FullDataReturned()
    {
        // Arrange
        _albumDetails.Tracks[0].DurationInSeconds = null;

        // Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveTrackDurationInSeconds_WebServiceWasCalled()
    {
        // Arrange
        _albumDetails.Tracks[0].DurationInSeconds = null;

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveTrackDurationInSeconds_UnitOfWorkSavingWasCalled()
    {
        // Arrange
        _albumDetails.Tracks[0].DurationInSeconds = null;

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumTracks_FullDataReturned()
    {
        // Arrange
        _albumDetails.Tracks = new List<Track>();

        // Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumTracks_WebServiceWasCalled()
    {
        // Arrange
        _albumDetails.Tracks = new List<Track>();

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumTracks_UnitOfWorkSavingWasCalled()
    {
        // Arrange
        _albumDetails.Tracks = new List<Track>();

        // Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
    }

    [Test]
    public async Task GetAlbumsAmountAsync_DBHasAlbums_AlbumsAmountReturned()
    {
        // Act
        int result = await _service.GetAlbumsAmountAsync(_startDate, _endDate);

        // Assert
        Assert.That(result, Is.EqualTo(AlbumsAmount));
    }

    [Test]
    public async Task GetAlbumsAmountAsync_DBDoesNotHaveTracksAmount_ZeroReturned()
    {
        // Assert
        var newStartDate = _endDate.AddDays(1);

        // Act
        int result = await _service.GetAlbumsAmountAsync(newStartDate, _endDate);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public async Task GetAlbumsByDatesAsync_DBHasAlbums_AlbumsAmountReturned()
    {
        // Act
        var result = await _service.GetAlbumsByDatesAsync(_startDate, _endDate, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [Test]
    public async Task GetAlbumsByDatesAsync_DBDoesNotHaveAlbums_EmptyCollectionReturned()
    {
        // Assert
        var newStartDate = _endDate.AddDays(1);

        // Act
        var result = await _service.GetAlbumsByDatesAsync(newStartDate, _endDate, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(0));
    }

    private static void VerifyAlbumDetailsAreCorrect(AlbumBL album)
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

    private void VerifyTopAlbumsAreCorrect(IEnumerable<AlbumBL> model)
    {
        var albums = model.ToList();

        Assert.That(albums[0].Name, Is.EqualTo(AlbumOneName));
        Assert.That(albums[0].PlayCount, Is.EqualTo(AlbumOnePlayCount));
        Assert.That(albums[0].ImageUrl, Is.EqualTo(AlbumOneImageUrl));
        Assert.That(albums[0].DateCreated, Is.EqualTo(_albumOneDateCreated));

        Assert.That(albums[1].Name, Is.EqualTo(AlbumTwoName));
        Assert.That(albums[1].PlayCount, Is.EqualTo(AlbumTwoPlayCount));
        Assert.That(albums[1].ImageUrl, Is.EqualTo(AlbumTwoImageUrl));
        Assert.That(albums[1].DateCreated, Is.EqualTo(_albumTwoDateCreated));
    }

    private void SetUpWebDataProviderMockGetArtistTopAlbums()
    {
        var artistAlbums = new ArtistAlbumsDAL
        {
            ArtistName = AlbumsArtistName,
            Albums = new List<AlbumDAL>
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
            },
        };
        _webDataProviderMock.Setup(wdp => wdp.GetArtistTopAlbumsAsync(
            AlbumsArtistName, PageSize, Page)).ReturnsAsync(artistAlbums);
    }

    private void SetUpWebDataProviderMockGetArtistAlbumDetails()
    {
        var albumDetails = new AlbumDAL()
        {
            Name = AlbumDetailsName,
            ArtistName = AlbumDetailsArtistName,
            ImageUrl = AlbumDetailsImageUrl,
            Description = AlbumDetailsDescription,
            Tracks = new List<TrackDAL>
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
        _webDataProviderMock.Setup(wdp => wdp.GetArtistAlbumDetailsAsync(
            AlbumDetailsArtistName, AlbumDetailsName)).ReturnsAsync(albumDetails);
    }

    private void SetUpUnitOfWorkMockGetArtistDetails()
    {
        _unitOfWorkMock.Setup(uow =>
                uow.Artists.GetArtistDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(new Artist());
    }

    private void SetUpUnitOfWorkMockGetTopAlbumsForArtist()
    {
        _albums = new List<Album>
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
        _unitOfWorkMock.Setup(uow => uow.Albums.GetTopAlbumsForArtistAsync(
            AlbumsArtistName, PageSize, Page)).ReturnsAsync(() => _albums);
    }

    private void SetUpUnitOfWorkMockGetAlbumDetails()
    {
        _albumDetails = new Album
        {
            Name = AlbumDetailsName,
            Artist = new Artist { Name = AlbumDetailsArtistName },
            ImageUrl = AlbumDetailsImageUrl,
            Description = AlbumDetailsDescription,
            Tracks = new List<Track>
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
        _unitOfWorkMock.Setup(uow => uow.Albums.GetAlbumDetailsAsync(
                AlbumDetailsArtistName, AlbumDetailsName))
            .ReturnsAsync(() => _albumDetails);
    }

    private void SetUpUnitOfWorkMockGetAlbumsByDates()
    {
        _albums = new List<Album>
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

        _unitOfWorkMock.Setup(uow => uow.Albums.GetAlbumsByDateAsync(
     _startDate, _endDate, PageSize, Page)).ReturnsAsync(() => _albums);
    }

    private void SetUpUnitOfWorkMockGetAlbumsByDateAmount()
    {
        _unitOfWorkMock.Setup(uow => uow.Albums.GetAlbumsByDateAmountAsync(
    _startDate, _endDate)).ReturnsAsync(AlbumsAmount);
    }
}