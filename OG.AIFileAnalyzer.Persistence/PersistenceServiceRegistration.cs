﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AIFileAnalyzerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AIFileAnalyzerDB")));
            services.AddTransient<IAzureAIService, AzureAIService>();
        }
    }
}
