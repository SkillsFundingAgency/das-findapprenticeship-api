using System;
using Azure.Search.Documents.Models;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class ApprenticeshipSearchItem
    {
        public string Id { get; set; }
        public string AnonymousEmployerName { get; set; }
        public string ApprenticeshipLevel { get; set; } 
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Description { get; set; }
        public string EmployerName { get; set; }
        public string FrameworkLarsCode { get; set; }
        public decimal? HoursPerWeek { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public bool IsRecruitVacancy { get; set; }
        public GeoPoint Location { get; set; }
        public int NumberOfPositions { get; set; }
        public DateTime PostedDate { get; set; }
        public string ProviderName { get; set; }
        public int? StandardLarsCode { get; set; }
        public DateTime StartDate { get; set; }
        public string SubCategory { get; set; }
        public string SubCategoryCode { get; set; }
        public string Title { get; set; }
        public long Ukprn { get; set; }
        public string VacancyLocationType { get; set; }
        public string VacancyReference { get; set; }
        public decimal? WageAmount { get; set; }
        public decimal? WageAmountLowerBound { get; set; }
        public decimal? WageAmountUpperBound { get; set; }
        public string WageText { get; set; }
        public int WageUnit { get; set; }
        public int WageType { get; set; }
        public string WorkingWeek { get; set; }
        public Address Address { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerContactName { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactEmail { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }
        public string ExpectedDuration { get; set; }
        //Calculated after search
        public decimal? Distance { get; set; }
        public double Score { get; set; }
        public WageSearchDocument Wage { get; set; }
        public CourseSearchDocument Course { get; set; }
    }
    
    
    
    public class GeoPoint
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
    
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    public class CourseSearchDocument
    {
        public int? LarsCode { get; set; }
        public string Title { get; set; }
        public int? Level { get; set; }
        public int? RouteCode { get; set; }
        public string Route { get; set; }
    }


    public class WageSearchDocument
    {
        public string WageAdditionalInformation { get; set; }
        public string WageType { get; set; }
        public string WorkingWeekDescription { get; set; }
        public string WageUnit { get; set; }
        public long? WageAmount { get; set; }
    }
}
