using System;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.Extensions
{
    public static class AdditionalDataSourceExtensions
    {
        // Maps DataSource enum values to their corresponding Azure Search terms.
        public static string GetAzureSearchTerm(this DataSource source)
        {
            return source switch
            {
                DataSource.Raa => "RAA", // Note: 'RAA' is the correct casing for Azure Search. FAI-2998
                DataSource.Nhs => "Nhs", // Note: 'Nhs' is the correct casing for Azure Search. FAI-2998
                _ => throw new InvalidOperationException("Unable to map data source to azure search term")
            };
        }
    }
}
