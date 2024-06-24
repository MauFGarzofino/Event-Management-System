using EventMS.Infrastructure.Auth.TokenManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace EventManagementSystemAPI.Util
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Authority = configuration["Jwt:Authority"];
                    o.Audience = configuration["Jwt:Audience"];

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["Jwt:Authority"],
                        ValidAudiences = new[] { "master-realm", "account", "ems-api" },
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                        {
                            var client = new HttpClient();
                            var discoveryDocument = client.GetStringAsync($"{o.Authority}/.well-known/openid-configuration").Result;
                            var keys = new JsonWebKeySet(discoveryDocument).GetSigningKeys();
                            return keys;
                        }
                    };

                    o.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claims = context.Principal.Claims.ToList();
                            var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;

                            if (resourceAccess != null)
                            {
                                var resourceAccessJson = JsonDocument.Parse(resourceAccess);
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiPolicies.AdminClientRole, policy => policy.RequireRole("admin_client_role"));
                options.AddPolicy(ApiPolicies.OrganizerClientRole, policy => policy.RequireRole("organizer_client_role"));
                options.AddPolicy(ApiPolicies.UserClientRole, policy => policy.RequireRole("user_client_role"));
            });

            return services;
        }
    }
}
