## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-findapprenticeship-api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Vacancy%20Services/Find%20Apprenticeship/das-findapprenticeship-api?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-api&branchName=main)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Vacancy%20Services/Find%20Apprenticeship/das-findapprenticeship-api?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-api&branchName=main)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-findapprenticeship-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-findapprenticeship-api)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

The das-findapprenticeship-api(https://github.com/SkillsFundingAgency/das-findapprenticeship-api) is the inner api for retrieving and filtering apprenticeship vacancies relying on the Azure Search created from das-findapprenticeship.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-findapprenticeship-api)
* A storage emulator like Azurite
* 
### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-findapprenticeship-api/SFA.DAS.FindApprenticeships.Api.json)

* If you are using Azure Storage Emulator for local development purpose, then In your Azure Storage Account, create a table called Configuration and Add the following

```
ParitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeships.Api_1.0
Data: {
  "FindApprenticeshipsApi": {
    "AzureSearchBaseUrl": "{{Azure Search url}}"
  },
  "AzureAd": {
    "tenant": "{{tenant}}",
    "identifier": "{{identifier}}"
  }
}
```

In the web project, if it does not exist already, add `AppSettings.Development.json` file with the following content:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.FindApprenticeships.Api",
  "Environment": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": "",
  "AllowedHosts": "*"
}
```

## Technologies
* .NetCore 8.0
* Azure Storage Account
* Azure Search
* NUnit
* Moq
* FluentAssertions
* Azure App Insights
* MediatR

## How It Works

### Running

* Open command prompt and change directory to _**/src/SFA.DAS.FAA.Api/**_
* Run the web project _**/src/SFA.DAS.FAA.csproj**_

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

### Application logs
Application logs are logged to [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) and can be viewed using [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/overview) at https://portal.azure.com

## Useful URLs

### Health check
https://localhost:5051/health - Endpoint to check the Azure search re-indexing health status

### Vacancies

https://localhost:5051/api/vacancies - Endpoint to get all vacancies

https://localhost:5051/api/vacancies/{vacancyReference} - Endpoint to get a particular vacancy

https://localhost:5051/api/vacancies/count - Endpoint to get vacancies count
