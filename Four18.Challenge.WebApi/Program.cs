using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;
using Four18.Challenge.Business.Interfaces;
using Four18.Challenge.Business.Services;
using Four18.Challenge.Data.Context;
using Four18.Challenge.Data.Interfaces;
using Four18.Challenge.Data.Repositories;
using Four18.Challenge.WebApi.Configuration;

Console.Title = "Customer API";

var builder = WebApplication.CreateBuilder(args);

var logFile = $"customers-api-.log";
var logPath = Path.Combine(new string[] { "logs", logFile });
Log.Logger = new LoggerConfiguration()
    // TODO: fix serilog configuration per environment
    //
    //.ReadFrom.Configuration(builder.Configuration)
    //.WriteToBlobStorage(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

Log.Logger.Information("test logger");

// get the configuration options
var azureADOptions = builder.Configuration.GetSection(AzureADConfigurationOptions.SectionName)
    .Get<AzureADConfigurationOptions>();
var databaseOption = builder.Configuration.GetSection(DatabaseConfigurationOptions.SectionName)
    .Get<DatabaseConfigurationOptions>();

// build the database connection
if (databaseOption == null) throw new ArgumentException("Database option was not created.");

if (!databaseOption.IsValid())
    throw new ArgumentException("One or more settings for the database are not properly configured");

var connectionString = databaseOption.ToString();

// TODO: add CORS

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options => {
            builder.Configuration.Bind(AzureADConfigurationOptions.SectionName, options);
            options.Events = new JwtBearerEvents();
        }, options => { builder.Configuration.Bind(AzureADConfigurationOptions.SectionName, options); }
    );

//  Add db context to container
builder.Services.AddDbContext<CustomerDbContext>(options => { options.UseInMemoryDatabase(databaseName: "TestDb"); });

// Add repositories to the container
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();


//Add services to the container
builder.Services.AddTransient<ICustomerService, CustomerService>();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FOUR18 API", Version = "1" });
    //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
    //    Type = SecuritySchemeType.OAuth2,
    //    Flows = new OpenApiOAuthFlows {
    //        Implicit = new OpenApiOAuthFlow {
    //            AuthorizationUrl =
    //                new Uri($"https://login.microsoftonline.com/{azureADOptions!.TenantId}/oauth2/v2.0/authorize"),
    //            TokenUrl = new Uri($"https://login.microsoftonline.com/{azureADOptions.TenantId}/oauth2/v2.0/token"),
    //            Scopes = new Dictionary<string, string> {
    //                { $"api://{azureADOptions.ClientId}/{azureADOptions.Scope}", "\r\nFour18 API Access" }
    //            }
    //        }
    //    }
    //});
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "local") {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();