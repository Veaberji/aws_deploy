Feature: ArtistsReport

Scenario: User sees the report page
	Given the user is on the Home page
	When the user clicks on the Artists Report link
	Then the Artists Report page should be opened

Scenario: User sees only allowed characters if they enter special characters to the Search input field
	Given the user is on the Artists Report page
	When the user enters special characters to the Search input field
	Then the Search input field contains only allowed symbols

Scenario: User sees empty report form after clicking the Cancel button
	Given the user is on the Artists Report page
	When the user enters characters to the Search and Amount input fields
	And the user presses the Cancel button
	Then the input fields get clear

Scenario: User sees a validation error on the report page if they enter too long string to the Search input field
	Given the user is on the Artists Report page
	When the user enters string equals to max length to the Search input field
	And the user enters another symbol
	Then the search error message should be shown
	
Scenario: User sees a validation error on the report page if they enter amount less than min to the Amount input field
	Given the user is on the Artists Report page
	When the user enters amount less than min to the Amount input field
	Then the min amount error message should be shown

Scenario: User sees a validation error on the report page if they don't fill the Amount input field
	Given the user is on the Artists Report page
	When the user touches amount input field
	And the user leaves it empty
	Then the required amount error message should be shown
	
Scenario: User sees a validation error on the report page if they enter amount more than max to the Amount input field
	Given the user is on the Artists Report page
	When the user enters the amount more than max to the Amount input field
	Then the max amount error message should be shown

Scenario: User doesn't see any error on the report page if they enter correct data
	Given the user is on the Artists Report page
	When the user enters the <amount> within the acceptable limits
	Then neither the max nor the min amount error messages should be shown

  Examples:
	| amount |
	|     10 |
	|     11 |
	|    999 |
	|   1000 |

Scenario: User sees the Cancel button being enabled if they fill any input field 
	Given the user is on the Artists Report page
	And the cancel button is disabled
	When the user enters data to the search input field
	Then the cancel button should be enabled
	When the user enters data to the amount input field
	Then the cancel button should be enabled
	
Scenario: User sees the Cancel button being disabled if they click the Cancel button
	Given the user fills the form input fields with some data
	When the user clicks the cancel button
	Then the cancel button should be disabled
	
Scenario: User sees the Generate button being enabled if they enter correct data to the input fields
	Given the user is on the Artists Report page
	When the user enters correct data to the input fields
	Then the generate button should be enabled
	
Scenario: User sees the Generate button being disabled if they click the Cancel button
	Given the user fills the form input fields with some data
	When the user clicks the cancel button
	Then the generate button should be disabled

Scenario: User sees the csv report being downloaded if they fill the form and press the Generate button
	Given the user fills the form input fields with correct data
	And the csv file does not exist
	When the user clicks Generate button
	Then the csv report should be downloaded
