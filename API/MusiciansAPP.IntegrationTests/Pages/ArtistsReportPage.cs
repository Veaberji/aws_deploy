using System;
using System.IO;
using System.Linq;
using MusiciansAPP.IntegrationTests.Support;
using OpenQA.Selenium;

namespace MusiciansAPP.IntegrationTests.Pages;

public class ArtistsReportPage : BasePage
{
    private readonly string _artistsReportPostfix = "artists/report";

    public ArtistsReportPage(IWebDriver webDriver)
        : base(webDriver)
    {
    }

    private IWebElement NameSearchInput => WebDriver.FindElementById("nameSearch");

    private IWebElement AmountInput => WebDriver.FindElementById("amount");

    private IWebElement NameSearchMaxLengthError => WebDriver.FindElementById("nameSearchMaxLengthError");

    private IWebElement AmountRequiredError => WebDriver.FindElementById("amountRequiredError");

    private IWebElement AmountMinError => WebDriver.FindElementById("amountMinError");

    private IWebElement AmountMaxError => WebDriver.FindElementById("amountMaxError");

    private IWebElement GenerateButton => WebDriver.FindElementById("generateButton");

    private IWebElement CancelButton => WebDriver.FindElementById("cancelButton");

    public bool WaitForReportFileResult(
        string path,
        string startsWith,
        WaitPeriodInSeconds timeout = WaitPeriodInSeconds.Long)
    {
        var getResult = () => DoesOneReportFileExist(path, startsWith);
        Func<bool, bool> isResultAccepted = result => result == true;
        try
        {
            return WebDriver.WaitAndCheckForInterval(timeout, getResult, isResultAccepted);
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public void OpenArtistsReportPage()
    {
        var artistsReportUri = new Uri(BaseUrl, _artistsReportPostfix);
        WebDriver.Navigate().GoToUrl(artistsReportUri);
    }

    public bool IsArtistsReportPage() =>
        WebDriver.WaitForTitleContainsValue("artists")
        && WebDriver.WaitForTitleContainsValue("report");

    public void InputNameSearch(string text) => NameSearchInput.SendKeys(text);

    public void InputTabToNameSearch() => NameSearchInput.SendKeys(Keys.Tab);

    public void InputTabToAmount() => AmountInput.SendKeys(Keys.Tab);

    public void InputAmount(int amount) => AmountInput.SendKeys(amount.ToString());

    public void ClickGenerateButton() => GenerateButton.Click();

    public void ClickCancel() => CancelButton.Click();

    public string GetNonEmptyNameSearchResult() => WebDriver.WaitForNonEmptyInput(NameSearchInput);

    public string GetEmptyNameSearchResult() => WebDriver.WaitForEmptyInput(NameSearchInput);

    public string GetEmptyAmountResult() => WebDriver.WaitForEmptyInput(AmountInput);

    public bool DoesNameSearchMaxLengthErrorExist() => NameSearchMaxLengthError is not null;

    public bool DoesAmountRequiredErrorExist() => AmountRequiredError is not null;

    public bool DoesAmountMinErrorExist() => AmountMinError is not null;

    public bool DoesAmountMaxErrorExist() => AmountMaxError is not null;

    public string GetNonEmptyNameSearchMaxLengthErrorResult() =>
        WebDriver.WaitForNonEmptyElement(NameSearchMaxLengthError);

    public string GetNonEmptyAmountRequiredErrorResult() =>
        WebDriver.WaitForNonEmptyElement(AmountRequiredError);

    public string GetNonEmptyAmountMinErrorResult() => WebDriver.WaitForNonEmptyElement(AmountMinError);

    public string GetNonEmptyAmountMaxErrorResult() => WebDriver.WaitForNonEmptyElement(AmountMaxError);

    public void ClickAmount() => AmountInput.Click();

    public bool IsCancelButtonEnabled() => CancelButton.Enabled;

    public bool IsGenerateButtonEnabled() => GenerateButton.Enabled;

    private static bool DoesOneReportFileExist(string dirPath, string startsWith)
    {
        const string FileType = "csv";
        var filenames = Directory
            .EnumerateFiles(dirPath, "*", SearchOption.AllDirectories)
            .Select(f => Path.GetFileName(f))
            .Where(name => name.StartsWith(startsWith, StringComparison.InvariantCultureIgnoreCase)
                && name.EndsWith(FileType, StringComparison.InvariantCultureIgnoreCase));

        return filenames.Count() == 1;
    }
}