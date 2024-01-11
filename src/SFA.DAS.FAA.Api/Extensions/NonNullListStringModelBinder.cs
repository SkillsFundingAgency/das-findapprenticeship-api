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
                var listString = new List<string>();

                // Get the raw value from the query string
                var rawValue = valueProviderResult.ToList();

                foreach (var item in rawValue.Where(item => !string.IsNullOrEmpty(item)))
                {
                    listString.Add(item);
                }

                bindingContext.Model = listString;
                bindingContext.Result = ModelBindingResult.Success(listString);
            }

            return Task.CompletedTask;
        }
    }
}
