using DentalApp.Application.Common.Interfaces;
using DentalApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Infrastructure
{
    public static class ServiceRegistration
    {
       
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
           
            services.AddScoped<IFileStorageService, MinioStorageService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        }
    }
}