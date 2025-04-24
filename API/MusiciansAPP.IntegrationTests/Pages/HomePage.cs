using MusiciansAPP.IntegrationTests.Support;
using OpenQA.Selenium;

namespace MusiciansAPP.IntegrationTests.Pages;

public class HomePage : BasePage
{
    public HomePage(IWebDriver webDriver)
        : base(webDriver)
    {
    }

    private IWebElement ArtistsReportButton => WebDriver.FindElementById("artists-report");

    private IWebElement FirstArtistCard =>
        WebDriver
        .WaitForElementByXPath("//mat-grid-list[@id='topArtistsList']/div/mat-grid-tile[1]");

    private IWebElement SettingsButton => WebDriver.FindElementById("settings");

    private IWebElement ChangeThemeButton => WebDriver.FindElementById("changeTheme");

    private IWebElement BodyTag => WebDriver.GetElementByName("body");

    private IWebElement ServiceUnavailableError =>
        WebDriver
        .WaitForElementByXPath("//div[@id='toast-container']/div[@toast-component]/div[contains(text(),'Unavailable')]");

    public void OpenHomePage() => WebDriver.Navigate().GoToUrl(BaseUrl);

    public void ClickArtistsReportButton() => ArtistsReportButton.Click();

    public void ClickFirstArtistCard() => FirstArtistCard.Click();

    public void ClickSettingsButton() => SettingsButton.Click();

    public bool DoesChangeThemeButtonExist() => ChangeThemeButton is not null;

    public bool DoesServiceUnavailableErrorExist() => ServiceUnavailableError is not null;

    public void ChangeTheme()
    {
        ClickSettingsButton();
        ChangeThemeButton.Click();
    }

    public bool IsDarkTheme()
    {
        const string darkThemeClass = "dark-theme";
        var cssClass = WebDriver.WaitForElementClassToHaveValue(BodyTag, "theme");

        return cssClass.Contains(darkThemeClass, System.StringComparison.InvariantCultureIgnoreCase);
    }
}