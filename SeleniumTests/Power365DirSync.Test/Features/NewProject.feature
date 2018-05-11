Feature: NewProject
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Create New Project
	Given I have signed into the website as "LoginUser1"
	And I have clicked "New Project" link in the menu
	Then the "Edit Project" page should be displayed
	When I have complete the wizard with "Integration1" workflow settings
	Then the "Project Overview" page should be displayed
