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
                WageType = source.WageType,
                WorkingWeek = source.WorkingWeek,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                Address = source.Address,
                AnonymousEmployerName = source.AnonymousEmployerName,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationMethod = source.ApplicationMethod,
                VacancySource = source.VacancySource,
                LongDescription = null,
                OutcomeDescription = null,
                TrainingDescription = null,
                ThingsToConsider = null,
                Skills = [],
                Qualifications = [],
                Description = source.Description,
                ApplicationUrl = source.ApplicationUrl,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                Category = source.Category,
                ClosingDate = source.ClosingDate,
                CategoryCode = source.CategoryCode,
                Course = source.Course,
                SearchGeoPoint = source.SearchGeoPoint,
                Distance = source.Distance,
                Score = source.Score,
                Duration = source.Duration,
                DurationUnit = source.DurationUnit,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerName = source.EmployerName,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                ExpectedDuration = source.ExpectedDuration,
                TypicalJobTitles = source.TypicalJobTitles,
                FrameworkLarsCode = source.FrameworkLarsCode,
                HoursPerWeek = source.HoursPerWeek,
                Id = source.Id,
                Wage = source.Wage,
                VacancyReference = source.VacancyReference,
                WageAmount = source.WageAmount,
                WageAmountLowerBound = source.WageAmountLowerBound,
                WageAmountUpperBound = source.WageAmountUpperBound,
                WageText = source.WageText,
                WageUnit = source.WageUnit,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                IsRecruitVacancy = source.IsRecruitVacancy,
                Location = source.Location,
                NumberOfPositions = source.NumberOfPositions,
                PostedDate = source.PostedDate,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                ProviderName = source.ProviderName,
                StandardLarsCode = source.StandardLarsCode,
                StartDate = source.StartDate,
                SubCategory = source.SubCategory,
                SubCategoryCode = source.SubCategoryCode,
                Title = source.Title,
                Ukprn = source.Ukprn,
                VacancyLocationType = source.VacancyLocationType,
            };
        }
    }
}