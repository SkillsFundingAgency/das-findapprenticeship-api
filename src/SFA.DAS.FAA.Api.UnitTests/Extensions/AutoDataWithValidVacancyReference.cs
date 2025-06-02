using AutoFixture;
using AutoFixture.AutoMoq;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Api.UnitTests.Extensions
{
    internal class AutoDataWithValidVacancyReference() : AutoDataAttribute(() =>
    {
        var fixture = new Fixture();
        fixture.Customize(new ValidVacancyReferenceCustomization());
        fixture.Customize(new AutoMoqCustomization());
        return fixture;
    });

    public class ValidVacancyReferenceCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new VacancyReference("VAC12345"));
        }
    }
}
