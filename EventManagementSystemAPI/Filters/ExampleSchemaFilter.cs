using EventManagementSystemAPI.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventManagementSystemAPI.Filters
{
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(EventDto))
            {
                schema.Example = new OpenApiObject()
                {
                    ["title"] = new OpenApiString("Book Signing Event"),
                    ["description"] = new OpenApiString("Meet your favorite author and get your book signed."),
                    ["date"] = new OpenApiString("2024-08-15"),
                    ["time"] = new OpenApiString("11:00:00"),
                    ["location"] = new OpenApiString("City Library, Main Hall")
                };
            }
        }
    }
}