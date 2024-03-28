using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text;
using test_case.api.Constants;
using test_case.api.Context;
using test_case.api.Filters;
using test_case.api.Interfaces;
using test_case.api.Models.DTO;
using test_case.api.Models.Transaction;
using test_case.api.Services;
using test_case.api.Validators;
using test_case.api.Middlewares;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace test_case.api
{
    public static class Infrastructure
    {
        public static WebApplication ConfigureApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMvcCore(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)));
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<EnumSchemaFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
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
                        new string[] { }
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                c.OperationFilter<AddSwaggerResponseAttributesFilter>();
            });
            builder.Services.AddDbContext<TestCaseContext>(options => options
                .UseSqlServer(builder.Configuration[ConfigurationConstants.ConnectionString]));
            builder.Services.AddTransient<IDbConnection>(_ => new SqlConnection(builder.Configuration[ConfigurationConstants.ConnectionString]));

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<IValidator<UserLoginDTO>, UserLoginDTOValidator>();
            builder.Services.AddSingleton<IValidator<UserRegisterDTO>, UserRegisterDTOValidator>();
            builder.Services.AddSingleton<IValidator<RefreshTokenDTO>, RefreshTokenDTOValidator>();
            builder.Services.AddSingleton<IValidator<AccessTokenDTO>, AccessTokenDTOValidator>();
            builder.Services.AddSingleton<Dictionary<Type, object>>();

            var defaultCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration[ConfigurationConstants.Issuer],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration[ConfigurationConstants.Audience],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[ConfigurationConstants.SecretAccessKey]!))
                    };
                });

            builder.Services.AddAuthorization();

            return UseApplication(builder);
        }

        public static WebApplication UseApplication(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseMiddleware<ValidatorMiddleware>();

            var validators = app.Services.GetService<Dictionary<Type, object>>()!;
            validators[typeof(AccessTokenDTO)] = app.Services.GetService<IValidator<AccessTokenDTO>>()!;
            validators[typeof(RefreshTokenDTO)] = app.Services.GetService<IValidator<RefreshTokenDTO>>()!;
            validators[typeof(UserLoginDTO)] = app.Services.GetService<IValidator<UserLoginDTO>>()!;
            validators[typeof(UserRegisterDTO)] = app.Services.GetService<IValidator<UserRegisterDTO>>()!;

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
