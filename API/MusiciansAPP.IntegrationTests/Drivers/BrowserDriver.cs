using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MusiciansAPP.IntegrationTests.Drivers;

public class BrowserDriver : IDisposable
{
    private readonly Lazy<IWebDriver> _lazyWebDriver;
    private bool _isDisposed;

    public BrowserDriver()
    {
        _lazyWebDriver = new Lazy<IWebDriver>(CreateWebDriver);
    }

    public IWebDriver Current => _lazyWebDriver.Value;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (_lazyWebDriver.IsValueCreated)
        {
            Current.Quit();
        }

        _isDisposed = true;
    }

    private IWebDriver CreateWebDriver()
    {
        var service = ChromeDriverService.CreateDefaultService();
        return new ChromeDriver(service);
    }
}