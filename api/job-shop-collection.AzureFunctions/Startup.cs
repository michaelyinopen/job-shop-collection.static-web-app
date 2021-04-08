using job_shop_collection.AzureFunctions.Data;
using job_shop_collection.AzureFunctions.Dtos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(job_shop_collection.AzureFunctions.Startup))]
namespace job_shop_collection.AzureFunctions
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("JobShopCollectionConnectionString");
            builder.Services.AddDbContext<JobShopCollectionDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}