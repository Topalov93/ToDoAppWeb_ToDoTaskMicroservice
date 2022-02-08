using DAL.Data;
using DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ToDoApp.Services.TaskService;
using ToDoAppWeb.ExceptionHandler;
using ToDoTask_Message_Recieve.Options;

namespace ToDoAppWeb
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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoAppWeb", Version = "v1" });
            });

            //EFCore
            services.AddDbContext<ToDoAppDbContext>(options => options.UseSqlServer("Data Source = .;Initial Catalog = ToDoAppdbWeb_ToDoTaskMicroservice;Integrated Security = True;TrustServerCertificate = False;"), ServiceLifetime.Singleton);

            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqOptions>();
            services.Configure<RabbitMqOptions>(serviceClientSettingsConfig);

            services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMq"));

            services.AddTransient<IToDoTaskRepository, ToDoTasksRepository>();

            services.AddTransient<ITaskService, TaskService>();

            services.AddHostedService<UserUpdateReciever>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoAppWeb v1"));

                app.UseExceptionHandlerMiddleware();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //using (var serviceScope = serviceScopeFactory.CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetService<ToDoAppDbContext>();
            //    dbContext.Database.EnsureCreated();
            //}
        }
    }
}
