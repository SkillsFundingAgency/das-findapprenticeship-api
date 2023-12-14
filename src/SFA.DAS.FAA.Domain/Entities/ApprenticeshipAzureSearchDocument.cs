using System;
using System.Collections.Generic;
using Microsoft.Spatial;

public class ApprenticeAzureSearchDocument
{
    public string? Description { get; set; }

    public string Route { get; set; } = null!;

    public string? EmployerName { get; set; }

    public long HoursPerWeek { get; set; }

    public string? ProviderName { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime ClosingDate { get; set; }

    
    public long NumberOfPositions { get; set; }

    public string? Title { get; set; }

    public long? Ukprn { get; set; }

    public string VacancyReference { get; set; } = null!;

    public CourseAzureSearchDocument? Course { get; set; }

    public AddressAzureSearchDocument? Address { get; set; }

    public WageAzureSearchDocument? Wage { get; set; }
    
    public string LongDescription { get; set; }

    public string OutcomeDescription { get; set; }

    public string TrainingDescription { get; set; }

    public List<string> Skills { get; set; }

    public List<QualificationAzureSearchDocument> Qualifications { get; set; }

    public string ThingsToConsider { get; set; }

    public string Id { get; set; }

    public string AnonymousEmployerName { get; set; }

    public bool IsDisabilityConfident { get; set; }

    public bool IsPositiveAboutDisability { get; set; }

    public bool IsEmployerAnonymous { get; set; }

    public bool IsRecruitVacancy { get; set; }

    public string VacancyLocationType { get; set; }
    
    public string EmployerDescription { get; set; }

    public string EmployerWebsiteUrl { get; set; }

    public string EmployerContactPhone { get; set; }

    public string EmployerContactEmail { get; set; }

    public string EmployerContactName { get; set; }
}

public class CourseAzureSearchDocument
{
    
    public int? LarsCode { get; set; }

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

public class QualificationAzureSearchDocument
{
   
    public string? QualificationType { get; set; }

    public string? Subject { get; set; }

    public string? Grade { get; set; }
    
    public string? Weighting { get; set; }
}