using FluentAssertions;
using MusiciansAPP.IntegrationTests.Drivers;
using MusiciansAPP.IntegrationTests.Pages;
using TechTalk.SpecFlow;

namespace MusiciansAPP.IntegrationTests.Steps;

[Binding]
public class ArtistsReportStepDefinitions
{
    private const int MaxNameLength = 100;
    private const int MinAmount = 10;
    private const int MaxAmount = 1000;

    private const string TestSpecialCharactersNameSearch = "ABC abc!\"#$%&\'()*+,-./:;<=>?@[]^_`{|}~";
    private const string TestNameSearchExpected = "abc abc";

    private const string TestNameSearchOne = "test search string";
    private const int TestAmount = 1;

    private const string TestDownloadsPath = "C:\\Users\\p2.morozov\\Downloads\\";
    private const string TestNameSearchTwo = "a";

    private readonly HomePage _homePage;
    private readonly ArtistsReportPage _artistsReportPage;

    public ArtistsReportStepDefinitions(BrowserDriver driver)
    {
        _homePage = new HomePage(driver.Current);
        _artistsReportPage = new ArtistsReportPage(driver.Current);
    }

    [When(@"the user clicks on the Artists Report link")]
    public void WhenTheUserClicksOnTheArtistsReportLink()
    {
        _homePage.ClickArtistsReportButton();
    }

    [Then(@"the Artists Report page should be opened")]
    public void ThenTheArtistsReportPageShouldOpen()
    {
        var result = _artistsReportPage.IsArtistsReportPage();

        result.Should().BeTrue();
    }

    [Given(@"the user is on the Artists Report page")]
    public void GivenTheUserIsOnTheArtistsReportPage()
    {
        _artistsReportPage.OpenArtistsReportPage();
    }

    [When(@"the user enters correct data to the input fields")]
    public void WhenTheUserEntersCorrectDataToTheInputFields()
    {
        _artistsReportPage.InputNameSearch("a");
        _artistsReportPage.InputAmount(MinAmount + 1);
    }

    [When(@"the user enters special characters to the Search input field")]
    public void WhenTheUserEntersSpecialCharactersToTheSearchInputField()
    {
        _artistsReportPage.InputNameSearch(TestSpecialCharactersNameSearch);
    }

    [Then(@"the Search input field contains only allowed symbols")]
    public void ThenTheSearchInputFieldContainsOnlyAllowedSymbols()
    {
        var result = _artistsReportPage.GetNonEmptyNameSearchResult();

        result.Should().Be(TestNameSearchExpected);
    }

    [When(@"the user enters characters to the Search and Amount input fields")]
    public void WhenTheUserEntersCharactersToTheSearchAndAmountInputFields()
    {
        _artistsReportPage.InputNameSearch(TestNameSearchOne);
        _artistsReportPage.InputAmount(TestAmount);
    }

    [When(@"the user presses the Cancel button")]
    public void WhenTheUserPressesTheCancelButton()
    {
        _artistsReportPage.ClickCancel();
    }

    [Then(@"the input fields get clear")]
    public void ThenTheInputFieldsGetClear()
    {
        var nameSearchResult = _artistsReportPage.GetEmptyNameSearchResult();
        var amountResult = _artistsReportPage.GetEmptyAmountResult();

        nameSearchResult.Should().BeNullOrEmpty();
        amountResult.Should().BeNullOrEmpty();
    }

    [When(@"the user enters string equals to max length to the Search input field")]
    public void WhenTheUserEntersStringEqualsToMaxLengthToTheSearchInputField()
    {
        var testString = new string('a', MaxNameLength);
        _artistsReportPage.InputNameSearch(testString);
        _artistsReportPage.InputTabToNameSearch();

        var result = _artistsReportPage.DoesNameSearchMaxLengthErrorExist();

        result.Should().BeFalse();
    }

    [When(@"the user enters another symbol")]
    public void WhenTheUserEntersAnotherSymbol()
    {
        _artistsReportPage.InputNameSearch("a");
        _artistsReportPage.InputTabToNameSearch();
    }

    [Then(@"the search error message should be shown")]
    public void ThenTheSearchErrorMessageShouldBeShown()
    {
        var doesExist = _artistsReportPage.DoesNameSearchMaxLengthErrorExist();
        var result = _artistsReportPage.GetNonEmptyNameSearchMaxLengthErrorResult();

        doesExist.Should().BeTrue();
        result.Should().ContainEquivalentOf("max length");
    }

    [When(@"the user touches amount input field")]
    public void WhenTheUserTouchesAmountInputField()
    {
        _artistsReportPage.ClickAmount();
    }

    [When(@"the user leaves it empty")]
    public void WhenTheUserLeavesItEmpty()
    {
        _artistsReportPage.InputTabToAmount();
    }

    [Then(@"the required amount error message should be shown")]
    public void ThenTheRequiredAmountErrorMessageShouldBeShown()
    {
        var doesExist = _artistsReportPage.DoesAmountRequiredErrorExist();
        var result = _artistsReportPage.GetNonEmptyAmountRequiredErrorResult();

        doesExist.Should().BeTrue();
        result.Should().ContainEquivalentOf("required");
    }

