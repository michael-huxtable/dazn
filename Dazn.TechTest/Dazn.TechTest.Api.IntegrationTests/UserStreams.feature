Feature: User stream count feature
  As a business
  I want to be able to limit users to stream a maximum of 3 videos at once
  so that users cannot abuse the system
  
Scenario: New user can watch stream
	Given user with id 1 is not streaming video
	When the stream count is updated 1 time(s)
	Then I should get a response with count of 1

Scenario: User can only request 3 streams
	Given user with id 1 is not streaming video
	When the stream count is updated 4 times(s)
	Then I should get a bad request response with exceeded limit message
