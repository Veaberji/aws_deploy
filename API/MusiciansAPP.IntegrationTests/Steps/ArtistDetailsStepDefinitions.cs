using FluentAssertions;
using MusiciansAPP.IntegrationTests.Drivers;
using MusiciansAPP.IntegrationTests.Pages;
using MusiciansAPP.IntegrationTests.Support;
using TechTalk.SpecFlow;

namespace MusiciansAPP.IntegrationTests.Steps;

[Binding]
public class ArtistDetailsStepDefinitions
{
    private const string TestArtistName = "Coldplay";
    private const int SimilarArtistsExpected = 10;

    private readonly HomePage _homePage;
    private readonly ArtistDetailsPage _artistDetailsPage;

    public ArtistDetailsStepDefinitions(BrowserDriver driver)
    {
        _homePage = new HomePage(driver.Current);
        _artistDetailsPage = new ArtistDetailsPage(driver.Current);
    }

    [When(@"the user clicks on an Artist Card")]
    public void WhenTheUserClicksOnAnArtistCard()
    {
        _homePage.ClickFirstArtistCard();
    }

    [Then(@"an artist details page should be opened")]
    public void ThenAnArtistDetailsPageShouldBeOpened()
    {
        var result = _artistDetailsPage.IsArtistDetailsPage();

        result.Should().BeTrue();
    }

    [Given(@"the user is on an artist details page")]
    public void GivenTheUserIsOnAnArtistDetailsPage()
    {
        _artistDetailsPage.OpenArtistDetailsPage(TestArtistName);
    }

    [When(@"the user clicks on the Similar Artists tab")]
    public void WhenTheUserClicksOnTheSimilarArtistsTab()
    {
        _artistDetailsPage.ClickSimilarArtistsTab();
    }

    [Then(@"a list of similar artists should be opened")]
    public void ThenAListOfSimilarArtistsShouldBeOpened()
    {
        var result = _artistDetailsPage.GetAmountOfSimilarArtists();

        result.Should().Be(SimilarArtistsExpected);
    }
}
