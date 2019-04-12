# Getting Started

Starting the repo is possible either with `docker-compose` or `dotnet cli`:

`docker-compose up`

To run the tests, once the system is started use `dotnet test`.

# System Scalability

The system uses a simple mechanism for maintaining an atomic counter, using redis as the backend. Using the INCR command in redis we can easily maintain a count of streams per user. Combining this with LUA scripts we can avoid any race conditions with getting, then setting or watching values in redis. A single command provides the atomic increment needeed.

Given AWS is used, if large scalability is required then I would look at sharding keys across multiple instances of redis or using https://aws.amazon.com/elasticache/redis/ to provide a managed redis cluster. Redis is pretty scalable as is so this may not be required depending on required load.

The main reason behind redis was to use a key value store that provided atomic counters or some support for atomic transactions for a given key with sharding capabilities. Given AWS is used you could also use something like DynamoDB if already in use.

