using Autofac;
using EmailGate.Api.Configuration;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using EmailGate.Application.MassTransit;

namespace EmailGate.Api
{
    public class Startup
    {
        private static IConfiguration Configuration { get; set; } = null!;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            #region "API versioning"

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat           = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions                   = true;
                o.DefaultApiVersion                   = new ApiVersion(1, 0);
                o.ApiVersionReader                    = new UrlSegmentApiVersionReader();
            });

            #endregion

            #region "Swagger API"

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title       = "Tennisi Api",
                    Version     = "v1",
                    Description = "API для приема email",
                    Contact = new OpenApiContact
                    {
                        Name  = "Tennisi",
                        Email = "tennisi.it",
                    }
                });
            });

            #endregion

            #region "MassTransit"

            services.AddMassTransit(x => x.ConfigureRabbitMqPublish());
            services.AddMassTransitHostedService();

            #endregion

            services.RegisterConfigurations(Configuration);

            services.AddControllers();
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterApplicationComponents();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "EmailGate.Api v1");
                c.RoutePrefix = string.Empty;
                c.DisplayOperationId();
                c.DisplayRequestDuration();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
