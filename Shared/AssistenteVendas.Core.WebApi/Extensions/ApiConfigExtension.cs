using AssistenteVendas.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asp.Versioning;
using AssistenteVencas.Core.WebApi.Middlewares;
using Microsoft.Extensions.Hosting;

namespace AssistenteVencas.Core.WebApi.Extensions
{
    public static class ApiConfigExtension
    {
        private const string CORS_ALLOW = "CorsAllow";

        public static IServiceCollection AddApiConfig(
            this IServiceCollection services,
            IConfiguration configuration,
            string origensPermitidas)
        {

            services
                .AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddCors(
            options =>
            {
                options.AddPolicy(CORS_ALLOW,
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins(origensPermitidas.Split("|", StringSplitOptions.RemoveEmptyEntries))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithExposedHeaders("Content-Disposition");
                    });
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

     
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var mailSenhaConf = configuration["MailConfig:Smtp:Password"];
            if (!string.IsNullOrEmpty(mailSenhaConf))
            {
                var mailSenha = UtilExtension.GetDecodedEnvironmentVariable(mailSenhaConf);
                configuration["MailConfig:Smtp:Password"] = mailSenha;
            }

            var clienteIdConf = configuration["MailConfig:ActiveDirectory:ClientId"];
            if (!string.IsNullOrEmpty(clienteIdConf))
            {
                var clienteId = UtilExtension.GetDecodedEnvironmentVariable(clienteIdConf);
                configuration["MailConfig:ActiveDirectory:ClientId"] = clienteId;
            }

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRouting();

            app.UseCors(CORS_ALLOW);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
