using AutoMapper;
using EventManagementSystemAPI.Filters;
using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.MappingProfile;
using EventManagementSystemAPI.Swagger;
using EventManagementSystemAPI.Util;
using EventMS.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Mime;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with SQL Server
var sqlConnection = builder.Configuration["ConnectionStrings:EMS:SqlDb"];

builder.Services.AddSqlServer<ApplicationDbContext>(sqlConnection, options => options.EnableRetryOnFailure());

builder.Services.AddRepositories();
builder.Services.AddUseCases();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<ExampleSchemaFilter>();
    c.EnableAnnotations();
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Validations
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new ValidationFailedResult(context.ModelState);

            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            result.ContentTypes.Add(MediaTypeNames.Application.Xml);

            return result;
        };
    });

//mapper 
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Authentication
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = "http://localhost:8080/realms/event-management-system/";
    o.Audience = "ems-api";

    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudiences = new[] { "master-realm", "account", "ems-api" }
    };

    o.Events = new JwtBearerEvents()
    {
        OnTokenValidated = context =>
        {
            var claims = context.Principal.Claims.ToList();

            var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;
            if (resourceAccess != null)
            {
                var resourceAccessJson = System.Text.Json.JsonDocument.Parse(resourceAccess);
                foreach (var resource in resourceAccessJson.RootElement.EnumerateObject())
                {
                    if (resource.Name == "ems-api")
                    {
                        foreach (var role in resource.Value.GetProperty("roles").EnumerateArray())
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.GetString()));
                        }
                    }
                }
            }

            var appIdentity = new ClaimsIdentity(claims);
            context.Principal.AddIdentity(appIdentity);

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(context.Exception.ToString());
        }
    };

    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminClientRole", policy => policy.RequireRole("admin_client_role"));
    options.AddPolicy("OrganizerClientRole", policy => policy.RequireRole("organizer_client_role"));
    options.AddPolicy("UserClientRole", policy => policy.RequireRole("user_client_role"));
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();