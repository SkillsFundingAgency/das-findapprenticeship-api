using System.Collections.Generic;
using SFA.DAS.FAA.Data.Extensions;

namespace SFA.DAS.FAA.Data.UnitTests.ExtensionTests;

public class WhenSearchingIn
{
    [Test, MoqAutoData]
    public void Then_The_Filter_Should_Be_Built_Correctly(List<string> values, string fieldName)
    {
        // act
        var result = values.SearchIn(fieldName);

        // assert
        result.Should().Be($"search.in({fieldName}, '{string.Join(',', values)}')");
    }
}