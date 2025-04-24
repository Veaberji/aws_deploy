using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusiciansAPP.API.Controllers;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.BL.Services.CSV;
using NUnit.Framework;

namespace MusiciansAPP.API.UnitTests.Controllers;

[TestFixture]
public class FileControllerTests
{
    private const string TestName = "TestName";
    private const int TestAmount = 2;

    private const string ArtistOneName = "Artist One Mock Name";
    private const string ArtistOneImageUrl = "Artist One Mock ImageUrl";
    private const string ArtistOneBiography = "Artist One Mock Biography";
    private const string ArtistTwoName = "Artist Two Mock Name";
    private const string ArtistTwoImageUrl = "Artist Two Mock ImageUrl";
    private const string ArtistTwoBiography = "Artist Two Mock Biography";

    private ArtistsReportRequest _request;

    private FileController _controller;
    private Mock<IErrorHandler> _errorHandlerMock;
    private Mock<ICSVService<ArtistBL>> _csvServiceMock;
    private Mock<IArtistsService> _artistsServiceMock;

    [SetUp]
    public void Setup()
    {
        _errorHandlerMock = new Mock<IErrorHandler>();
        _csvServiceMock = new Mock<ICSVService<ArtistBL>>();
        _artistsServiceMock = new Mock<IArtistsService>();

        _controller = new FileController(
            _errorHandlerMock.Object,
            _csvServiceMock.Object,
            _artistsServiceMock.Object);

        _request = new() { Name = TestName, Amount = TestAmount };
    }

    [Test]
    public void GetArtistsReport_RequestWasCanceled_BadRequestResponseReturned()
    {
        // Arrange
        _artistsServiceMock.Setup(s => s.GetMatchedArtistsAsync(
            It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws<OperationCanceledException>();

        // Act
        var result = _controller.GetArtistsReport(_request, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public void GetArtistsReport_RequestWasNotCanceled_FileResponseReturned()
    {
        // Act
        var result = _controller.GetArtistsReport(_request, default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<FileContentResult>());
    }

    [Test]
    public void GetArtistsReport_RequestWasNotCanceled_CorrectDataReturned()
    {
        // Arrange
        var artists = new List<ArtistBL>()
        {
            new()
            {
                Name = ArtistOneName,
                ImageUrl = ArtistOneImageUrl,
                Biography = ArtistOneBiography,
            },
            new()
            {
                Name = ArtistTwoName,
                ImageUrl = ArtistTwoImageUrl,
                Biography = ArtistTwoBiography,
            },
        };
        byte[] expectedFile =
{
            78, 97, 109, 101, 32, 124, 32, 73, 109, 97, 103, 101, 85, 114, 108, 32,
            124, 32, 66, 105, 111, 103, 114, 97, 112, 104, 121, 13, 10, 34, 65, 114, 116, 105, 115,
            116, 32, 79, 110, 101, 32, 77, 111, 99, 107, 32, 78, 97, 109, 101, 34, 32, 124, 32, 65,
            114, 116, 105, 115, 116, 32, 79, 110, 101, 32, 77, 111, 99, 107, 32, 73, 109, 97, 103,
            101, 85, 114, 108, 32, 124, 32, 65, 114, 116, 105, 115, 116, 32, 79, 110, 101, 32, 77,
            111, 99, 107, 32, 66, 105, 111, 103, 114, 97, 112, 104, 121, 13, 10, 34, 65, 114, 116,
            105, 115, 116, 32, 84, 119, 111, 32, 77, 111, 99, 107, 32, 78, 97, 109, 101, 34, 32, 124,
            32, 65, 114, 116, 105, 115, 116, 32, 84, 119, 111, 32, 77, 111, 99, 107, 32, 73, 109, 97,
            103, 101, 85, 114, 108, 32, 124, 32, 65, 114, 116, 105, 115, 116, 32, 84, 119, 111, 32, 77,
            111, 99, 107, 32, 66, 105, 111, 103, 114, 97, 112, 104, 121,
};

        _artistsServiceMock.Setup(a => a.GetMatchedArtistsAsync(
            _request.Name, _request.Amount, default)).ReturnsAsync(artists);
        _csvServiceMock.Setup(a => a.GetReport(It.IsAny<IEnumerable<ArtistBL>>())).Returns(expectedFile);
        _csvServiceMock.Setup(a => a.GetReportFileName(_request.Name, _request.Amount)).Returns(TestName);

        // Act
        var result = _controller.GetArtistsReport(_request, default);
        var value = result.Result as FileContentResult;

        // Assert
        Assert.That(value.ContentType, Is.EqualTo("text/csv"));
        Assert.That(value.FileDownloadName, Is.EqualTo(TestName));
        Assert.That(value.FileContents, Is.EqualTo(expectedFile));
    }
}
