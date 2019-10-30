using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestYourself.Authorization;
using TestYourself.Options;
using TestYourself.Services;

namespace TestYourself.Installers
{
  public class MvcInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
      var jwtSettings = new JwtSettings();
      configuration.Bind(key: nameof(jwtSettings), jwtSettings);
      services.AddSingleton(jwtSettings); 

      services.AddScoped<IIdentityService, IdentityService>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
      };

      services.AddSingleton(tokenValidationParameters);

      services.AddAuthentication(configureOptions: x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }
        )
        .AddJwtBearer(x =>
        {
          x.SaveToken = true;
          x.TokenValidationParameters = tokenValidationParameters;
        });

      services.AddAuthorization(
        //options =>
        //{
        //  options.AddPolicy(name: "WorksForDima", configurePolicy: builder => builder.AddRequirements(new WorksRequirements(domainName: ".dima")));
        //}
      );

      //services.AddSingleton<IAuthorizationHandler, WorksHandler>();

      services.AddSwaggerGen(x =>
      {
        x.SwaggerDoc("v1", new Info { Title = "TestYourself", Version = "v1" });

        var security = new Dictionary<string, IEnumerable<string>>
        {
          { "Bearer", new string[]{ } }
        };

        x.AddSecurityDefinition(name: "Bearer", new ApiKeyScheme
        {
          Description = "JWT",
          Name = "Authorization",
          In = "header",
          Type = "apiKey"
        });

        x.AddSecurityRequirement(security);
      });
    }
  }
}
