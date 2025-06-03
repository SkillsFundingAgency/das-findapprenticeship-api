using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    int? Distance,
    bool DisabilityConfident,
    List<int>? SelectedLevelIds,
    string? Location,
    string? Latitude,
    string? Longitude,
    bool ExcludeNational)
{
    public static SearchParameters From(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;
        try
        {
            return JsonConvert.DeserializeObject<SearchParameters>(source);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
};
