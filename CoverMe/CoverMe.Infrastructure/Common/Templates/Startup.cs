using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoverCore.Abstractions.Templates;
using CoverMe.Domain.Entities.Settings;
using CoverMe.Infrastructure.Common.Settings.Services;
using CoverMe.Infrastructure.Common.Templates.Services;

namespace CoverMe.Infrastructure.Common.Templates;

public static class Startup
{
	/// <summary>
	///     Adds a singleton service of the typed ApplicationSettings stored in appsettings.json
	/// </summary>
	public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<ITemplateService, TemplateService>();
	}

	public static void Configure(IApplicationBuilder app, IConfiguration configuration)
	{
		using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
		{
			// Load templates
			var templateService = serviceScope.ServiceProvider.GetRequiredService<ITemplateService>();
		}
	}
}

