using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Extensions;
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


        public static implicit operator GetApprenticeshipVacancyDetailResponse(ApprenticeshipVacancyItem source)
        {
            var duration = source.Duration == 0 ? source.Wage.Duration : source.Duration;
            var durationUnit = string.IsNullOrEmpty(source.DurationUnit) ? source.Wage?.WageUnit.GetDisplayName() : source.DurationUnit;
            
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
                WorkingWeek = source.WorkingWeek ?? source.Wage?.WorkingWeekDescription,
                Distance = source.Distance,
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
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmployerDescription = source.EmployerDescription,
                Address = source.Address
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