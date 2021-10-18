using System;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.ApiResponses
{
    public class GetApprenticeshipVacancyResponse
    {
        public int Id { get; set; }
        public string AnonymousEmployerName { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
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
        public VacancyLocationType VacancyLocationType { get; set; }
        public string VacancyReference { get; set; }
        public decimal? WageAmount { get; set; }
        public decimal? WageAmountLowerBound { get; set; }
        public decimal? WageAmountUpperBound { get; set; }
        public string WageText { get; set; }
        public int WageUnit { get; set; }
        public int WageType { get; set; }
        public string WorkingWeek { get; set; }
        
        //Calculated after search
        public double Distance { get; set; }
        public double Score { get; set; }

        public static implicit operator GetApprenticeshipVacancyResponse(ApprenticeshipSearchItem source)
        {
            return new GetApprenticeshipVacancyResponse
            {
                Id = source.Id,
                AnonymousEmployerName = source.AnonymousEmployerName,
                ApprenticeshipLevel = (ApprenticeshipLevel) source.ApprenticeshipLevel,
                Category = source.Category,
                CategoryCode = source.CategoryCode,
                ClosingDate = source.ClosingDate,
                Description = source.Description,
                EmployerName = source.EmployerName,
                FrameworkLarsCode = source.FrameworkLarsCode,
                HoursPerWeek = source.HoursPerWeek,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                IsRecruitVacancy = source.IsRecruitVacancy,
                Location = source.Location,
                NumberOfPositions = source.NumberOfPositions,
                PostedDate = source.PostedDate,
                ProviderName = source.ProviderName,
                StandardLarsCode = source.StandardLarsCode,
                StartDate = source.StartDate,
                SubCategory = source.SubCategory,
                SubCategoryCode = source.SubCategoryCode,
                Title = source.Title,
                Ukprn = source.Ukprn,
                VacancyLocationType = (VacancyLocationType) source.VacancyLocationType,
                VacancyReference = source.VacancyReference,
                WageAmount = source.WageAmount,
                WageAmountLowerBound = source.WageAmountLowerBound,
                WageAmountUpperBound = source.WageAmountUpperBound,
                WageText = source.WageText,
                WageUnit = source.WageUnit,
                WageType = source.WageType,
                WorkingWeek = source.WorkingWeek,
                Distance = source.Distance,
                Score = source.Score
            };
        }
    }
    
    public class GeoPoint
    {
        public double lon { get; set; }
        public double lat { get; set; }

        public static implicit operator GeoPoint(SFA.DAS.FAA.Domain.Entities.GeoPoint source)
        {
            return new GeoPoint
            {
                lon = source.lon,
                lat = source.lat
            };
        }
    }
    
    public enum ApprenticeshipLevel
    {
        Unknown = 0,
        Intermediate,
        Advanced,
        Higher,
        Degree,
        Foundation,
        Masters
    }
    
    public enum VacancyLocationType
    {
        Unknown = 0,
        NonNational,
        National
    }
}