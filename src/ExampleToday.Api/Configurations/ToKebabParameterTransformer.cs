using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.RegularExpressions;

namespace ExampleToday.Api.Configurations
{
    public class ToKebabParameterTransformer : IOutboundParameterTransformer
    {
        public static RouteTokenTransformerConvention KebabTransform => new(new ToKebabParameterTransformer());

        public string TransformOutbound(object? value)
        {
            var path = value?.ToString();
            if (string.IsNullOrEmpty(path))
            {
                return "";
            }

            return Regex.Replace(path, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}