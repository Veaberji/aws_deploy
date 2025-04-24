using System.Collections.Generic;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.BL.Services.CSV;
using NUnit.Framework;

namespace MusiciansAPP.BL.UnitTests.Services.CSV;

[TestFixture]
public class ArtistsCSVServiceTests
{
    private const string ArtistOneName = "Artist One Mock Name";
    private const string ArtistOneImageUrl = "Artist One Mock ImageUrl";
    private const string ArtistOneBiography = "Artist One Mock Biography";
    private const string ArtistTwoName = "Artist Two Mock Name";
    private const string ArtistTwoImageUrl = "Artist Two Mock ImageUrl";
    private const string ArtistTwoBiography = "Artist Two Mock Biography";

    private ArtistsCSVService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new ArtistsCSVService();
    }

    [Test]
    public void GetReportFileName_ArtistsPassed_ReportReturned()
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
        byte[] expected =
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

        // Act
        var result = _service.GetReport(artists);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(null, 1, "artists_1-line_report.csv")]
    [TestCase("", 1, "artists_1-line_report.csv")]
    [TestCase(" ", 1, "artists_1-line_report.csv")]
    [TestCase("testName", 1, "testName_1-line_report.csv")]
    [TestCase("testName", 2, "testName_2-lines_report.csv")]
    public void GetReportFileName_NameAndAmountPassed_FileNameReturned(
        string name, int amount, string expected)
    {
        // Act
        var result = _service.GetReportFileName(name, amount);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
