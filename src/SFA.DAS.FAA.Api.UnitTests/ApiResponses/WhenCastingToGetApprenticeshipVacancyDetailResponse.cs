using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Api.ApiResponses;
using SFA.DAS.FAA.Domain.Entities;

namespace SFA.DAS.FAA.Api.UnitTests.ApiResponses
{
    public class WhenCastingToGetApprenticeshipVacancyDetailResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ApprenticeshipVacancyItem source)
        {
            var actual = (GetApprenticeshipVacancyDetailResponse)source;

            actual.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.Duration)
                .Excluding(c=>c.DurationUnit)
            );
        }
    }
}