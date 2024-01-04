using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Api.Extensions;

namespace SFA.DAS.FAA.Api.UnitTests.Extensions
{
    [TestFixture]
    public class NonNullListStringModelBinderTests
    {
        [Test]
        [InlineAutoData("value1")]
        public void BindModelAsync_ValidQueryString_ReturnsNonNullableList(string query)
        {
            // Arrange
            var modelBinder = new NonNullListStringModelBinder();
            var bindingContext = GetBindingContext(query);

            // Act
            modelBinder.BindModelAsync(bindingContext);

            // Assert
            var result = bindingContext.Result.Model as List<string>;

            result.Count.Should().Be(1);
            result.Should().Contain("value1");
        }

        [Test]        
        public void BindModelAsync_NullQueryString_ReturnsEmptyList()
        {
            // Arrange
            var modelBinder = new NonNullListStringModelBinder();
            var bindingContext = GetBindingContext(null);

            // Act
            modelBinder.BindModelAsync(bindingContext);

            // Assert
            var result = bindingContext.Result.Model as List<string>;
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void BindModelAsync_EmptyQueryString_ReturnsEmptyList()
        {
            // Arrange
            var modelBinder = new NonNullListStringModelBinder();
            var bindingContext = GetBindingContext("");

            // Act
            modelBinder.BindModelAsync(bindingContext);

            // Assert
            var result = bindingContext.Result.Model as List<string>;
            result.Should().BeNullOrEmpty();
        }

        private static ModelBindingContext GetBindingContext(string queryString)
        {
            var valueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(vp => vp.GetValue(It.IsAny<string>()))
                         .Returns(new ValueProviderResult(queryString));

            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "yourParam",
                ValueProvider = valueProvider.Object,
            };

            return bindingContext;
        }
    }
}
