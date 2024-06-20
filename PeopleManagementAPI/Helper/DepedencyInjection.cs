using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using PeopleManagement_BI;
using PeopleManagement_Utility;

namespace PeopleManagementAPI;

public static class DepedencyInjection
{
    public static IServiceCollection CustomInjection(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddSingleton<ISqlDataAccess, SqlDataAccess>();
        services.TryAddSingleton<IUserRepository, UserRepository>();
        services.TryAddSingleton<IPersonRepository, PersonRepository>();
        services.TryAddSingleton<IAccountsRepository, AccountsRepository>();
        services.TryAddSingleton<ITransactionsRespository, TransactionsRespository>();
        return services;
    }

    public static IServiceCollection CustomSwaggerUI(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "People Management API", Version = "v1" });

            // To Enable authorization using Swagger (JWT)  
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                });

        });
        return services;

    }
}
