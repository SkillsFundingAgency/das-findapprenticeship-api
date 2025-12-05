using SFA.DAS.FAA.Api.Extensions;
using SFA.DAS.FAA.Domain.Entities;
using SFA.DAS.FAA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FAA.Api.ApiResponses
{
    public class GetApprenticeshipVacancyDetailResponse : GetApprenticeshipVacancyResponse
    {
        public long? AccountId { get; set; }
        public long? AccountLegalEntityId { get; set; }
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

        public static GetApprenticeshipVacancyDetailResponse From(ApprenticeshipVacancyItem source)
        {
            var sourceLocation = source.Location is { Lat: 0, Lon: 0 } ? new GeoPoint{Lon = source.Address.Longitude, Lat = source.Address.Latitude} : source.Location;

            decimal? distance = null;
            if (sourceLocation is not null)
            {
                distance = source.Distance ?? (source.SearchGeoPoint != null ? (decimal)GetDistanceBetweenPointsInMiles(sourceLocation.Lon, sourceLocation.Lat, source.SearchGeoPoint.Lon, source.SearchGeoPoint.Lat) : 0);
            }

            return new GetApprenticeshipVacancyDetailResponse
            {
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
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
                ExpectedDuration = Enum.TryParse<DataSource>(source.VacancySource, true, out var src) && src == DataSource.Raa
                    ? WageExtensions.GetDuration(source)
                    : string.Empty,
                FrameworkLarsCode = source.FrameworkLarsCode,
                HoursPerWeek = source.HoursPerWeek,
                Id = source.Id,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                IsPrimaryLocation = source.IsPrimaryLocation,
                AvailableWhere = source.AvailableWhere,
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
                Qualifications = source.Qualifications?.Select(c => (Qualification)c).ToList(),
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
                ApprenticeMinimumWage = source.Wage?.ApprenticeMinimumWage,
                Under18NationalMinimumWage = source.Wage?.Under18NationalMinimumWage,
                Between18AndUnder21NationalMinimumWage = source.Wage?.Between18AndUnder21NationalMinimumWage,
                Between21AndUnder25NationalMinimumWage = source.Wage?.Between21AndUnder25NationalMinimumWage,
                Over25NationalMinimumWage = source.Wage?.Over25NationalMinimumWage,
                WageText = source.WageText,
                WageType = source.Wage is { WageType: not null } ? (int)source.Wage.WageType : source.WageType ?? 0,
                WageUnit = source.Wage != null ? 4 : source.WageUnit ?? 0,//Always annual for v2 TODO look at removing
                WorkingWeek = source.WorkingWeek ?? source.Wage?.WorkingWeekDescription,
                ApprenticeshipType = source.ApprenticeshipType ?? ApprenticeshipTypes.Standard,
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
            if (source is null) return null;

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