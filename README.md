# Welcome to the .NET Hacktoberfest project

We welcome PRs small and large to any .NET open source project on GitHub.




#### Hacktoberfest Project Dev Setup
This project currently uses Table storage for persistant data. 
To allow for development of this application we are using CosmosDb Emulator with Table storage API.

###### [Emulator download and install instructions](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21)


###### Emulator Configuration
For this project we are using the Table storage Emulation which is not on by default.  
To switch this on run the Cosmos Emulator with the command line switch /EnableTableEndpoint e.g.:  
 {{InstallLocation}}\Microsoft.Azure.Cosmos.Emulator.exe /EnableTableEndpoint

Once the emulator is up and running the only thing left is to set the connection string and table name in the application the appsettings.json or user secrets can be used.

The connection string should be(the account key shown here is the default installed by CosmosDB Emulator): 
DefaultEndpointsProtocol=http;AccountName=localhost;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;TableEndpoint=http://localhost:8902/;  

The table name dos not matter as it can be unique to your installation but we suggest:  
HacktoberfestProject

This will make life easier if we need to help you debug an issue.
