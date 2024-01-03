﻿using System;
using Microsoft.OpenApi.Extensions;
using Microsoft.Spatial;
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
        public int? StandardLevel { get; set; }
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
        public string ApplicationMethod { get; set; }
        public string ApplicationUrl { get; set; }

        public static implicit operator GetApprenticeshipVacancyResponse(ApprenticeshipSearchItem source)
        {
            var duration = source.Duration == 0 ? source.Wage.Duration : source.Duration;
            var durationUnit = string.IsNullOrEmpty(source.DurationUnit) ? source.Wage?.WageUnit.GetDisplayName() : source.DurationUnit;

            var sourceLocation = source.Location.Lat == 0 && source.Location.Lon == 0 ? new GeoPoint{Lon = source.Address.Longitude, Lat = source.Address.Latitude} : source.Location;

            var distance = source.Distance ?? (source.SearchGeoPoint != null ? (decimal)GetDistanceBetweenPointsInMiles(sourceLocation.Lon, sourceLocation.Lat, source.SearchGeoPoint.Lon, source.SearchGeoPoint.Lat) : 0);
            
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
                StandardLevel = source.Course?.Level,
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
                WageUnit = source.Wage != null ? 4 : source.WageUnit,//Always annual for v2 TODO look at removing
                WageType = source.Wage != null ? (int)source.Wage.WageType : source.WageType,
                WorkingWeek = source.WorkingWeek ?? source.Wage.WorkingWeekDescription,
                Distance = source.Distance ?? (decimal)distance,
                Score = source.Score,
                ExpectedDuration = !string.IsNullOrEmpty(source.ExpectedDuration) 
                    ? source.ExpectedDuration 
                    : $"{duration} {(duration == 1 || string.IsNullOrEmpty(durationUnit) || durationUnit.EndsWith("s") ? durationUnit : $"{durationUnit}s")}",
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                Address = source.Address,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationUrl = source.ApplicationUrl
            };
        }

        internal static double GetDistanceBetweenPointsInMiles(
            double lon1,
            double lat1,
            double lon2,
            double lat2)
        {
            const double radiusOfEarth = 6378.1;
            var longitudeDelta =  CalculateRadians(lon2 - lon1);
            var latitudeDelta =  CalculateRadians(lat2 - lat1);

            var a = (Math.Sin(latitudeDelta / 2) * Math.Sin(latitudeDelta / 2)) + Math.Cos(CalculateRadians(lat1)) * Math.Cos(CalculateRadians(lat2)) * (Math.Sin(longitudeDelta / 2) * Math.Sin(longitudeDelta / 2));
            var angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (angle * radiusOfEarth) * 0.6213711922;//distance in miles
        }
        private static double CalculateRadians(double x)
        {
            return x * Math.PI / 180;
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