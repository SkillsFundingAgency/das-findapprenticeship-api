# das-findapprenticeship-api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Vacancy%20Services/Find%20Apprenticeship/das-findapprenticeship-api?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-api&branchName=main)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Vacancy%20Services/Find%20Apprenticeship/das-findapprenticeship-api?repoName=SkillsFundingAgency%2Fdas-findapprenticeship-api&branchName=main)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-findapprenticeship-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-findapprenticeship-api)

The das-findapprenticeship-api is the inner api for retrieving and filtering apprenticeship vacancies relying on the ElasticIndex created from das-findapprenticeship.

## How It Works

### Requirements
• DotNet Core 3.1 and any supported IDE for DEV running.
• Azure Storage Account
• ElasticIndex created from das-findapprenticeship

### Configuration
• In your Azure Storage Account, create a table called Configuration and Add the following
```
ParitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeships.Api_1.0
Data: {"FindApprenticeshipsApi": {"ElasticSearchUsername": "username", "ElasticSearchPassword": "password", "ElasticSearchServerUrl": "serverUrl"},
}
```
