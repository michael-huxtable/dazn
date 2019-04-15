# Getting Started

Starting the repo is possible either with `docker-compose` or `dotnet cli`:

`docker-compose up` / `dotnet run`

To run the tests, once the system is started use `dotnet test` (Requires .net core SDK). Currently for development the tests use an in-memory version of the API, but the docker compose script sets up Redis. The API is provided for convenience and local testing.
Once the API is setup, the service provides two endpoints:

 *Post* `http://localhost:5000/{userId}/stream`
 *Delete* `http://localhost:5000/{userId}/stream`

The POST increments the count of a given `userId`. The user id is not validated so any int can be provided.
The DELETE resets the count of a given `userId`, by removing the key in Redis. The user id is not validated so any int can be provided.

# Integration Tests

BDD via Specflow has been used to test the endpoint. Only integration tests for the project exist given that the code is simple and does not gain value from unit tests, the value is within testing the data ends up in Redis and works end to end.

# Notes

The API endpoint is not intended to be publicly accessible, given it is a simple endpoint where if discovered a user could easily abuse the stream count. This endpoint would have to be internal, being used by other APIs and services to maintain the code of streams in a more controlled environment.

# System Scalability

The system uses a simple mechanism for maintaining an atomic counter, using redis as the backend. Using the INCR command in redis we can easily maintain a count of streams per user. Combining this with LUA scripts we can avoid any race conditions with getting, then setting or watching values in redis. A single command provides the atomic increment needed.

Given AWS is used, if large scalability is required then I would look at sharding keys across multiple instances of redis or using https://aws.amazon.com/elasticache/redis/ to provide a managed redis cluster. Redis is pretty scalable as-is so this may not be required depending on required load.

The main reason behind redis was to use a key value store that provided atomic counters or some support for atomic transactions for a given key with sharding capabilities. Given AWS is used you could also use something like DynamoDB if already in use.

