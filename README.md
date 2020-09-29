# Welcome to the .NET Hacktoberfest project

We welcome PRs small and large to any .NET open source project on GitHub.

This project is intended to be used as a PR Tracker to help with Hacktoberfest.

#### Hacktoberfest Project Dev Setup
This project currently uses Table storage for persisting data. To allow for development of this application we are using CosmosDB Emulator
with Table storage API.

###### [Emulator download and install instructions](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21)

###### Emulator Configuration
For this project we are using the Table Storage Emulation which is not enabled by default. To enable this, run the Cosmos Emulator with the
command line switch `/EnableTableEndpoint` e.g.: `{{InstallLocation}}\Microsoft.Azure.Cosmos.Emulator.exe /EnableTableEndpoint`

Once the emulator is up and running the only thing left to do is to set the connection string and table name in the application. You can use
appsettings.json or user secrets.

The connection string should be as follows (the account key shown here is the default installed by CosmosDB Emulator): 
`DefaultEndpointsProtocol=http;AccountName=localhost;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;TableEndpoint=http://localhost:8902/;`

The table name doesn't matter as it can be unique to your installation. We suggest you use:  
`HacktoberfestProject`

This will make life easier if we need to help you debug an issue.
