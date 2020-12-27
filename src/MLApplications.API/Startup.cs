using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MLApplications.API.Config;
using System.Reflection;
using Microsoft.Extensions.ML;

namespace MLApplications.API
{
    /// <summary>
    ///     Start up
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Cosmos DB for application data
            services.SetupCosmosDb(Configuration);
            
            // API controllers
            services.SetupControllers();

            // HttpContextServiceProviderValidatorFactory requires access to HttpContext
            services.AddHttpContextAccessor();

            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // MediatR
            services.SetupMediatr();

            // Swagger
            services.SetupNSwag();

            // TODO : instead of using singleton instance, consider using a PredictionEngine pool
            // ML Models
            services.AddSingleton<MLApplications.SentimentAnalysis.ConsumeModel>();


        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // ONLY automatically create development database
                //app.EnsureCosmosDbIsCreated();
                //app.SeedApplicationData();
                
            }

            // NSwag
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
