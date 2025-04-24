Feature: ServiceUnavailableMessage

@needsToBreakDataProviders
Scenario: User sees the error message if Data Providers are unavailable
	When the user opens the Home page
	Then the Service Unavailable Message should be shown up
