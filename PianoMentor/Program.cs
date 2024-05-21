using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PianoMentor.BLL;
using PianoMentor.BLL.CryptoLinkManager;
using PianoMentor.BLL.MultipartRequestHelper;
using PianoMentor.BLL.TokenService;
using PianoMentor.BLL.UploadPercentageChecker;
using PianoMentor.Controllers;
using PianoMentor.DAL;
using PianoMentor.JsonSettings;
using PianoMentor.Middleware;
using System.Reflection;

namespace PianoMentor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
			});
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGenWithBearerAuthentication();

			builder.Services.AddStackExchangeRedisCache(options => 
			{
				options.Configuration = "localhost";
				options.InstanceName = "local_PianoMentor";
			});
			builder.Services.ConfigureDbContext(builder.Configuration);
			builder.Services.ConfigureBLL();
			builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			builder.Services.AddBearerAuthentication(builder.Configuration);
			builder.Services.AddBearerAuthorization();
			builder.Services.AddApplicationIdentity();

			//builder.Services.AddTransient<TokenServiceMiddleware>();
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
			//app.UseMiddleware<TokenServiceMiddleware>();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
