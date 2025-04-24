Feature: Theme

Scenario: User sees the Change Theme button
	Given the user is on the Home page
	When the user clicks on the Settings button
	Then the Change Theme button should be shown

Scenario: User sees the theme switching happened
	Given the user is on the Home page
	When the user clicks on the Change Theme button
	Then the Theme should change