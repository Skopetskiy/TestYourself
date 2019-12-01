using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestYourself.Data;
using TestYourself.Installers;
using TestYourself.Options;

namespace TestYourself
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      Install.InstallServicesInAssembly(services, Configuration);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }
     
      app.Use(async (ctx, next) =>
      {
        await next();
        if (ctx.Response.StatusCode == 204)
        {
          ctx.Response.ContentLength = 0;
        }
      });

      var swaggerOptions = new SwaggerOptions();

      Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

      app.UseSwagger(option =>
      {
        option.RouteTemplate = swaggerOptions.JsonRoute;
      });

      app.UseSwaggerUI(option =>
      {
        option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
      });

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseAuthentication();

      app.UseCors(options =>
      options.WithOrigins("http://localhost:4200")
      .AllowAnyMethod()
      .AllowAnyHeader());

      app.UseMvc(); 
    }
  }
}
