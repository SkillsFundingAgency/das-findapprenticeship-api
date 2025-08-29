using System.Collections.Generic;
using System.Text.Json;
using SFA.DAS.FAA.Domain.Entities;

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
    bool ExcludeNational,
    List<ApprenticeshipTypes>? SelectedApprenticeshipTypes)
{
    public static SearchParameters From(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;
        try
        {
            return JsonSerializer.Deserialize<SearchParameters>(source);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
};
