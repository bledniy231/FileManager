using FileManager.DAL.Domain.Identity;
using FileManager.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FileManager
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
					ValidIssuer =configManager["Jwt:Issuer"],
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
			=> services.AddIdentity<FileManagerUser, IdentityRole<long>>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 5; 
			})
			.AddEntityFrameworkStores<FileManagerDbContext>()
			.AddDefaultTokenProviders()
			.AddUserManager<UserManager<FileManagerUser>>()
			.AddSignInManager<SignInManager<FileManagerUser>>();
	}
}
