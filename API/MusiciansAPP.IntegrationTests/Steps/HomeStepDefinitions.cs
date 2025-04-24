using MusiciansAPP.IntegrationTests.Drivers;
using MusiciansAPP.IntegrationTests.Pages;
using TechTalk.SpecFlow;

namespace MusiciansAPP.IntegrationTests.Steps;

[Binding]
public class HomeStepDefinitions
{
    private readonly HomePage _homePage;

    public HomeStepDefinitions(BrowserDriver driver)
    {
        _homePage = new HomePage(driver.Current);
    }

    [Given(@"the user is on the Home page")]
    public void GivenTheUserIsOnTheHomePage()
    {
        _homePage.OpenHomePage();
    }
}
