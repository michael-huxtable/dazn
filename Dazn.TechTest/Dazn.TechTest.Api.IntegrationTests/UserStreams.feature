Feature: User stream count feature
  As a business
  I want to be able to limit users to stream a maximum of 3 videos at once
  so that users cannot abuse the system
  
Scenario: New user can watch stream
	Given user with id 1 is not streaming video
	When the stream count is updated 1 time(s)
	Then the status code of the last response should be 200
	And the content of the last response should be 1

Scenario: User can only request 3 streams
	Given user with id 1 is not streaming video
	When the stream count is updated 7 time(s)
	Then I should get a bad request response for the last 4 requests
	
Scenario: User cannot brute force the limit
	Given user with id 1 is not streaming video
	When the stream count is updated 500 times in parallel
	Then I should get a bad request response for 497 requests
	