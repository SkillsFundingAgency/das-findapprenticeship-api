﻿using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApiRequests;

public record SaveSearchRequest(string UnsubscribeToken, SearchParameters SearchParameters);