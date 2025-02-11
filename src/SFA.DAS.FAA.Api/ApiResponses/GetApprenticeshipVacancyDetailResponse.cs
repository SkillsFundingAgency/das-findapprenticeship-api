using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Extensions;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;

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
        public string WageAdditionalInformation { get; set; }
        public string ApplicationInstructions { get; set; }

        public static implicit operator GetApprenticeshipVacancyDetailResponse(ApprenticeshipVacancyItem source)
        {
            var sourceLocation = source.Location.Lat == 0 && source.Location.Lon == 0 ? new GeoPoint{Lon = source.Address.Longitude, Lat = source.Address.Latitude} : source.Location;
            var distance = source.Distance ?? (source.SearchGeoPoint != null ? (decimal)GetDistanceBetweenPointsInMiles(sourceLocation.Lon, sourceLocation.Lat, source.SearchGeoPoint.Lon, source.SearchGeoPoint.Lat) : 0);
            
            return new GetApprenticeshipVacancyDetailResponse
            {
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                Address = source.Address,
                AnonymousEmployerName = source.AnonymousEmployerName,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationUrl = source.ApplicationUrl,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                Category = source.Category ?? source.Course?.Title,
                CategoryCode = source.CategoryCode ?? "SSAT1.UNKNOWN",
                ClosingDate = source.ClosingDate,
                CompanyBenefitsInformation = source.Wage?.CompanyBenefitsInformation,
                Description = source.Description,
                Distance = distance,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerName = source.EmployerName,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                ExpectedDuration = source.VacancySource.Equals(DataSource.Nhs.ToString(), StringComparison.CurrentCultureIgnoreCase) ? string.Empty : GetDuration(source),
                FrameworkLarsCode = source.FrameworkLarsCode,
                HoursPerWeek = source.HoursPerWeek,
                Id = source.Id,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                IsPrimaryLocation = source.IsPrimaryLocation,
                IsRecruitVacancy = source.IsRecruitVacancy,
                Location =  sourceLocation,
                LongDescription = source.LongDescription,
                NumberOfPositions = source.NumberOfPositions,
                OtherAddresses = source.OtherAddresses?.Select(add => (Address)add).ToList(),
                OutcomeDescription = source.OutcomeDescription,
                PostedDate = source.PostedDate,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                ProviderName = source.ProviderName,
                Qualifications = source.Qualifications.Select(c => (Qualification)c).ToList(),
                RouteCode = source.Course?.RouteCode,
                Score = source.Score,
                Skills = source.Skills,
                StandardLarsCode = source.StandardLarsCode ?? source.Course?.LarsCode,
                StandardLevel = source.Course?.Level ?? "0",
                StandardTitle = source.Course?.Title,
                StartDate = source.StartDate,
                SubCategory = source.SubCategory?? source.Course?.Title,
                SubCategoryCode = source.SubCategoryCode?? source.Course?.Title,
                ThingsToConsider = source.ThingsToConsider,
                Title = source.Title,
                TrainingDescription = source.TrainingDescription,
                Ukprn = source.Ukprn,
                VacancyLocationType = source.VacancyLocationType,
                VacancyReference = source.VacancyReference,
                VacancySource = source.VacancySource,
                WageAdditionalInformation = source.Wage != null ? source.Wage.WageAdditionalInformation : string.Empty,
                WageAmount = source.WageAmount,
                WageAmountLowerBound = source.WageAmountLowerBound,
                WageAmountUpperBound = source.WageAmountUpperBound,
                WageText = source.WageText,
                WageType = source.Wage != null && source.Wage.WageType.HasValue ? (int)source.Wage.WageType : source.WageType ?? 0,
                WageUnit = source.Wage != null ? 4 : source.WageUnit ?? 0,//Always annual for v2 TODO look at removing
                WorkingWeek = source.WorkingWeek ?? source.Wage?.WorkingWeekDescription,
            };
        }

        

        private static string GetDuration(ApprenticeshipVacancyItem source)
        {
            if (!string.IsNullOrEmpty(source.ExpectedDuration))
            {
                return source.ExpectedDuration;
            }

            var duration = source.Duration == 0 ? source.Wage.Duration : source.Duration;

            var durationUnit = string.IsNullOrEmpty(source.DurationUnit) ? source.Wage?.WageUnit.GetDisplayName() : source.DurationUnit;

            if (durationUnit == Domain.Models.WageUnit.Month.GetDisplayName())
            {
                var wholeYears = duration / 12;
                var remainingMonths = duration % 12;

                switch (wholeYears)
                {
                    case 0:
                        return remainingMonths == 1 ? "1 Month" : $"{remainingMonths} Months";
                    case 1 when remainingMonths == 0:
                        return "1 Year";
                    case 1:
                    {
                        var months = remainingMonths == 1 ? "1 Month" : $"{remainingMonths} Months";
                        return $"1 Year {months}";
                    }
                    default:
                        return remainingMonths == 0 ? $"{wholeYears} Years" : $"{wholeYears} Years {remainingMonths} Months";
                }
            }

            return $"{duration} {(duration == 1 || string.IsNullOrEmpty(durationUnit) || durationUnit.EndsWith("s") ? durationUnit : $"{durationUnit}s")}";
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