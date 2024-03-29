﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestYourself.Contracts.Requests;
using TestYourself.Data;
using TestYourself.Services;

namespace TestYourself.Installers
{
  public class DbInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options => options
      .UseSqlServer(configuration
      .GetConnectionString("DefaultConnection")));

      services.AddTransient<MobileContext>();

      services.AddDefaultIdentity<IdentityUser>()
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<DataContext>();

      services.AddScoped<IPostService, PostService>();
      services.AddScoped<IProfileService, ProfileService>();
    }
  }
}
