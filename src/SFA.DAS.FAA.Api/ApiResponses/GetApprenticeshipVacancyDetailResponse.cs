using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Extensions;
using Microsoft.Spatial;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.ApiResponses
{
    public class GetApprenticeshipVacancyDetailResponse : GetApprenticeshipVacancyResponse
    {
        public string LongDescription { get; set; }
        public string OutcomeDescription { get; set; }
        public string TrainingDescription { get; set; }
        public List<string> Skills { get; set; }
        public List<Qualification> Qualifications { get; set; }
        public string ThingsToConsider { get; set; }
        public string AdditionalQuestion1 { get; set; }
        public string AdditionalQuestion2 { get; set; }

        public static implicit operator GetApprenticeshipVacancyDetailResponse(ApprenticeshipVacancyItem source)
        {
            var duration = source.Duration == 0 ? source.Wage.Duration : source.Duration;
            var durationUnit = string.IsNullOrEmpty(source.DurationUnit) ? source.Wage?.WageUnit.GetDisplayName() : source.DurationUnit;

            var sourceLocation = source.Location.Lat == 0 && source.Location.Lon == 0 ? new GeoPoint{Lon = source.Address.Longitude, Lat = source.Address.Latitude} : source.Location;

            var distance = source.Distance ?? (source.SearchGeoPoint != null ? (decimal)GetDistanceBetweenPointsInMiles(sourceLocation.Lon, sourceLocation.Lat, source.SearchGeoPoint.Lon, source.SearchGeoPoint.Lat) : 0);
            
            return new GetApprenticeshipVacancyDetailResponse
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
                Location =  sourceLocation,
                NumberOfPositions = source.NumberOfPositions,
                PostedDate = source.PostedDate,
                ProviderName = source.ProviderName,
                StandardTitle = source.Course?.Title,
                StandardLarsCode = source.StandardLarsCode ?? source.Course?.LarsCode,
                StandardLevel = source.Course?.Level ?? "0",
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
                WageUnit = source.Wage != null ? 4 : source.WageUnit ?? 0,//Always annual for v2 TODO look at removing
                WageType = source.Wage != null ? (int)source.Wage.WageType : source.WageType ?? 0,
                WorkingWeek = source.WorkingWeek ?? source.Wage?.WorkingWeekDescription,
                Distance = source.Distance ?? (decimal)distance,
                Score = source.Score,
                LongDescription = source.LongDescription,
                OutcomeDescription = source.OutcomeDescription,
                TrainingDescription = source.TrainingDescription,
                ThingsToConsider = source.ThingsToConsider,
                Skills = source.Skills,
                Qualifications = source.Qualifications.Select(c=> (Qualification)c).ToList(),
                ExpectedDuration = !string.IsNullOrEmpty(source.ExpectedDuration) 
                    ? source.ExpectedDuration 
                    : $"{duration} {(duration == 1 || string.IsNullOrEmpty(durationUnit) || durationUnit.EndsWith("s") ? durationUnit : $"{durationUnit}s")}",
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmployerDescription = source.EmployerDescription,
                Address = source.Address,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationUrl = source.ApplicationUrl,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2
            };
        }

        
    }
    
    public class Qualification
    {
        public string Weighting { get ; set ; }
        public string QualificationType { get ; set ; }
        public string Subject { get ; set ; }
        public string Grade { get ; set ; }

        public static implicit operator Qualification(VacancyQualification source)
        {
            return new Qualification
            {
                Grade = source.Grade,
                Subject = source.Subject,
                Weighting = source.Weighting,
                QualificationType = source.QualificationType

            };
        }
    }
}