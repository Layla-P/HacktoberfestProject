# Welcome to the .NET Hacktoberfest project

We welcome PRs small and large to any .NET open source project on GitHub.

This project is intended to be used as a PR Tracker to help with Hacktoberfest.

Be sure to check the wiki for more information too!

### Hacktoberfest Project Dev Setup
This project currently uses Table storage for persisting data. To allow for development of this application we are using Cosmos DB Emulator
with Table storage API.

##### [Emulator download and install instructions](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21)

##### Emulator Configuration

For this project we are using the Table Storage Emulation which is not enabled by default. To enable this, run the Cosmos Emulator with the
command line switch `/EnableTableEndpoint` e.g.: `{{InstallLocation}}\Microsoft.Azure.Cosmos.Emulator.exe /EnableTableEndpoint`

Once the emulator is up and running the only thing left to do is to set the connection string and table name in the application. You can use
appsettings.json or user secrets.

The connection string should be as follows (the account key shown here is the default installed by the emulator): 
`DefaultEndpointsProtocol=http;AccountName=localhost;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;TableEndpoint=http://localhost:8902/;`

The table name doesn't matter as it can be unique to your installation. We suggest you use: `HacktoberfestProject`.

This will make life easier if we need to help you debug an issue.

###### Github application registration
* Visit https://github.com/settings/applications/new
* Set Application Name to HacktoberfestProject
* Homepage URL should be http://localhost:50085 (by default)
* Authorization Callback Url should be http://localhost:50085/github-oauth

This will provide you with the client id and secret needed for the secrets configuration below.

###### Secrets Configuration
This application uses the dotnet user-secrets tool documented [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1)

The following secrets will need setup from within the `HacktoberfestProject.Web` directory where the `.csproj` file exists. Run the following commands:

```dotnet user-secrets set "CosmosTableStorage:ConnectionString" "DefaultEndpointsProtocol=http;AccountName=localhost;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;TableEndpoint=http://localhost:8902/;"```

```dotnet user-secrets set "CosmosTableStorage:TableName" "HacktoberfestProject"```

```dotnet user-secrets set "GitHub:ClientId" "<replace with your clientid>"```

```dotnet user-secrets set "GitHub:ClientSecret" "<replace with your client secret>"```

#### Known Issues
The following error occurs due to using http for local development instead of SSL with the Chrome Browser:

*An unhandled exception occurred while processing the request. Exception: Correlation failed.*

To correct the problem:
* Visit chrome://flags
* Change setting `Cookies without SameSite must be secure` to `Disabled`.
