using System;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.ApiResponses
{
    public class GetApprenticeshipVacancyResponse
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
        public string Ukprn { get; set; }
        public string VacancyLocationType { get; set; }
        public string VacancyReference { get; set; }
        public decimal? WageAmount { get; set; }
        public decimal? WageAmountLowerBound { get; set; }
        public decimal? WageAmountUpperBound { get; set; }
        public string WageText { get; set; }
        public int WageUnit { get; set; }
        public int WageType { get; set; }
        public string WorkingWeek { get; set; }
        public string ExpectedDuration { get ; set ; }
        
        //Calculated after search
        public decimal? Distance { get; set; }
        public double Score { get; set; }
        public Address Address { get ; set ; }
        public string EmployerDescription { get ; set ; }
        public string EmployerWebsiteUrl { get ; set ; }
        public string EmployerContactPhone { get ; set ; }
        public string EmployerContactEmail { get ; set ; }
        public string EmployerContactName { get ; set ; }
        public int? RouteCode { get ; set ; }
        public string StandardTitle { get; set; }

        public static implicit operator GetApprenticeshipVacancyResponse(ApprenticeshipSearchItem source)
        {
            return new GetApprenticeshipVacancyResponse
            {
                Id = source.Id,
                AnonymousEmployerName = source.AnonymousEmployerName,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                Category = source.Category ?? source.Course?.Title,
                CategoryCode = source.CategoryCode ?? "SSAT1.UNKNOWN",
                ClosingDate = source.ClosingDate,
                Description = source.Description,
                EmployerName = source.EmployerName,
                FrameworkLarsCode = source.FrameworkLarsCode,
                HoursPerWeek = source.HoursPerWeek,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                IsRecruitVacancy = source.IsRecruitVacancy,
                Location =  source.Location.Lat == 0 && source.Location.Lon == 0 ? new GeoPoint{Lon = source.Address.Longitude, Lat = source.Address.Latitude} : source.Location,
                NumberOfPositions = source.NumberOfPositions,
                PostedDate = source.PostedDate,
                ProviderName = source.ProviderName,
                StandardTitle = source.Course?.Title,
                StandardLarsCode = source.StandardLarsCode ?? source.Course?.LarsCode,
                RouteCode = source.Course?.RouteCode,
                StartDate = source.StartDate,
                SubCategory = source.SubCategory?? source.Course?.Title,
                SubCategoryCode = source.SubCategoryCode?? source.Course?.Title,
                Title = source.Title,
                Ukprn = source.Ukprn,
                VacancyLocationType = source.VacancyLocationType,
                VacancyReference = source.VacancyReference,
                WageAmount = source.WageAmount,
                WageAmountLowerBound = source.WageAmountLowerBound,
                WageAmountUpperBound = source.WageAmountUpperBound,
                WageText = source.WageText,
                WageUnit = source.Wage != null ? (int)source.Wage.WageUnit : source.WageUnit,
                WageType = source.Wage != null ? (int)source.Wage.WageType : source.WageType,
                WorkingWeek = source.WorkingWeek ?? source.Wage.WorkingWeekDescription,
                Distance = source.Distance,
                Score = source.Score,
                ExpectedDuration = !string.IsNullOrEmpty(source.ExpectedDuration) 
                    ? source.ExpectedDuration 
                    : $"{source.Duration} {(source.Duration == 1 || string.IsNullOrEmpty(source.DurationUnit) || source.DurationUnit.EndsWith("s") ? source.DurationUnit : $"{source.DurationUnit}s")}",
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                Address = source.Address
            };
        }
    }
    
    public class GeoPoint
    {
        public double Lon { get; set; }
        public double Lat { get; set; }

        public static implicit operator GeoPoint(Domain.Entities.GeoPoint source)
        {
            return new GeoPoint
            {
                Lon = source.Lon,
                Lat = source.Lat
            };
        }
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
        public static implicit operator Address(Domain.Entities.Address source)
        {
            if (source == null)
            {
                return null;
            }
            return new Address
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
            };
        }
    }
}