using EventMS.Application.DTOs;
using EventMS.Application.DTOs.Tickets;
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
                    ["title"] = new OpenApiString("Jazz Night"),
                    ["description"] = new OpenApiString("Experience a night of smooth jazz music with renowned musicians."),
                    ["date"] = new OpenApiString("2024-11-12"),
                    ["time"] = new OpenApiString("19:30:00"),
                    ["location"] = new OpenApiString("Jazz Club, Main Street")
                };
            }
            if (context.Type == typeof(UpdateEventDto))
            {
                schema.Example = new OpenApiObject()
                {
                    ["title"] = new OpenApiString("Robotics Workshop"),
                    ["description"] = new OpenApiString("Learn the basics of robotics and build your own robot in this hands-on workshop."),
                    ["date"] = new OpenApiString("2024-08-22"),
                    ["time"] = new OpenApiString("09:00:00"),
                    ["location"] = new OpenApiString("Tech Innovation Lab")
                };
            }
            if (context.Type == typeof(TypeTicketDto))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Name"] = new OpenApiString("VIP Ticket"),
                    ["Description"] = new OpenApiString("Includes access to all workshop sessions and materials."),
                    ["Price"] = new OpenApiDouble(150.00),
                    ["QuantityAvailable"] = new OpenApiInteger(50),
                    ["EventId"] = new OpenApiInteger(1)
                };
            }
            if (context.Type == typeof(EventRegistrationDto))
            {
                schema.Example = new OpenApiObject()
                {
                    ["UserId"] = new OpenApiString("c52b5ff3-8a08-4d6f-aaed-e967472e2388"),
                    ["EventId"] = new OpenApiInteger(1)
                };
            }
        }
    }
}