    [When(@"the user enters amount less than min to the Amount input field")]
    public void WhenTheUserEntersAmountLessThanMinToTheAmountInputField()
    {
        _artistsReportPage.InputAmount(MinAmount - 1);
        _artistsReportPage.InputTabToAmount();
    }

    [Then(@"the min amount error message should be shown")]
    public void ThenTheMinAmountErrorMessageShouldBeShown()
    {
        var doesExist = _artistsReportPage.DoesAmountMinErrorExist();
        var result = _artistsReportPage.GetNonEmptyAmountMinErrorResult();

        doesExist.Should().BeTrue();
        result.Should().ContainEquivalentOf("min");
    }

    [When(@"the user enters the amount more than max to the Amount input field")]
    public void WhenTheUserEntersTheAmountMoreThanMaxToTheAmountInputField()
    {
        _artistsReportPage.InputAmount(MaxAmount + 1);
        _artistsReportPage.InputTabToAmount();
    }

    [Then(@"the max amount error message should be shown")]
    public void ThenTheMaxAmountErrorMessageShouldBeShown()
    {
        var doesExist = _artistsReportPage.DoesAmountMaxErrorExist();
        var result = _artistsReportPage.GetNonEmptyAmountMaxErrorResult();

        doesExist.Should().BeTrue();
        result.Should().ContainEquivalentOf("max");
    }

    [When(@"the user enters the (.*) within the acceptable limits")]
    public void WhenTheUserEntersTheWithinTheAcceptableLimits(int amount)
    {
        _artistsReportPage.InputAmount(amount);
        _artistsReportPage.InputTabToAmount();
    }

    [Then(@"neither the max nor the min amount error messages should be shown")]
    public void ThenNeitherTheMaxNorTheMinAmountErrorMessagesShouldBeShown()
    {
        var doesMinErrorExist = _artistsReportPage.DoesAmountMinErrorExist();
        var doesMaxErrorExist = _artistsReportPage.DoesAmountMaxErrorExist();

        doesMinErrorExist.Should().BeFalse();
        doesMaxErrorExist.Should().BeFalse();
    }

    [Given(@"the cancel button is disabled")]
    public void GivenTheCancelButtonIsDisabled()
    {
        var result = _artistsReportPage.IsCancelButtonEnabled();

        result.Should().BeFalse();
    }

    [When(@"the user enters data to the search input field")]
    public void WhenTheUserEntersDataToTheSearchInputField()
    {
        _artistsReportPage.InputNameSearch("a");
    }

    [Then(@"the cancel button should be enabled")]
    public void ThenTheCancelButtonShouldBeEnabled()
    {
        var result = _artistsReportPage.IsCancelButtonEnabled();

        result.Should().BeTrue();
    }

    [When(@"the user enters data to the amount input field")]
    public void WhenTheUserEntersDataToTheAmountInputField()
    {
        _artistsReportPage.InputAmount(MaxAmount + 1);
    }

    [Given(@"the user fills the form input fields with some data")]
    public void GivenTheUserFillsTheFormInputFieldsWithSomeData()
    {
        _artistsReportPage.OpenArtistsReportPage();

        _artistsReportPage.InputNameSearch("a");
        _artistsReportPage.InputAmount(MinAmount + 1);
    }

    [When(@"the user clicks the cancel button")]
    public void WhenTheUserClicksTheCancelButton()
    {
        _artistsReportPage.ClickCancel();
    }

    [Then(@"the cancel button should be disabled")]
    public void ThenTheCancelButtonShouldBeDisabled()
    {
        var result = _artistsReportPage.IsCancelButtonEnabled();

        result.Should().BeFalse();
    }

    [Then(@"the generate button should be enabled")]
    public void ThenTheGenerateButtonShouldBeEnabled()
    {
        var result = _artistsReportPage.IsGenerateButtonEnabled();

        result.Should().BeTrue();
    }

    [Then(@"the generate button should be disabled")]
    public void ThenTheGenerateButtonShouldBeDisabled()
    {
        var result = _artistsReportPage.IsGenerateButtonEnabled();

        result.Should().BeFalse();
    }

    [Given(@"the user fills the form input fields with correct data")]
    public void GivenTheUserFillsTheFormInputFieldsWithCorrectData()
    {
        _artistsReportPage.OpenArtistsReportPage();
        _artistsReportPage.InputNameSearch("a");
        _artistsReportPage.InputAmount(MinAmount + 1);
    }

    [Given(@"the csv file does not exist")]
    public void GivenTheCsvFileDoesNotExist()
    {
        var doesExist = _artistsReportPage
            .WaitForReportFileResult(TestDownloadsPath, TestNameSearchTwo, Support.WaitPeriodInSeconds.Short);

        doesExist.Should().BeFalse();
    }

    [When(@"the user clicks Generate button")]
    public void WhenTheUserClicksGenerateButton()
    {
        _artistsReportPage.ClickGenerateButton();
    }

    [Then(@"the csv report should be downloaded")]
    public void ThenTheCsvReportShouldBeDownloaded()
    {
        var doesExist = _artistsReportPage.WaitForReportFileResult(TestDownloadsPath, TestNameSearchTwo);

        doesExist.Should().BeTrue();
    }
}
