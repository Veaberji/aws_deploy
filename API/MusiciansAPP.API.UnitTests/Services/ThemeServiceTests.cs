using System;
using Moq;
using MusiciansAPP.API.Services;
using NUnit.Framework;

namespace MusiciansAPP.API.UnitTests.Services;

[TestFixture]
public class ThemeServiceTests
{
    private ThemeService _service;
    private Mock<IThemeDataProvider> _themeProviderMock;

    [SetUp]
    public void Setup()
    {
        _themeProviderMock = new Mock<IThemeDataProvider>();
        _service = new ThemeService(_themeProviderMock.Object);
    }

    [TestCase("20:00:01", "20:00:00", "08:00:00", true)]
    [TestCase("07:59:59", "20:00:00", "08:00:00", true)]
    [TestCase("20:00:00", "20:00:00", "08:00:00", true)]
    [TestCase("00:00:00", "20:00:00", "08:00:00", true)]
    [TestCase("19:59:59", "20:00:00", "08:00:00", false)]
    [TestCase("08:00:00", "20:00:00", "08:00:00", false)]
    [TestCase("08:00:01", "20:00:00", "08:00:00", false)]
    [TestCase("20:00:01", "20:00:00", "22:00:00", true)]
    [TestCase("21:59:59", "20:00:00", "22:00:00", true)]
    [TestCase("20:00:00", "20:00:00", "22:00:00", true)]
    [TestCase("00:00:01", "20:00:00", "22:00:00", false)]
    [TestCase("19:59:59", "20:00:00", "22:00:00", false)]
    [TestCase("22:00:00", "20:00:00", "22:00:00", false)]
    [TestCase("22:00:01", "20:00:00", "22:00:00", false)]
    [TestCase("01:00:01", "01:00:00", "09:00:00", true)]
    [TestCase("08:59:59", "01:00:00", "09:00:00", true)]
    [TestCase("01:00:00", "01:00:00", "09:00:00", true)]
    [TestCase("00:00:01", "01:00:00", "09:00:00", false)]
    [TestCase("00:59:59", "01:00:00", "09:00:00", false)]
    [TestCase("09:00:00", "01:00:00", "09:00:00", false)]
    [TestCase("09:00:01", "01:00:00", "09:00:00", false)]
    public void IsDarkTheme_DatePassed_ReturnedBoolResult(
        DateTime date,
        DateTime darkFromTime,
        DateTime darkToTime,
        bool expected)
    {
        // Arrange
        _themeProviderMock.Setup(t => t.DarkFromTime).Returns(darkFromTime);
        _themeProviderMock.Setup(t => t.DarkToTime).Returns(darkToTime);

        // Act
        var result = _service.IsDarkTheme(date);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
