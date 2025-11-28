using System.Collections.Generic;

namespace SFA.DAS.FAA.Data.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceParameters(this string source, Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                source = source.Replace($"{{{parameter.Key}}}", parameter.Value.ToString());
            }
            return source;
        }

        public static string SearchIn(this List<string> values, string fieldName)
        {
            return $"search.in({fieldName}, '{string.Join(',', values)}')";
        }
    }
}