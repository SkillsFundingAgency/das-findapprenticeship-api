using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Search.Documents;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Data.AzureSearch;
using SFA.DAS.FAA.Data.Extensions;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Data.UnitTests.AzureSearch;

public class WhenBuildingFilters
{
    [Test]
    public void Then_The_Default_Filter_Is_Added()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be(AzureSearchConstants.VacancySourceEqualsRaa);
    }

    [Test]
    public void Then_The_AdditionalDataSources_Are_Added_To_Filter()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = [DataSource.Nhs, DataSource.Csj],
            VacancySort = VacancySort.AgeAsc,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"(VacancySource eq 'RAA' or VacancySource eq 'Nhs' or VacancySource eq 'Csj')");
    }

    [Test]
    public void Then_If_Sort_Is_ExpectedStartDate_AdditionalDataSources_Are_Ignored()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = [DataSource.Nhs],
            VacancySort = VacancySort.ExpectedStartDateAsc,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be(AzureSearchConstants.VacancySourceEqualsRaa);
    }

    [Test, AutoData]
    public void Then_The_Ukprn_Filter_Is_Added(int ukprn)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = ukprn,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and Ukprn eq '{ukprn}'");
    }

    [Test, AutoData]
    public void Then_The_AccountPublicHashedId_Filter_Is_Added(string accountId)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = accountId,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and AccountPublicHashedId eq '{accountId}'");
    }

    [Test, AutoData]
    public void Then_The_AccountLegalEntityPublicHashedId_Filter_Is_Added(string accountLegalEntityId)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = accountLegalEntityId,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and AccountLegalEntityPublicHashedId eq '{accountLegalEntityId}'");
    }

    [Test, AutoData]
    public void Then_The_EmployerName_Filter_Is_Added(string employerName)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = employerName,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and EmployerName eq '{employerName}'");
    }

    [Test, AutoData]
    public void Then_The_StandardLarsCode_Filter_Is_Added(List<int> larsCodes)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = larsCodes,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        var larsCodeFilters = string.Join(" or ", larsCodes.Select(x => $"Course/LarsCode eq {x}"));

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and ({larsCodeFilters})");
    }

    [Test, AutoData]
    public void Then_The_Categories_Filter_Is_Added(List<string> categories)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = categories,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        var categoryFilters = string.Join(" or ", categories.Select(x => $"Route eq '{x}'"));

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and ({categoryFilters})");
    }

    [Test, AutoData]
    public void Then_The_RouteIds_Filter_Is_Added(List<int> routeIds)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = routeIds,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        var routeIdFilters = string.Join(" or ", routeIds.Select(x => $"Course/RouteCode eq {x}"));

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and ({routeIdFilters})");
    }

    [Test, AutoData]
    public void Then_The_Levels_Filter_Is_Added(List<string> levels)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = levels,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        var levelFilters = string.Join(" or ", levels.Select(x => $"Course/Level eq '{x}'"));

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and ({levelFilters})");
    }

    [Test, AutoData]
    public void Then_The_Geo_Distance_Filter_Is_Added(double lat, double lon, uint distance)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = lat,
            Lon = lon,
            DistanceInMiles = distance,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        var distanceInMiles = Convert.ToDecimal(distance);
        var distanceInKm = (distanceInMiles - distanceInMiles / 5) * 2;

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Contain($"geo.distance(Location, geography'POINT({lon} {lat})') le {distanceInKm}");
        result.Filter.Should().Contain("Location eq null");
        result.Filter.Should().Contain("not ((AvailableWhere eq 'MultipleLocations' or AvailableWhere eq 'OneLocation') and not (Address/Latitude ne null and Address/Latitude ne 0 and Address/Longitude ne null and Address/Longitude ne 0))");
    }

    [Test]
    public void Then_The_ExcludeNational_Filter_Is_Added()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = true,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and VacancyLocationType ne 'National'");
    }

    [Test, AutoData]
    public void Then_The_PostedInLastNumberOfDays_Filter_Is_Added(uint days)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = days,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Contain($"{AzureSearchConstants.VacancySourceEqualsRaa} and PostedDate ge");
    }

    [Test]
    public void Then_The_DisabilityConfident_Filter_Is_Added()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = true,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and IsDisabilityConfident eq true");
    }

    [Test]
    public void Then_The_SkipWageType_Filter_Is_Added_When_Sorted_By_Salary()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            VacancySort = VacancySort.SalaryAsc,
            SkipWageType = WageType.CompetitiveSalary,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = false
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and Wage/WageType ne 'CompetitiveSalary'");
    }

    [Test, AutoData]
    public void Then_The_ApprenticeshipTypes_Filter_Is_Added(List<ApprenticeshipTypes> types)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = types,
            OnlyPrimaryLocations = false
        };

        var typeFilters = string.Join(" or ", types.Select(x => $"ApprenticeshipType eq '{x}'"));

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and ({typeFilters})");
    }

    [Test]
    public void Then_The_OnlyPrimaryLocations_Filter_Is_Added()
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = null,
            Ukprn = null,
            AccountPublicHashedId = null,
            AccountLegalEntityPublicHashedId = null,
            EmployerName = null,
            StandardLarsCode = null,
            Categories = null,
            RouteIds = null,
            Levels = null,
            Lat = null,
            Lon = null,
            DistanceInMiles = null,
            ExcludeNational = null,
            PostedInLastNumberOfDays = null,
            DisabilityConfident = false,
            SkipWageType = null,
            ApprenticeshipTypes = null,
            OnlyPrimaryLocations = true
        };

        // Act
        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Be($"{AzureSearchConstants.VacancySourceEqualsRaa} and IsPrimaryLocation eq true");
    }

    [Test, AutoData]
    public void Then_The_Filter_Is_Correct_When_All_Filters_Are_Provided(
        List<DataSource> additionalDataSources,
        int ukprn,
        string accountPublicHashedId,
        string accountLegalEntityPublicHashedId,
        string employerName,
        List<int> standardLarsCodes,
        List<string> categories,
        List<int> routeIds,
        List<string> levels,
        double lat,
        double lon,
        uint distanceInMiles,
        uint postedInLastNumberOfDays,
        List<ApprenticeshipTypes> apprenticeshipTypes)
    {
        // Arrange
        var model = new FindVacanciesModel
        {
            AdditionalDataSources = additionalDataSources,
            Ukprn = ukprn,
            AccountPublicHashedId = accountPublicHashedId,
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId,
            EmployerName = employerName,
            StandardLarsCode = standardLarsCodes,
            Categories = categories,
            RouteIds = routeIds,
            Levels = levels,
            Lat = lat,
            Lon = lon,
            DistanceInMiles = distanceInMiles,
            ExcludeNational = true,
            PostedInLastNumberOfDays = postedInLastNumberOfDays,
            DisabilityConfident = true,
            VacancySort = VacancySort.SalaryAsc,
            SkipWageType = WageType.CompetitiveSalary,
            ApprenticeshipTypes = apprenticeshipTypes,
            OnlyPrimaryLocations = true
        };

        var sourceClauses = new List<string> { AzureSearchConstants.VacancySourceEqualsRaa };
        sourceClauses.AddRange(additionalDataSources.Select(source => $"VacancySource eq '{source.GetAzureSearchTerm()}'"));
        var sourcesFilter = $"({string.Join(" or ", sourceClauses)})";

        var result = new SearchOptions().BuildFilters(model);

        // Assert
        result.Filter.Should().Contain(sourcesFilter);
        result.Filter.Should().Contain($"Ukprn eq '{ukprn}'");
        result.Filter.Should().Contain($"AccountPublicHashedId eq '{accountPublicHashedId}'");
        result.Filter.Should().Contain($"AccountLegalEntityPublicHashedId eq '{accountLegalEntityPublicHashedId}'");
        result.Filter.Should().Contain($"EmployerName eq '{employerName}'");
        result.Filter.Should().Contain($"({string.Join(" or ", standardLarsCodes.Select(c => $"Course/LarsCode eq {c}"))})");
        result.Filter.Should().Contain($"({string.Join(" or ", categories.Select(c => $"Route eq '{c}'"))})");
        result.Filter.Should().Contain($"({string.Join(" or ", routeIds.Select(c => $"Course/RouteCode eq {c}"))})");
        result.Filter.Should().Contain($"({string.Join(" or ", levels.Select(c => $"Course/Level eq '{c}'"))})");
        result.Filter.Should().Contain($"(geo.distance(Location, geography'POINT({lon} {lat})') le {(Convert.ToDecimal(distanceInMiles) - Convert.ToDecimal(distanceInMiles) / 5) * 2} or Location eq null)");
        result.Filter.Should().Contain("not ((AvailableWhere eq 'MultipleLocations' or AvailableWhere eq 'OneLocation') and not (Address/Latitude ne null and Address/Latitude ne 0 and Address/Longitude ne null and Address/Longitude ne 0))");
        result.Filter.Should().Contain("VacancyLocationType ne 'National'");
        result.Filter.Should().Contain("PostedDate ge ");
        result.Filter.Should().Contain("IsDisabilityConfident eq true");
        result.Filter.Should().Contain("Wage/WageType ne 'CompetitiveSalary'");
        result.Filter.Should().Contain($"({string.Join(" or ", apprenticeshipTypes.Select(t => $"ApprenticeshipType eq '{t}'"))})");
        result.Filter.Should().Contain("IsPrimaryLocation eq true");
    }
}
