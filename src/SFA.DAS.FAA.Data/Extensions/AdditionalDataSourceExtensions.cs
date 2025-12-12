using System;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.Extensions
{
    public static class AdditionalDataSourceExtensions
    {
        public static string GetAzureSearchTerm(this DataSource source)
        {
            return source switch
            {
                DataSource.Raa => "RAA",
                DataSource.Nhs => "NHS",
                DataSource.Csj => "Csj",
                _ => throw new InvalidOperationException("Unable to map data source to azure search term")
            };
        }
    }
}
