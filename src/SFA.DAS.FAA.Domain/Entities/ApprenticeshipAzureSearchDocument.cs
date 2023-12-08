using Microsoft.Spatial;
using System;

namespace SFA.DAS.FAA.Domain.Entities;
public class ApprenticeshipAzureSearchDocument
{
    public string? Description { get; set; }
    public string Route { get; set; } = null!;
    public string? EmployerName { get; set; }
    public long HoursPerWeek { get; set; }
    public string? ProviderName { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset PostedDate { get; set; }
    public DateTimeOffset ClosingDate { get; set; }
    public long NumberOfPositions { get; set; }
    public string? Title { get; set; }
    public long? Ukprn { get; set; }
    public string VacancyReference { get; set; } = null!;
    public CourseAzureSearchDocument? Course { get; set; }
    public AddressAzureSearchDocument? Address { get; set; }
    public WageAzureSearchDocument? Wage { get; set; }
    public GeographyPoint? Location { get; set; }
}

public class CourseAzureSearchDocument
{
    public long LarsCode { get; set; }
    public string? Title { get; set; }
    public long Level { get; set; }
}

public class AddressAzureSearchDocument
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? Postcode { get; set; }
}

public class WageAzureSearchDocument
{
    public string? WageAdditionalInformation { get; set; }
    public string? WageType { get; set; }
    public string? WorkingWeekDescription { get; set; }
    public string? WageUnit { get; set; }
    public long? WageAmount { get; set; }
}
