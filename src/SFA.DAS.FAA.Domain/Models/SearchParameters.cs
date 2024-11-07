using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<int>? SelectedRouteIds,
    decimal? Distance,
    bool DisabilityConfident,
    List<int>? SelectedLevelIds,
    string? Location,
    string? Latitude,
    string? Longitude
)
{
    public static SearchParameters From(string source)
    {
        return JsonConvert.DeserializeObject<SearchParameters>(source);
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
};
