using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using MusiciansAPP.API.Controllers;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using NUnit.Framework;

namespace MusiciansAPP.API.UnitTests.Controllers;

[TestFixture]
public class SettingsControllerTests
{
    private const string NotProductionEnvironment = "NotProductionMock";
    private const string ProductionEnvironment = "Production";

    private SettingsController _controller;
    private Mock<IErrorHandler> _errorHandlerMock;
    private Mock<IHostEnvironment> _testEnvironmentMock;
    private Mock<IThemeService> _themeServiceMock;

    [SetUp]
    public void Setup()
    {
        _errorHandlerMock = new Mock<IErrorHandler>();
        _testEnvironmentMock = new Mock<IHostEnvironment>();
        _themeServiceMock = new Mock<IThemeService>();

        _controller = new SettingsController(
            _errorHandlerMock.Object,
            _testEnvironmentMock.Object,
            _themeServiceMock.Object);
    }

    [Test]
    public void IsDarkTheme_EnvironmentHasNotSetToProduction_NotFoundResponseReturned()
    {
        // Arrange
        _testEnvironmentMock.SetupGet(x => x.EnvironmentName).Returns(NotProductionEnvironment);

        // Act
        var result = _controller.IsDarkTheme(default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public void IsDarkTheme_EnvironmentSetToProduction_OkResponseReturned()
    {
        // Arrange
        _testEnvironmentMock.SetupGet(x => x.EnvironmentName).Returns(ProductionEnvironment);

        // Act
        var result = _controller.IsDarkTheme(default);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [TestCase(true)]
    [TestCase(false)]
    public void IsDarkTheme_EnvironmentSetToProduction_CorrectDataReturned(bool isDarkTheme)
    {
        // Arrange
        _testEnvironmentMock.SetupGet(x => x.EnvironmentName).Returns(ProductionEnvironment);
        _themeServiceMock.Setup(t => t.IsDarkTheme(It.IsAny<DateTime>())).Returns(isDarkTheme);

        // Act
        var result = _controller.IsDarkTheme(default);
        var value = (result.Result as ObjectResult).Value as ThemeSettings;

        // Assert
        Assert.That(value.IsDarkTheme, Is.EqualTo(isDarkTheme));
    }
}
