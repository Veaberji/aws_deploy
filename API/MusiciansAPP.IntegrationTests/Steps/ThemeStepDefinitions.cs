using FluentAssertions;
using MusiciansAPP.IntegrationTests.Drivers;
using MusiciansAPP.IntegrationTests.Pages;
using TechTalk.SpecFlow;

namespace MusiciansAPP.IntegrationTests.Steps;

[Binding]
public class ThemeStepDefinitions
{
    private readonly HomePage _homePage;
    private bool _isDarkTheme;

    public ThemeStepDefinitions(BrowserDriver driver)
    {
        _homePage = new HomePage(driver.Current);
    }

    [When(@"the user clicks on the Settings button")]
    public void WhenTheUserClicksOnTheSettingsButton()
    {
        _homePage.ClickSettingsButton();
    }

    [Then(@"the Change Theme button should be shown")]
    public void ThenTheChangeThemeButtonShouldBeShown()
    {
        var result = _homePage.DoesChangeThemeButtonExist();

        result.Should().BeTrue();
    }

    [When(@"the user clicks on the Change Theme button")]
    public void WhenTheUserClicksOnTheChangeThemeButton()
    {
        _isDarkTheme = _homePage.IsDarkTheme();
        _homePage.ChangeTheme();
    }

    [Then(@"the Theme should change")]
    public void ThenTheThemeShouldChange()
    {
        var result = _homePage.IsDarkTheme();

        result.Should().NotBe(_isDarkTheme);
    }
}
