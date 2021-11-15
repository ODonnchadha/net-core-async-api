namespace NetCoreAsyncApi.Books.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Ensure *only* IEnumerable types.
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            // Obtain input via value provider.
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            // Obtain the type of the enumerable
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            // Obtain a converter of the enumerable
            var converter = TypeDescriptor.GetConverter(elementType);

            // Convert each item within the value list to the enumerable type.
            var values = value.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(v => converter.ConvertFromString(v.Trim())).ToArray();

            // Create an array of that type. And set it as the model value,
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;

            // Return a successful result, passing in the model.
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
