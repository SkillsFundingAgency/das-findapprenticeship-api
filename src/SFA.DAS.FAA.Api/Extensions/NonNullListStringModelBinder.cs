using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Api.Extensions
{
    public class NonNullListStringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                // Get the raw value from the query string
                var rawValue = valueProviderResult.FirstValue;

                // Split the raw value into individual items, removing null or empty items
                var list = rawValue?.Split(',').Where(item => !string.IsNullOrEmpty(item)).ToList() ?? new List<string>();

                bindingContext.Model = list;
                bindingContext.Result = ModelBindingResult.Success(list);
            }

            return Task.CompletedTask;
        }
    }
}
