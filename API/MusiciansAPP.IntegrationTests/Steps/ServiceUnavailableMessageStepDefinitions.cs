using FluentAssertions;
using MusiciansAPP.IntegrationTests.Drivers;
using MusiciansAPP.IntegrationTests.Pages;
using TechTalk.SpecFlow;

namespace MusiciansAPP.IntegrationTests.Steps;

[Binding]
public class ServiceUnavailableMessageStepDefinitions
{
    private readonly HomePage _homePage;

    public ServiceUnavailableMessageStepDefinitions(BrowserDriver driver)
    {
        _homePage = new HomePage(driver.Current);
    }

    [When(@"the user opens the Home page")]
    public void WhenTheUserOpensTheHomePage()
    {
        _homePage.OpenHomePage();
    }

    [Then(@"the Service Unavailable Message should be shown up")]
    public void ThenTheServiceUnavailableMessageShouldBeShownUp()
    {
        var result = _homePage.DoesServiceUnavailableErrorExist();

        result.Should().BeTrue();
    }
}
