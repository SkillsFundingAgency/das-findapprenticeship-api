using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Entities
{
    public class ApprenticeshipVacancyItem : ApprenticeshipSearchItem
    {
        public string LongDescription { get; set; }
        public string OutcomeDescription { get; set; }
        public string TrainingDescription { get; set; }
        public string ThingsToConsider { get; set; }
        public List<string> Skills { get; set; }
        public List<VacancyQualification> Qualifications { get; set; }

        public static ApprenticeshipVacancyItem FromApprenticeshipSearchItem(ApprenticeshipSearchItem source)
        {
            return new ApprenticeshipVacancyItem
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
                Category = source.Category,
                CategoryCode = source.CategoryCode,
                ClosingDate = source.ClosingDate,
                Course = source.Course,
                Description = source.Description,
                Distance = source.Distance,
                Duration = source.Duration,
                DurationUnit = source.DurationUnit,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerName = source.EmployerName,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                ExpectedDuration = source.ExpectedDuration,
                FrameworkLarsCode = source.FrameworkLarsCode,
                HoursPerWeek = source.HoursPerWeek,
                Id = source.Id,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                IsRecruitVacancy = source.IsRecruitVacancy,
                Location = source.Location,
                LongDescription = null,
                NumberOfPositions = source.NumberOfPositions,
                OtherAddresses = source.OtherAddresses,
                OutcomeDescription = null,
                PostedDate = source.PostedDate,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                ProviderName = source.ProviderName,
                Qualifications = [],
                Score = source.Score,
                SearchGeoPoint = source.SearchGeoPoint,
                Skills = [],
                StandardLarsCode = source.StandardLarsCode,
                StartDate = source.StartDate,
                SubCategory = source.SubCategory,
                SubCategoryCode = source.SubCategoryCode,
                ThingsToConsider = null,
                Title = source.Title,
                TrainingDescription = null,
                TypicalJobTitles = source.TypicalJobTitles,
                Ukprn = source.Ukprn,
                VacancyLocationType = source.VacancyLocationType,
                VacancyReference = source.VacancyReference,
                VacancySource = source.VacancySource,
                Wage = source.Wage,
                WageAmount = source.WageAmount,
                WageAmountLowerBound = source.WageAmountLowerBound,
                WageAmountUpperBound = source.WageAmountUpperBound,
                WageText = source.WageText,
                WageType = source.WageType,
                WageUnit = source.WageUnit,
                WorkingWeek = source.WorkingWeek,
            };
        }
    }
}