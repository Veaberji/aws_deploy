using BoDi;
using MusiciansAPP.IntegrationTests.Drivers;
using TechTalk.SpecFlow;

namespace MusiciansAPP.IntegrationTests.Hooks;

[Binding]
public class SharedBrowserHooks
{
    [BeforeTestRun]
    public static void BeforeTestRun(ObjectContainer container)
    {
        container.BaseContainer.Resolve<BrowserDriver>();
    }
}
