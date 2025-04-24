using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MusiciansAPP.BL.Services;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.Domain;
using NUnit.Framework;

namespace MusiciansAPP.BL.UnitTests.Services.Artists;

[TestFixture]
public class ArtistsServiceTests
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

    private const string ArtistNameForMatchedArtists = "Mock Artist Name For Matched Artists";
    private const int MatchedArtists = 1;

    private ArtistsService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Artist> _artists;
    private Artist _artistDetails;
    private Artist _artistWithSimilar;

    [SetUp]
    public void SetUp()
    {
        _webDataProviderMock = new Mock<IWebDataProvider>();
        SetUpWebDataProviderMockGetTopArtists();
        SetUpWebDataProviderMockGetArtistDetails();
        SetUpWebDataProviderMockGetSimilarArtists();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        SetUpUnitOfWorkMockGetTopArtists();
        SetUpUnitOfWorkMockGetArtistDetails();
        SetUpUnitOfWorkMockGetArtistWithSimilar();
        SetUpUnitOfWorkMockGetMatchedArtists();

        _service = new ArtistsService(_webDataProviderMock.Object, _mapper, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetTopArtistsAsync_DBHasFullData_FullDataReturned()
    {
        // Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [Test]
    public async Task GetTopArtistsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        // Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBHasArtistsAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        // Arrange
        _artists = _artists.Take(amount).ToList();

        // Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBHasArtistsAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        // Arrange
        _artists = _artists.Take(amount).ToList();

        // Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBHasArtistsAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        // Arrange
        _artists = _artists.Take(amount).ToList();

        // Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateRangeAsync(It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullArtistData_FullDataReturned(
        string imageUrl)
    {
        // Arrange
        _artists[0].ImageUrl = imageUrl;

        // Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullArtistData_WebServiceWasCalled(
        string imageUrl)
    {
        // Arrange
        _artists[0].ImageUrl = imageUrl;

        // Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullArtistData_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        // Arrange
        _artists[0].ImageUrl = imageUrl;

        // Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateRangeAsync(It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistDetailsAsync_DBHasFullData_FullDataReturned()
    {
        // Act
        var result = await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        VerifyArtistDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistDetailsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        // Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistDetailsAsync(ArtistDetailsName), Times.Never);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveImageUrl_FullDataReturned(
        string imageUrl)
    {
        // Arrange
        _artistDetails.ImageUrl = imageUrl;

        // Act
        var result = await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        VerifyArtistDetailsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        // Arrange
        _artistDetails.ImageUrl = imageUrl;

        // Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistDetailsAsync(ArtistDetailsName), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        // Arrange
        _artistDetails.ImageUrl = imageUrl;

        // Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateAsync(It.IsAny<Artist>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveBiography_FullDataReturned(
        string biography)
    {
        // Arrange
        _artistDetails.Biography = biography;

        // Act
        var result = await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        VerifyArtistDetailsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveBiography_WebServiceWasCalled(
        string biography)
    {
        // Arrange
        _artistDetails.Biography = biography;

        // Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistDetailsAsync(ArtistDetailsName), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveBiography_UnitOfWorkSavingWasCalled(
        string biography)
    {
        // Arrange
        _artistDetails.Biography = biography;

        // Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateAsync(It.IsAny<Artist>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetSimilarArtistsAsync_DBHasFullData_FullDataReturned()
    {
        // Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [Test]
    public async Task GetSimilarArtistsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        // Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetSimilarArtistsAsync_DBHasSimilarArtistsAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        // Arrange
        _artistWithSimilar.SimilarArtists = _artistWithSimilar.SimilarArtists.Take(amount).ToList();

        // Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetSimilarArtistsAsync_DBHasSimilarArtistsAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        // Arrange
        _artistWithSimilar.SimilarArtists = _artistWithSimilar.SimilarArtists.Take(amount).ToList();

        // Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetSimilarArtistsAsync_DBHasSimilarArtistsAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        // Arrange
        _artistWithSimilar.SimilarArtists = _artistWithSimilar.SimilarArtists.Take(amount).ToList();

        // Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateSimilarArtistsAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetSimilarArtistsAsync_DBDoesNotHaveSimilarArtistImageUrl_FullDataReturned(
        string imageUrl)
    {
        // Arrange
        _artistWithSimilar.SimilarArtists[0].ImageUrl = imageUrl;

        // Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetSimilarArtistsAsync_DBDoesNotHaveSimilarArtistImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        // Arrange
        _artistWithSimilar.SimilarArtists[0].ImageUrl = imageUrl;

        // Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        _webDataProviderMock.Verify(
            w => w.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetSimilarArtistsAsync_DBDoesNotHaveSimilarArtistImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        // Arrange
        _artistWithSimilar.SimilarArtists[0].ImageUrl = imageUrl;

        // Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        // Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateSimilarArtistsAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetMatchedArtistsAsync_NameWasNotPassed_FullDataReturned(string name)
    {
        // Act
        var result = await _service.GetMatchedArtistsAsync(name, amount: PageSize, token: default);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [Test]
    public async Task GetMatchedArtistsAsync_NameWasPassed_FullDataReturned()
    {
        // Act
        var result = await _service.GetMatchedArtistsAsync(
            ArtistNameForMatchedArtists, MatchedArtists, token: default);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        VerifyArtistAreCorrect(result);
    }

    private static void VerifyTopArtistsAreCorrect(IEnumerable<ArtistBL> model)
    {
        var artists = model.ToList();
        Assert.That(artists[0].Name, Is.EqualTo(ArtistOneName));
        Assert.That(artists[0].ImageUrl, Is.EqualTo(ArtistOneImageUrl));

        Assert.That(artists[1].Name, Is.EqualTo(ArtistTwoName));
        Assert.That(artists[1].ImageUrl, Is.EqualTo(ArtistTwoImageUrl));
    }

    private static void VerifyArtistAreCorrect(IEnumerable<ArtistBL> model)
    {
        var artists = model.ToList();
        Assert.That(artists[0].Name, Is.EqualTo(ArtistOneName));
        Assert.That(artists[0].ImageUrl, Is.EqualTo(ArtistOneImageUrl));
    }

    private static void VerifyArtistDetailsAreCorrect(ArtistBL artist)
    {
        Assert.That(artist.Name, Is.EqualTo(ArtistDetailsName));
        Assert.That(artist.ImageUrl, Is.EqualTo(ArtistDetailsImageUrl));
        Assert.That(artist.Biography, Is.EqualTo(ArtistDetailsBiography));
    }

    private void SetUpWebDataProviderMockGetTopArtists()
    {
        var artists = new List<ArtistDAL>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl },
        };
        _webDataProviderMock.Setup(wdp => wdp.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(artists);
    }

    private void SetUpWebDataProviderMockGetArtistDetails()
    {
        var artistDetails = new ArtistDAL
        {
            Name = ArtistDetailsName,
            ImageUrl = ArtistDetailsImageUrl,
            Biography = ArtistDetailsBiography,
        };
        _webDataProviderMock
            .Setup(wdp => wdp.GetArtistDetailsAsync(ArtistDetailsName))
            .ReturnsAsync(artistDetails);
    }

    private void SetUpWebDataProviderMockGetSimilarArtists()
    {
        var similarArtists = new SimilarArtistsDAL
        {
            ArtistName = ArtistNameForSimilar,
            Artists = new List<ArtistDAL>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl },
            },
        };
        _webDataProviderMock.Setup(wdp => wdp.GetSimilarArtistsAsync(
            ArtistNameForSimilar, PageSize, Page)).ReturnsAsync(similarArtists);
    }

    private void SetUpUnitOfWorkMockGetTopArtists()
    {
        _artists = new List<Artist>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl },
        };
        _unitOfWorkMock.Setup(uow => uow.Artists.GetTopArtistsAsync(PageSize, It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => _artists);
    }

    private void SetUpUnitOfWorkMockGetArtistDetails()
    {
        _artistDetails = new Artist
        {
            Name = ArtistDetailsName,
            ImageUrl = ArtistDetailsImageUrl,
            Biography = ArtistDetailsBiography,
        };
        _unitOfWorkMock.Setup(uow =>
                uow.Artists.GetArtistDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(() => _artistDetails);
    }

    private void SetUpUnitOfWorkMockGetArtistWithSimilar()
    {
        _artistWithSimilar = new Artist
        {
            Name = ArtistNameForSimilar,
            SimilarArtists = new List<Artist>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl },
            },
        };
        _unitOfWorkMock.Setup(uow => uow.Artists.GetArtistWithSimilarAsync(
                ArtistNameForSimilar, PageSize, Page))
            .ReturnsAsync(() => _artistWithSimilar);
    }

    private void SetUpUnitOfWorkMockGetMatchedArtists()
    {
        _unitOfWorkMock.Setup(uow => uow.Artists.GetMatchedArtistsAsync(
            ArtistNameForMatchedArtists, MatchedArtists, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => _artists.Take(1));
    }
}