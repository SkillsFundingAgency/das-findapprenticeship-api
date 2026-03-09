using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.Extensions;

public class NonNullListStringModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult != ValueProviderResult.None)
        {
            // Get the raw value from the query string
            var rawValue = valueProviderResult.ToList();

            var listString = rawValue.Where(item => !string.IsNullOrEmpty(item)).ToList();

            bindingContext.Model = listString;
            bindingContext.Result = ModelBindingResult.Success(listString);
        }

        return Task.CompletedTask;
    }
}