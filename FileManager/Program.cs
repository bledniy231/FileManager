using FileManager.BLL;
using FileManager.BLL.CryptoLinkManager;
using FileManager.BLL.MultipartRequestHelper;
using FileManager.BLL.TokenService;
using FileManager.BLL.UploadPercentageChecker;
using FileManager.Controllers;
using FileManager.DAL;
using FileManager.Middleware;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FileManager
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			//builder.WebHost.ConfigureCertificate();
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGenWithBearerAuthentication();

			builder.Services.AddStackExchangeRedisCache(options => {
				options.Configuration = "localhost";
				options.InstanceName = "local_FileManager";
			});
			builder.Services.ConfigureDbContext(builder.Configuration);
			builder.Services.ConfigureBLL();
			builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			builder.Services.AddBearerAuthentication(builder.Configuration);
			builder.Services.AddBearerAuthorization();
			builder.Services.AddApplicationIdentity();

			builder.Services.AddTransient<TokenServiceMiddleware>();
			builder.Services.AddTransient<ITokenService, TokenService>();
			builder.Services.AddSingleton<IMultipartRequestHelper, MultipartRequestHelper>();
			builder.Services.AddSingleton<ICryptoLinkManager, CryptoLinkManagerViaAes>();
			builder.Services.AddSingleton<IPercentageChecker, PercentageChecker>();
			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			builder.Services.AddSingleton<ControllersHelper>();
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
				options.AllowSynchronousIO = true;
			});

			builder.Services.Configure<KestrelServerOptions>(options =>
			{
				options.Limits.MaxRequestBodySize = int.MaxValue;
				options.AllowSynchronousIO = true;
				//options.ConfigureHttpsDefaults(opt => opt.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
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
			app.UseMiddleware<TokenServiceMiddleware>();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
