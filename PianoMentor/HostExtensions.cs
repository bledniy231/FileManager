using PianoMentor.DAL.Domain.Identity;
using PianoMentor.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace PianoMentor
{
	public static class HostExtensions
	{
		public static IServiceCollection AddSwaggerGenWithBearerAuthentication(this IServiceCollection services)
			=> services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "The coolest File Manager you've ever seen", Version = "v1.0.0" });

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Enter a valid token...",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

		public static AuthenticationBuilder AddBearerAuthentication(this IServiceCollection services, ConfigurationManager configManager)
			=> services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configManager["Jwt:Issuer"],
					ValidAudience = configManager["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configManager["Jwt:Secret"]))
				};
			});

		public static AuthorizationBuilder AddBearerAuthorization(this IServiceCollection services)
			=> services.AddAuthorizationBuilder()
				.AddPolicy("RequireAdminRole", cfg => cfg.RequireRole("Admin"))
				.SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.Build());

		public static IdentityBuilder AddApplicationIdentity(this IServiceCollection services)
			=> services.AddIdentity<PianoMentorUser, IdentityRole<long>>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 5; 
			})
			.AddEntityFrameworkStores<PianoMentorDbContext>()
			.AddDefaultTokenProviders()
			.AddUserManager<UserManager<PianoMentorUser>>()
			.AddSignInManager<SignInManager<PianoMentorUser>>();

		public static void ConfigureCertificate(this IWebHostBuilder webBuilder)
		{
			var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);
			var certificate = store.Certificates.Where(c => c.FriendlyName.Equals("For project of Egor Seryakov's diploma")).FirstOrDefault()
				?? throw new ArgumentException("Certificate not found");


			webBuilder.UseKestrel(options =>
			{
				options.Listen(System.Net.IPAddress.Loopback, 44321, listenOptions =>
				{
					var connectionOptions = new HttpsConnectionAdapterOptions
					{
						ServerCertificate = certificate,
						//ServerCertificateChain = new X509Certificate2Collection(certificate),
						//ClientCertificateMode = ClientCertificateMode.AllowCertificate,
						//ClientCertificateValidation = (certificate, chain, errors) => errors == SslPolicyErrors.None
					};

					listenOptions.UseHttps(connectionOptions);
				});
			});
		}
	}
}
