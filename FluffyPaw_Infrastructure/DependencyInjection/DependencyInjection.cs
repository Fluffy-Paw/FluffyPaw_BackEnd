﻿using Firebase.Auth;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Infrastructure.Authentication;
using FluffyPaw_Infrastructure.Data;
using FluffyPaw_Infrastructure.Hashing;
using FluffyPaw_Infrastructure.Intergrations.Firebase;
using FluffyPaw_Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);

            services.AddRepositories();

            //services.AddRabbitMQServices(configuration);

            //services.AddQuartzAndSchedule();

            services.AddService();

            //services.AddSignalR();

            services.AddAuthen(configuration);

            //services.AddUtils();

            services.AddExternalServices();

            //services.AddPayOS(configuration);

            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));
            services.AddDbContext<MyDbContext>(options =>
                {
                    var connectionString = configuration.GetConnectionString("MyDB");
                    options.UseMySql(connectionString, serverVersion, options => 
                    options.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName));
                }
            );
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddAuthen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthentication, Authen>();
            services.AddScoped<IHashing, Hash>();
        }

        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            
        }



        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IFirebaseConfiguration, FirebaseConfiguration>();
        }

        /*public static void AddPayOS(this IServiceCollection services, IConfiguration configuration)
        {
            PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));

            services.AddSingleton(payOS);

            services.AddControllersWithViews();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                    });
            });
        }*/
    }
}
