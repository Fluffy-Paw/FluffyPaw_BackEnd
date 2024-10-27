using Firebase.Auth;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using FluffyPaw_Application.Utils.Pagination;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Infrastructure.Authentication;
using FluffyPaw_Infrastructure.Data;
using FluffyPaw_Infrastructure.Hashing;
using FluffyPaw_Infrastructure.Intergrations.Firebase;
using FluffyPaw_Infrastructure.Intergrations.Quartz;
using FluffyPaw_Infrastructure.Intergrations.SignalR;
using FluffyPaw_Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Quartz;
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

            services.AddQuartzAndSchedule();

            services.AddService();

            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(60);
            });

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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddScoped<IAuthentication, Authen>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IHashing, Hash>();
        }

        public static void AddQuartzAndSchedule(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            services.ConfigureOptions<QuartzJobSetup>();

            services.AddSingleton(provider => provider.GetRequiredService<ISchedulerFactory>().GetScheduler().Result);

            services.AddScoped<IJobScheduler, JobScheduler>();
        }

        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IServiceTypeService, ServiceTypeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IPetOwnerService, PetOwnerService>();
            services.AddScoped<ISerService, SerService>();
            services.AddScoped<ICertificateService, CertificatesService>();
            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<IVaccineService, VaccineService>();
            services.AddScoped<IStoreManagerService, StoreManagerService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IPaymentService, PaymentService>();
        }



        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IFirebaseConfiguration, FirebaseConfiguration>();
            services.AddScoped<ISignalRConfiguration, SignalRConfiguration>();
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
