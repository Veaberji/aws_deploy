using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MusiciansAPP.IntegrationTests.Support;

public static class IWebDriverExtensions
{
    public static IWebElement FindElementById(this IWebDriver webDriver, string id)
    {
        try
        {
            return webDriver.FindElement(By.Id(id));
        }
        catch (NoSuchElementException)
        {
            return null;
        }
    }

    public static IWebElement FindElementByXPath(this IWebDriver webDriver, string path)
    {
        return webDriver.FindElement(By.XPath(path));
    }

    public static IWebElement GetElementByName(this IWebDriver webDriver, string name)
    {
        return webDriver.FindElement(By.TagName(name));
    }

    public static string WaitForNonEmptyInput(this IWebDriver webDriver, IWebElement element)
    {
        return webDriver.WaitUntil(
            () => element.GetAttribute("value"),
            result => !string.IsNullOrEmpty(result));
    }

    public static string WaitForEmptyInput(this IWebDriver webDriver, IWebElement element)
    {
        return webDriver.WaitUntil(
            () => element.GetAttribute("value"),
            result => string.IsNullOrEmpty(result));
    }

    public static string WaitForNonEmptyElement(this IWebDriver webDriver, IWebElement element)
    {
        return webDriver.WaitUntil(
            () => element.GetAttribute("textContent"),
            result => !string.IsNullOrEmpty(result));
    }

    public static string WaitForEmptyElement(this IWebDriver webDriver, IWebElement element)
    {
        return webDriver.WaitUntil(
            () => element.GetAttribute("textContent"),
            result => string.IsNullOrEmpty(result));
    }

    public static IWebElement WaitForElementByXPath(
        this IWebDriver webDriver,
        string path,
        WaitPeriodInSeconds timeout = WaitPeriodInSeconds.Long)
    {
        Func<IWebElement> getResult = () =>
        {
            IWebElement element = null;
            try
            {
                element = webDriver.FindElementByXPath(path);
            }
            catch (NoSuchElementException)
            {
            }

            return element;
        };

        Func<IWebElement, bool> isResultAccepted = result =>
        {
            var now = DateTime.Now;
            return result is not null || (DateTime.Now - now) - GetTimeSpanFromSeconds(timeout) > TimeSpan.Zero;
        };

        var element = webDriver.WaitAndCheckForInterval(timeout, getResult, isResultAccepted);

        return element is null ? throw new NoSuchElementException() : element;
    }

    public static IReadOnlyList<IWebElement> WaitForChildrenLoaded(this IWebDriver webDriver, IWebElement element)
    {
        const string ChildXPath = "./child::*";
        return webDriver.WaitUntil(
            () => element.FindElements(By.XPath(ChildXPath)),
            result => result.Any());
    }

    public static string WaitForElementClassToHaveValue(this IWebDriver webDriver, IWebElement element, string value)
    {
        return webDriver.WaitUntil(
            () => element.GetAttribute("className"),
            result => result.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    public static bool WaitForTitleContainsValue(this IWebDriver webDriver, string value)
    {
        return webDriver.WaitUntil(
            () => webDriver.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase),
            result => result == true);
    }

    public static T WaitAndCheckForInterval<T>(
        this IWebDriver webDriver,
        WaitPeriodInSeconds timeout,
        Func<T> getResult,
        Func<T, bool> isResultAccepted,
        int intervalInMilliseconds = 500)
    {
        var wait = new WebDriverWait(webDriver, GetTimeSpanFromSeconds(timeout))
        {
            PollingInterval = TimeSpan.FromMilliseconds(intervalInMilliseconds),
        };
        return wait.Until(driver =>
        {
            var result = getResult();
            if (!isResultAccepted(result))
            {
                return default;
            }

            return result;
        });
    }

    private static TimeSpan GetTimeSpanFromSeconds(WaitPeriodInSeconds seconds)
    {
        return TimeSpan.FromSeconds((double)seconds);
    }

    private static T WaitUntil<T>(this IWebDriver webDriver, Func<T> getResult, Func<T, bool> isResultAccepted)
    {
        var wait = new WebDriverWait(webDriver, GetTimeSpanFromSeconds(WaitPeriodInSeconds.Default));
        return wait.Until(driver =>
        {
            var result = getResult();
            if (!isResultAccepted(result))
            {
                return default;
            }

            return result;
        });
    }
}
