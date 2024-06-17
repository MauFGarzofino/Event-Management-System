using AutoMapper;
using EventManagementSystemAPI.Filters;
using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.MappingProfile;
using EventManagementSystemAPI.Util;
using EventMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



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