using Asp.Versioning;
using Microsoft.OpenApi;

namespace DevSu.Bank.Customers.Presentation.Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        private const string DevelopmentServerUrl = "http://localhost";

        public static IServiceCollection AddPresentationApiServices(this IServiceCollection services)
        {
            services.AddHealthChecks();
            services
                .AddVersioning()
                .AddOpenApiDocumentation();

            return services;
        }

        private static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            var apiVersioningBuilder = services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        private static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi("v1", options =>
            {
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "DevSu.Bank.Customers.Presentation.Api"
                    };

                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes.Add("bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        Description = @"JWT Authorization header using the bearer scheme. 
                                        Enter 'bearer' [space] and then your token in the text input below.
                                        Example: 'bearer eyJhbGci'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "bearer"
                    });

                    document.Security =
                    [
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecuritySchemeReference("bearer"), []
                            }
                        }
                    ];

                    document.SetReferenceHostDocument();

                    return Task.CompletedTask;
                });

                options.AddDocumentTransformer((document, _, _) =>
                {
                    if (document.Servers is null || document.Servers.Count == 0)
                    {
                        document.Servers =
                        [
                            new OpenApiServer()
                            {
                                Url = DevelopmentServerUrl, Description = "Development environment"
                            }];
                    }

                    return Task.CompletedTask;
                });
            });

            return services;
        }
    }
}