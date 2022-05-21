RandomOrgDotNet

A simple async .net client for the random.org [v4 JSON-RPC API](https://api.random.org/json-rpc/4).

> "RANDOM.ORG is a true random number service that generates randomness via atmospheric noise" - https://www.random.org/history/

N.b. that [official libraries](https://github.com/RandomOrg/JSON-RPC-.NET) also exist as well as multiple unofficial ones.

This was implemented to test [StreamJsonRpc](https://github.com/microsoft/vs-streamjsonrpc).

Supported:
 - Basic API v4
 - "Advisory wait" rate limiting
 - Replay of historic/seeded data 

Not supported:
 - Signed API 
 - Caching
