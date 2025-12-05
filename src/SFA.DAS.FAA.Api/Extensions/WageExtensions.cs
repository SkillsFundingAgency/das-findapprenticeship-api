using Microsoft.OpenApi.Extensions;
using SFA.DAS.FAA.Domain.Entities;
using System;

namespace SFA.DAS.FAA.Api.Extensions;

public static class WageExtensions
{
    public static string GetDuration(ApprenticeshipSearchItem source)
    {
        if (!string.IsNullOrWhiteSpace(source.ExpectedDuration))
        {
            return source.ExpectedDuration;
        }

        var duration = source.Duration != 0 ? source.Duration : source.Wage?.Duration ?? 0;

        var durationUnit = !string.IsNullOrWhiteSpace(source.DurationUnit)
            ? source.DurationUnit
            : source.Wage?.WageUnit.GetDisplayName();

        if (string.IsNullOrWhiteSpace(durationUnit))
        {
            return duration.ToString();
        }

        if (durationUnit.Equals(Domain.Models.WageUnit.Month.GetDisplayName(), StringComparison.OrdinalIgnoreCase))
        {
            var years = duration / 12;
            var months = duration % 12;

            switch (years)
            {
                case 0:
                    return months == 1 ? "1 Month" : $"{months} Months";
                case 1 when months == 0:
                    return "1 Year";
                case 1:
                {
                    var monthPart = months == 1 ? "1 Month" : $"{months} Months";
                    return $"1 Year {monthPart}";
                }
                default:
                    return months == 0
                        ? $"{years} Years"
                        : $"{years} Years {months} Months";
            }
        }

        var pluralUnit = duration == 1
            ? durationUnit.TrimEnd('s')
            : durationUnit.EndsWith("s", StringComparison.OrdinalIgnoreCase)
                ? durationUnit
                : $"{durationUnit}s";

        return $"{duration} {pluralUnit}";
    }
}