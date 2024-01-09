# 418 challenge notes
1. This code was modified from my personal projects developed over many years
2. Solution struture is designed to isolate endpoints, services, and data
3. Data layer uses in-memory database for simplicity 
4. Docker is not enabled, but could be with a little more effort
5. Authentication and Authorization is disabled, but can easily be configured IDP's like Azure Entra ID or Azure B22
6. Logging is configured to use Serilog with a file sink

# 418 challenge startup
1. Set startup progject to Four18.Challenge.WebApi
2. Build solution
3. Run solution (http or https)
4. Swagger UI will be displayed and the endpoints can be invoked

# 418 challenge project structure

Four18.Challenge.WebApi contains the web api startup and configuration, middleware, and controllers (e.g. http endpoints)
Four18.Challenge.Business contains the services and busisness rules (e.g. data validation) and data mapping to and from webapi and data layers
Four18.Challenge.Data contains the data repository (e.g. in-memory database) 
Four18.Common.* contains common code used in the API stack and is used for multiple api solutions.  Note that the common libraries are ment to be a NuGet package to be used by the multiple api projects.
