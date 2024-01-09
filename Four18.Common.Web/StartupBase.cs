using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using Four18.Common.Web.Configuration;
using Four18.Common.Web.Middleware;

namespace Four18.Common.Web;

public abstract class StartupBase
{
    private const string CorsPolicyName = "CorsPolicy";

    protected IConfiguration Configuration { get; set; }
    protected string ApiName { get; set; }
    protected int ApiId { get; set; }
    protected Lazy<string[]> CorsOrigins { get; }

    protected StartupBase(IConfiguration config)
    {
        Configuration = config;
        ApiName = string.Empty;
        ApiId = 0;
        CorsOrigins = new Lazy<string[]>(GetCorsOrigins);
    }

    protected void ConfigureCommonServices(IServiceCollection services, TeCommonWebOptions? webOptions = null)
    {
        webOptions ??= new TeCommonWebOptions();
        
        // Register Web Api services
        services.AddCorrelationId();
        services.AddDefaultCorrelationId();
        services.AddOptions();
        if (webOptions.UseNewtonsoftJson)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                });
        }
        else
        {
            services.AddControllers();
        }

        var healthChecksBuilder = services.AddHealthChecks();
        webOptions.CustomHealthChecks?.Invoke(healthChecksBuilder);

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName,
                builder =>
                {
                    if (CorsOrigins.Value.Length > 0)
                    {
                        builder.WithOrigins(CorsOrigins.Value);
                    }
                    else
                    {
                        builder.SetIsOriginAllowed(_ => true);
                    }

                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders(HeaderNames.ContentDisposition, HeaderNames.ContentLength, "X-Correlation-Id")
                        .AllowCredentials();
                });
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{ApiName.ToUpper(CultureInfo.CurrentCulture)} API", Version = "v1" });
            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "docs.xml");
            c.IncludeXmlComments(filePath);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
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
            c.IgnoreObsoleteActions();
        });

        // Register common services
        services.RegisterCommonServices();

        // Don't map standard claim types to other values
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        // Get config values for the jwt configs below
        var identityAuthority = Configuration.GetValue<string>("ApiUris:UriDictionary:Identity");
        if (string.IsNullOrWhiteSpace(identityAuthority))
        {
            throw new ArgumentException("Config Value 'ApiUris:UriDictionary:Identity' Not Configured");
        }
        
        if (string.IsNullOrWhiteSpace(identityAuthority))
        {
            throw new ArgumentException("Config Value 'CertificatePath' Not Configured");
        }

        var backendAuthJwtOptions = Configuration.GetSection(BackendAuthJwtOptions.SectionName).Get<BackendAuthJwtOptions>();
        if (string.IsNullOrWhiteSpace(backendAuthJwtOptions!.Secret))
        {
            throw new ArgumentException(
                $"Config Section '{BackendAuthJwtOptions.SectionName}:{nameof(backendAuthJwtOptions.Secret)}' Not Configured");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

            // SymmetricKey JWT Handler used for backend-to-backend service communication
            .AddJwtBearer("BackendOnlyAuth", configureOptions: jwtBackendBearerOptions =>
            {
                //use this to validate specifics about the backend token & secret
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(backendAuthJwtOptions.Secret));
                jwtBackendBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricKey,
                    ValidAudience = "Four18",
                    ValidIssuer = "Four18",
                    RequireSignedTokens = true,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ValidateAudience = true,
                    ValidateIssuer = true
                };
            })

            // Adds Microsoft Identity platform
            .AddMicrosoftIdentityWebApi(

                configureJwtBearerOptions: jwtBearerOptions =>
                {
                    //use this to validate specifics about the access token from Azure AD
                },

                configureMicrosoftIdentityOptions: microsoftIdentityOptions =>
                {
                    //use this to configure Microsoft Identity Platform
                    Configuration.Bind("AzureAdB2C", microsoftIdentityOptions);

                }).EnableTokenAcquisitionToCallDownstreamApi(
                options =>
                {
                    //add certificate logic for calling Graph (currently using ClientSecret)

                }).AddInMemoryTokenCaches();

        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, "BackendOnlyAuth");
            defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        IdentityModelEventSource.ShowPII = true;

        var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

        if (loggerFactory != null)
        {
            loggerFactory.AddSerilog();
            var log = loggerFactory.CreateLogger("Startup"); 
            app.UseCorrelationId();
            app.PipelineCheckPoint(log);
        }


        // Middleware Order
        // ExceptionHandler -> HSTS -> Redirection -> Static Files -> Routing -> CORS -> Authentication -> Authorization -> Custom -> Endpoint (last)

        app.Use(async (context, next) =>
        {
            context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                NoCache = true,
                NoStore = true,
                MaxAge = new TimeSpan(0),
                MustRevalidate = true
            };
            context.Response.Headers[HeaderNames.Vary] = new[] {"Accept-Encoding"};
            await next();
        });
        app.UseStaticFiles();
        app.UseSwagger(c => { c.RouteTemplate = $"api/{ApiName}/docs/{{documentName}}/swagger.json"; });
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = $"Swagger UI - {ApiName.ToUpper(CultureInfo.CurrentCulture)}";
            c.RoutePrefix = $"api/{ApiName}/docs/swagger";
            c.SwaggerEndpoint($"/api/{ApiName}/docs/v1/swagger.json", $"{ApiName.ToUpper(CultureInfo.CurrentCulture)} API V1");
            c.DocExpansion(DocExpansion.None);
        });
        app.UseRouting();
        app.UseCors(CorsPolicyName);
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks($"/api/{ApiName}/health", new HealthCheckOptions
            {
                ResponseWriter = WriteHealthChecksResponse
            });
        });
    }

    private string[] GetCorsOrigins()
    {
        var corsOriginsSection = Configuration.GetValue<string?>("CorsOrigins");
        if (corsOriginsSection == null)
        {
            return Array.Empty<string>();
        }

        var corsOrigins = corsOriginsSection
            .Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim())
            .ToArray();
        return corsOrigins;
    }

    private static Task WriteHealthChecksResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("status",
                    healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("description",
                    healthReportEntry.Value.Description);
                jsonWriter.WriteStartObject("data");

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    System.Text.Json.JsonSerializer.Serialize(jsonWriter, item.Value, item.Value?.GetType() ?? typeof(object));
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        return context.Response.WriteAsync(
            Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}
