using Components.Core.entities.Emails;
using Components.Web;
using Contracts.Managers;
using DAL;
using DTO.CustomerProducts;
using DTO.Documents;
using DTO.Eaqf;
using DTO.FBInternalReport;
using DTO.FullBridge;
using DTO.ScheduleJob;
using DTO.TCF;
using LINK_UI.App_start;
using LINK_UI.Filters;
using LoggerComponent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQUtility;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPush;

namespace LINK_UI
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            // Init Serilog configuration
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            Configuration = configuration;

            //Log.Logger = new LoggerConfiguration()
            //            .Enrich.FromLogContext()
            //            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200/"))
            //            {
            //                AutoRegisterTemplate = true,
            //            })
            //        .CreateLogger();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:4200")
                .AllowCredentials();
            }));

            //services.AddMvc(config =>
            //{
            //    config.Filters.Add(typeof(RightFilter));
            //    //   config.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());

            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // services.AddAutoMapper();
            services.AddControllersWithViews(config => config.Filters.Add(typeof(RightFilter))).AddNewtonsoftJson();
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("MyPolicy"));
            //});
            //services.AddRabbitMQGenericClient();
            // services.AddRabbitMQGenericClient("localhost:15672");

            services.AddRabbitMQGenericClient(Configuration.GetValue<string>("RabbitMQServer"));

            // services.AddRabbitMQGenericClient(o => o.ConnectionString = "host=localhost:15672");
            // services.AddRabbitMQGenericClient(c =>c.con)

            //RabbitMQServiceCollectionExtensions.AddRabbitMQGenericClient(services);


            //services.AddScoped<RightFilter>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API LINK Services",
                    Description = "Services used by APILINK UI",
                });

                c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                    },
                    new List < string > ()
                }

            });
                c.CustomSchemaIds(type => type.ToString());
                //c.OperationFilter<CustomSwaggerParametersFilter>();
            });

            services.AddSession(options =>
            {
                options.Cookie.Name = ".APILINK.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });

            // SMTPSettings
            services.Configure<ApismtpSettings>(Configuration.GetSection("SMTPSettings:API"));
            services.Configure<SgtsmtpSettings>(Configuration.GetSection("SMTPSettings:SGT"));
            services.Configure<AqfsmtpSettings>(Configuration.GetSection("SMTPSettings:AQF"));

            // Full bridge Settings
            services.Configure<FBSettings>(options => Configuration.GetSection("FBSettings").Bind(options));
            //TCF settings
            services.Configure<TCFSettings>(options => Configuration.GetSection("TCFSettings").Bind(options));
            //schedule settings
            services.Configure<CulturaPackingSettings>(Configuration.GetSection("ScheduleJobSettings:CulturaPacking"));

            services.Configure<EAQFSettings>(options => Configuration.GetSection("EAQFSettings").Bind(options));
            //Ocr settings
            services.Configure<OcrSettings>(options => Configuration.GetSection("OCRSettings").Bind(options));

            // Add background services
            services.AddHostedService<BackgroundServiceWorker>();

            // add browser detection
            services.AddBrowserDetection();

            // Dependancy Injection
            ApplicationServicesConfiguration.Configure(services);

            // AUthentification service
            AuthentificationService.Configure(services, Configuration);

            // Entity Framework Service
            services.AddDbContext<API_DBContext>(
                options =>
                {
                    options.UseSqlServer(this.Configuration.GetConnectionString("APIConnection"), option =>
                  {
                      //  option.UseRowNumberForPaging(); this code for 2008r2 but not support from 3.0
                      option.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                      option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                  });
                    options.ConfigureWarnings(builder =>
                     {
                         //suppress the global query filter warnings
                         builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                     });
                }                
           );

            // Memory Cache
            services.AddMemoryCache();

            var sp = services.BuildServiceProvider();
            // Register UserRightManager to filter
            // GlobalFilter.SetUserRightManager(sp.GetService<IUserRightsManager>());
            // RightFilter.SetUserRightManager(sp.GetService<IUserRightsManager>());

            // Init Controller context for components
            ControlllerExtensions.InitControlllerExtensions(sp);

            // upload file szie
            services.Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = Int32.MaxValue;
            });

            // all mapper profile classs assembly
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //vapidDetails
            var vapidDetails = new VapidDetails(
            Configuration.GetValue<string>("VapidDetails:Subject"),
            Configuration.GetValue<string>("VapidDetails:PublicKey"),
            Configuration.GetValue<string>("VapidDetails:PrivateKey"));
            services.AddTransient(c => vapidDetails);
            services.AddTransient<IBroadCastService, MyBroadCastService>();

            // init some data in cache :'
            //Entity : 

            var itUserManager = sp.GetService<IUserRightsManager>();
            itUserManager.SetCacheEntities();
            itUserManager.SetRightsCaches();

            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});

            // suppress model level error 
            services.AddControllers().ConfigureApiBehaviorOptions(option =>
              option.SuppressModelStateInvalidFilter = true
            );

            services.AddSignalR((options) =>
            {
                options.EnableDetailedErrors = true;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // TODO To remove after  move to 3.2
            //app.Use(async (context, next) =>
            //{
            //    await next.Invoke();

            //    GC.Collect(2, GCCollectionMode.Forced, true);
            //    GC.WaitForPendingFinalizers();
            //});




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapHub<UserHub>("/userhub");
                endpoint.MapControllers();
                endpoint.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<UserHub>("/userhub");
            //});



            app.UseSession();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            // logging
            loggerFactory.AddSerilog();

#if MULTI
            app.Run(context =>
            {
                context.Response.Redirect("swagger");
                return Task.CompletedTask;
            });
#endif

#if !MULTI

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                //if (env.IsDevelopment())
                //{
                //    spa.UseAngularCliServer(npmScript: "start");
                //}
            });
#endif


            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });


            // Database logger 
            loggerFactory.AddProvider(new DataBaseLoggerProvider(this.Configuration.GetConnectionString("APIConnection")));
        }
    }
}
