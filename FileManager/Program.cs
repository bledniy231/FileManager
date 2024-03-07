using FileManager.BLL;
using FileManager.BLL.CryptoLinkManager;
using FileManager.BLL.MultipartRequestHelper;
using FileManager.BLL.TokenService;
using FileManager.BLL.UploadPercentageChecker;
using FileManager.DAL;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FileManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGenWithBearerAuthentication();

			builder.Services.ConfigureDbContext(builder.Configuration);
			builder.Services.ConfigureBLL();
			builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			builder.Services.AddBearerAuthentication(builder.Configuration);
			builder.Services.AddBearerAuthorization();
			builder.Services.AddApplicationIdentity();

			builder.Services.AddSingleton<ITokenService, TokenService>();
			builder.Services.AddSingleton<IMultipartRequestHelper, MultipartRequestHelper>();
			builder.Services.AddSingleton<ICryptoLinkManager, CryptoLinkManagerViaAes>();
			builder.Services.AddSingleton<IPercentageChecker, PercentageChecker>();
			builder.Services.TryAddSingleton<FormOptions>();

			builder.Services.Configure<FormOptions>(options =>
			{
				options.ValueLengthLimit = int.MaxValue;
				options.MultipartHeadersLengthLimit = int.MaxValue;
				options.MultipartBoundaryLengthLimit = 524288000;
			});

			builder.Services.Configure<IISServerOptions>(options =>
			{
				options.MaxRequestBodySize = 524288000;
			});

			builder.Services.Configure<KestrelServerOptions>(options =>
			{
				options.Limits.MaxRequestBodySize = int.MaxValue;
			});

			var app = builder.Build();

			app.Services.MigrateDbContext();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
