Feature: ArtistDetails

Scenario: User sees an artist details page being opened
	Given the user is on the Home page
	When the user clicks on an Artist Card
	Then an artist details page should be opened

Scenario: User sees a list of similar artists
	Given the user is on an artist details page
	When the user clicks on the Similar Artists tab
	Then a list of similar artists should be opened