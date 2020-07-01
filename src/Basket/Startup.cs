using AutoMapper;
using BasketApi.Data.Context;
using BasketApi.Domain.Events.Product;
using BasketApi.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Qtyb.Common.EventBus.Extensions;
using Qtyb.Common.EventBus.Interfaces;
using System.Reflection;

namespace BasketApi
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
            services.AddDbContext<BasketContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("BasketContext")));

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(BasketApi), Version = "v1" });
            });

            services.AddEventBus();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //TODO 3: Move to extension method
            var subsriberService = app.ApplicationServices.GetRequiredService<IEventBusSubscriber>();
            subsriberService.Subscribe<ProductCreated>();
            subsriberService.BindQueue<ProductRejected>();
        }
    }
}