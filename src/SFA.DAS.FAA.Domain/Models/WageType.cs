using System.ComponentModel;

namespace SFA.DAS.FAA.Domain.Models;

public enum WageType
{
    [Description("Unknown")]
    Unknown = 1,
    [Description("National Minimum Wage For Apprentices")]
    NationalMinimumWageForApprentices = 2,
    [Description("National Minimum Wage")]
    NationalMinimumWage = 3,
    [Description("Fixed Wage")]
    FixedWage = 4,
    [Description("Competitive Salary")]
    CompetitiveSalary = 6
}