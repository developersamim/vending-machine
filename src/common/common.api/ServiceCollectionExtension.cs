using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.api;

public static class ServiceCollectionExtension
{
    public static TSettings AddAndBindConfigurationSection<TSettings>(this IServiceCollection services, IConfiguration configuration, string sectionPath)
            where TSettings : class, new()
    {
        var section = configuration.GetSection(sectionPath);
        services.Configure<TSettings>(section);
        var instance = new TSettings();
        section.Bind(instance);

        return instance;
    }
}
