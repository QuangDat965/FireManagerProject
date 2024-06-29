using FireManagerServer.BackgroundServices;
using FireManagerServer.Database;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.ApartmentService;
using FireManagerServer.Services.AuthenService;
using FireManagerServer.Services.DeviceServices;
using FireManagerServer.Services.HistoryServices;
using FireManagerServer.Services.ModuleServices;
using FireManagerServer.Services.RoleService;
using FireManagerServer.Services.RuleServiceServices;
using FireManagerServer.Services.UnitServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FireManagerServer
{
    public static class StartUp
    {
        public static WebApplicationBuilder AddServicesBase(this WebApplicationBuilder builder)
        {

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthenService, AuthenService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IBuildingService, BuildingService>();
            builder.Services.AddScoped<IApartmentService, ApartmentService>();
            builder.Services.AddScoped<IModuleService, ModuleService>();
            builder.Services.AddScoped<IRuleService, RuleService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();
            builder.Services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            return builder;
        }
        public static WebApplicationBuilder AddBackgroundServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();
            builder.Services.AddSingleton(typeof(ScopedServiceFactory<>));

            builder.Services.AddHostedService<ListeningService>();
            builder.Services.AddHostedService<AutoService>();
            return builder;
        }
        public static WebApplicationBuilder AddMySql(this WebApplicationBuilder builder)
        {
            var serverVersion = new MySqlServerVersion(new Version(5, 7, 0));

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            builder.Services.AddDbContext<FireDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), serverVersion, mysqlOptions =>
                    {
                        mysqlOptions.EnableRetryOnFailure();
                    })
                    );

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ManagerServer", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return builder;
        }
        public static WebApplication UsesService(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseCustomMiddleware();
            app.UseCors("AllowAllHeaders");
            app.UseHttpsRedirection();
            //app.UseMiddleware<ApiResponseMiddleware> ();
            app.MapControllers();
            return app;
        }
    }
}
