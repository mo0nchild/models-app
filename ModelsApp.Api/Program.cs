
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Minio;
using ModelsApp.Api.Commons.ConfigureOptions;
using ModelsApp.Api.Commons.Middlewares;
using ModelsApp.Api.Services;
using ModelsApp.Api.Services.S3Storage;
using ModelsApp.Dal;
using System.Net;
using System.Security.Claims;
using static ModelsApp.Api.Commons.ConfigureOptions.ConfigureJwtBearer;

namespace ModelsApp.Api
{
    public class Program : object
    {
        public static readonly string CorsName = "WebUI";
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddProblemDetails();

            builder.Services.AddCors(options => options.AddPolicy(CorsName, builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            builder.Services.AddControllers();
            builder.Services.Configure<JwtBearerConfig>(options =>
            {
                var authOptions = builder.Configuration.GetSection("Authentication").Get<JwtBearerConfig>()!;
                options.SecretKey = Guid.NewGuid().ToString();
                (options.Issuer, options.Audience) = (authOptions.Issuer, authOptions.Audience);
            });
            builder.Services.ConfigureOptions<ConfigureJwtBearer>();
            builder.Services.ConfigureOptions<ConfigureApiAccess>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", item => item.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("User", item => item.RequireClaim(ClaimTypes.Role, "User"));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("modelsapp", new OpenApiInfo()
                {
                    Title = "Models App",
                    Version = "v1",
                    Description = "API for sharing 3d models",
                    Contact = new OpenApiContact()
                    {
                        Name = "byterbrod",
                        Url = new Uri("https://github.com/mo0nchild")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Jwt access token for using API",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[]{}
                    }
                });
                var localpath = AppDomain.CurrentDomain.BaseDirectory;
                options.IncludeXmlComments(Path.Combine(localpath, "ModelsApp.Api.xml"));
            });
            await builder.Services.AddDatabase(builder.Configuration);
            await builder.Services.AddApiServices(builder.Configuration);

            var application = builder.Build();
            if (application.Environment.IsDevelopment())
            {
                application.UseSwagger();
                application.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/modelsapp/swagger.json", "modelsapp");
                });
            }
            application.UseCors(CorsName);

            application.UseApiAccess().UseRequestLogging();
            application.UseAuthentication().UseAuthorization();

            application.MapControllers();
            application.Run();
        }
    }
}
