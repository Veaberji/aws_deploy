using System;
using OpenQA.Selenium;

namespace MusiciansAPP.IntegrationTests.Pages;

public abstract class BasePage
{
    protected BasePage(IWebDriver webDriver)
    {
        WebDriver = webDriver;
    }

    protected Uri BaseUrl { get; } = new Uri("http://localhost:4200/");

    protected IWebDriver WebDriver { get; private init; }
}
