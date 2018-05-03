Feature: SignIn
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@SmokeTest
Scenario: Sign In with valid user credentials
	Given I have opened the website
	When I authenticate with "LoginUser1"
	Then the Projects List should be visible

Scenario: Sign In and set client
	Given I have opened the website
	When I authenticate with "LoginUser1"
	And I open the menu
	And I select a client "BT-Support"
	Then the client Projects list should be visible