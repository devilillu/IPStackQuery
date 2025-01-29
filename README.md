________________________________________
1) Service.IPQuery (use argument APIKey={YOURS_API_KEY} for IPStack usage, or else runs with mock up)
/
/api/set/port/{CachePortNo}
/api/query/{IP}

2) Service.IPCache
/
/api/check/{IP}
/api/cache/{IP}

3) Service.IPBatch
/
/api/batch
/api/query/{GUID}
/api/set/port/{CachePortNo}
________________________________________

---------------------------------------------
(Console)BatchTester
Arg[0] : Number of random IPs to generage
Arg[1] : IPBatch service port
---------------------------------------------
A. Containerization: Docker for each service, for deploying in Linux docker environment.
However, couldn't resolve/make Docker internal network through bridge etc, so inter-container communication was not possible.
Tested with IIS and custom build console progam 'BatchTester' for testing IPBatch service.

B. Documentation
1) Start the three services individually
2) For IPQuery and IPBatch need to set port of cache service (/api/set/port/{CacheServicePortNo})
3) 
[a] IPQuery communicate with IPCache asking for IP info before calling external API which lead in cache update
[b] IPCache only receives calls
[c] IPBatch asks external API and then updates cache

