using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileManager.DAL
{
	public static class HostExtensions
	{
		public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration cfg)
			=> services.AddDbContext<FileManagerDbContext>(options =>
			{
				options.UseSqlServer(cfg.GetConnectionString("SqlServerConnectionString"));
			});

		public static IServiceProvider MigrateDbContext(this IServiceProvider services)
		{
			using var scope = services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<FileManagerDbContext>();
			dbContext.Database.Migrate();
			return services;
		}
	}
}
