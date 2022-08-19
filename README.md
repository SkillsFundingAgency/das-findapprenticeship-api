# das-findapprenticeship-api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/_projectname_?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=_projectid_&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)

The das-findapprenticeship-api is the inner api for retrieving and filtering apprenticeship vacancies relying on the FAA elastic index.

## How It Works

### Requirements
• DotNet Core 3.1 and any supported IDE for DEV running.
• If you are not wishing to run the in memory database then
• SQL Server database.
• Azure Storage Account
• Elastic 

### Configuration
• In your Azure Storage Account, create a table called Configuration and Add the following
```
ParitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeships.Api_1.0
Data: {"FindApprenticeshipsApi": {"ElasticSearchUsername": "username", "ElasticSearchPassword": "password", "ElasticSearchServerUrl": "serverUrl"},
}
```
