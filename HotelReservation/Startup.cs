using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HotelReservation.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HotelReservation.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HotelReservation
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment _env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddJsonOptions(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 44326;
            });

            services.AddDbContext<HotelReservationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HotelReservation")));
            services.AddScoped<HotelReservationDbInitializer>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddAutoMapper();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(options =>
            options.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin()
            );
            app.UseMvc();
        }
    }
}
