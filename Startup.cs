using AutoMapper;
using GService.Common.Implementation;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Extensions;
using GudelIdService.Implementation.Persistence.Context;
using GudelIdService.Implementation.Persistence.Repository;
using GudelIdService.Implementation.Services;
using GudelIdService.Implementation.Services.Background;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GudelIdService
{
    public class Startup
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );
            
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped(typeof(IConverterService<>), typeof(ConverterService<>));
            services.AddScoped(typeof(IConverterServiceFactory<>), typeof(ConverterServiceFactory<>));
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IPoolRepository, PoolRepository>(); 
            services.AddScoped<IPoolService, PoolService>();
            services.AddScoped<IGudelIdStateRepository, GudelIdStateRepository>();
            services.AddScoped<IGudelIdStateService, GudelIdStateService>();
            services.AddScoped<IExtraFieldRepository, ExtraFieldRepository>();
            services.AddScoped<IExtraFieldService, ExtraFieldService>();
            services.AddScoped<IGudelIdService, Implementation.Services.GudelIdService>();
            services.AddScoped<IGudelIdRepository, GudelIdRepository>();
            services.AddScoped<IPermissionKeyRepository, PermissionKeyRepository>();
            services.AddScoped<IPermissionKeyService, PermissionKeyService>();
            services.AddScoped<IUtilsService, UtilsService>();
            services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());
            services.AddDbContext<AppDbContext>(); 
            services.AddHostedService<GenerateGudelIdsHostedService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient();


            services.AddCors(o => o.AddPolicy("DevelopmentPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(_ => true);
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ID Service API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {
            if (!context.Database.IsInMemory())
            {
                context.Database.Migrate();
            }

            app.UseGlobalExceptionHandler(env);
            if(env.IsDevelopment())
            {
                app.UseCors("DevelopmentPolicy");
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ID Service API");
                });
            }
            
            app.UseRouting();
            app.UseAuthMiddleware();
            app.UseRightsMiddleware();
            app.UseEndpoints(endpoints =>
                endpoints.MapControllers()
            );
        }
    }
}

