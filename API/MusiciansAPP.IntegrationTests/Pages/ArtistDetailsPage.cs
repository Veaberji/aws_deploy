using System;
using MusiciansAPP.IntegrationTests.Support;
using OpenQA.Selenium;

namespace MusiciansAPP.IntegrationTests.Pages;

public class ArtistDetailsPage : BasePage
{
    private readonly string _testArtistDetailsPostfix = "artists/details/";

    public ArtistDetailsPage(IWebDriver webDriver)
        : base(webDriver)
    {
    }

    private IWebElement SimilarArtistsTab =>
        WebDriver
        .WaitForElementByXPath("//a[contains(text(),'Similar Artists')]");

    private IWebElement SimilarArtistsListContainer =>
        WebDriver
        .WaitForElementByXPath("//mat-grid-list[@id='similarArtistsList']/div");

    public void OpenArtistDetailsPage(string name)
    {
        var artistDetailsUri = new Uri(BaseUrl, _testArtistDetailsPostfix + name);
        WebDriver.Navigate().GoToUrl(artistDetailsUri);
    }

    public bool IsArtistDetailsPage() => WebDriver.WaitForTitleContainsValue("details");

    public void ClickSimilarArtistsTab() => SimilarArtistsTab.Click();

    public int GetAmountOfSimilarArtists() =>
        WebDriver.WaitForChildrenLoaded(SimilarArtistsListContainer).Count;
}
