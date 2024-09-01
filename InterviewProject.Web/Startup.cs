using FluentValidation;
using FluentValidation.AspNetCore;
using InterviewProject.Configuration;
using InteviewProject.Application;
using InteviewProject.Application.Validations.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;

namespace InterviewProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews()
                         .AddJsonOptions(options =>
                         {
                             options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                             options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                         });
            // Swagger configuration
            services.AddSwaggerGen();
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddValidatorsFromAssemblyContaining<GetLocationsCommandValidator>();
            services.AddFluentValidationAutoValidation();
            // Registrar HttpClient con una BaseAddress
            services.AddHttpClient("AccuWeatherClient", client =>
            {
                client.BaseAddress = new Uri("http://dataservice.accuweather.com/");
                
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // URL de tu frontend
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable Swagger in development
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty; // Opcional: hace que Swagger esté en la raíz
                });
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("AllowSpecificOrigins");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
