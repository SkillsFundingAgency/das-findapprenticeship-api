using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Models;

public record SearchParameters(
    string? SearchTerm,
    List<string>? Categories,
    int? Distance,
    bool DisabilityConfident,
    List<string>? Levels,
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
