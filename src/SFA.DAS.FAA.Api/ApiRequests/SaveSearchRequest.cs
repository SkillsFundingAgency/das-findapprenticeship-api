﻿using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.ApiRequests;

public record SaveSearchRequest(
    Guid UserReference,
    SearchParameters SearchParameters
);