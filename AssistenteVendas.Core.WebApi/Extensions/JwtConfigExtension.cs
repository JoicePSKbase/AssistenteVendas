using AssistenteVencas.Core.WebApi.Configurations;
using AssistenteVendas.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AssistenteVencas.Core.WebApi.Extensions
{
    public static class JwtConfigExtension
    {
        public static void AddJwtConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            var variavelSenha = configuration["JwtConfig:Secret"];
            if (!string.IsNullOrEmpty(variavelSenha))
            {
                var secretJwt = UtilExtension.GetDecodedEnvironmentVariable(variavelSenha);
                configuration["JwtConfig:Secret"] = secretJwt;
            }

            var jwtConfigSection = configuration.GetSection("JwtConfig");
            services.Configure<JwtConfig>(jwtConfigSection);

            var jwtConfig = jwtConfigSection.Get<JwtConfig>();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.ValidoEm,
                    ValidIssuer = jwtConfig.Emissor,
                    RequireExpirationTime = false
                };
            });
        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
