using System;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Data.Extensions
{
    public static class AdditionalDataSourceExtensions
    {
        public static string GetAzureSearchTerm(this AdditionalDataSource source)
        {
            return source switch
            {
                AdditionalDataSource.Nhs => "NHS",
                _ => throw new InvalidOperationException("Unable to map data source to azure search term")
            };
        }
    }
}
