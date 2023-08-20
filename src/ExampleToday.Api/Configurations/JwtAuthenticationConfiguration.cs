using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ExampleToday.Api.Configurations
{
    public static class JwtAuthenticationConfiguration
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authUserSettingsConfiguration = configuration.GetSection(nameof(AuthUserSettings));
            var authUserSettings = authUserSettingsConfiguration.Get<AuthUserSettings>()!;
            services.Configure<AuthUserSettings>(authUserSettingsConfiguration);

            var authApplicationSettingsConfiguration = configuration.GetSection(nameof(AuthApplicationSettings));
            var authApplicationSettings = authApplicationSettingsConfiguration.Get<AuthApplicationSettings>()!;
            services.Configure<AuthApplicationSettings>(authApplicationSettingsConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("User", configure =>
            {
                configure.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authUserSettings.Issuer,
                    ValidAudience = authUserSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authUserSettings.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            }).AddJwtBearer("Application", configure =>
            {
                configure.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authApplicationSettings.Issuer,
                    ValidAudience = authApplicationSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authApplicationSettings.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            services.AddAuthentication();
        }
    }
}