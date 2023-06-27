using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {

            _configuration = configuration;
            _env = env;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }

        public IConfiguration _configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //X509Certificate2 cert = null;

            //cert = new X509Certificate2(Path.Combine(_env.ContentRootPath, "SgTIdentity.pfx"), "SgTfine@010");

            services.AddTransient<ICustomTokenRequestValidator, CustomClaimInjection>();

            services.AddIdentityServer()
              .AddDeveloperSigningCredential(persistKey: false)
            // .AddSigningCredential(cert) // live server purpose
            .AddInMemoryPersistedGrants()
            .AddInMemoryApiResources(GetApiResources())
            .AddInMemoryClients(GetClients())
            .AddCustomTokenRequestValidator<CustomClaimInjection>();

            //add framework services
            services.AddControllersWithViews();
        }

        private IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                {
                    new ApiResource(_configuration["apiScope"], "My API"),
                    new ApiResource(_configuration["roleScope"], "My Roles", new[] { "role" })
                };
        }

        private IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                  new Client
                        {
                            ClientId = _configuration["apiClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["apiSecretKey"].Sha256())
                            },
                               AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] },
                               AccessTokenLifetime = Int32.Parse(_configuration["apiTokenLifeTime"]) // 15 days
                        },

                 new Client
                        {
                            ClientId = _configuration["fbClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["fbSecretKey"].Sha256())
                            },
                            AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] }
                        },
                  new Client
                        {
                            ClientId = _configuration["zohoClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["zohoSecretKey"].Sha256())
                            },
                            AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] }
                        },
                    new Client
                        {
                            ClientId = _configuration["invoicePdfClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["invoicePdfSecretKey"].Sha256())
                            },
                            AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] }
                        },

                      new Client
                        {
                            ClientId = _configuration["mobileAppClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["mobileAppSecretKey"].Sha256())
                            },
                            AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] }
                        },
                      new Client
                        {
                            ClientId = _configuration["eaqfClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["eaqfSecretKey"].Sha256())
                            },
                            AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] }
                        },
                       new Client
                        {
                            ClientId = _configuration["cflClientId"],
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets =
                            {
                                new Secret(_configuration["cflSecretKey"].Sha256())
                            },
                               AllowedScopes = { _configuration["apiScope"], _configuration["roleScope"] }                               
                        },
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
                endpoint.MapControllerRoute(
                   name: "default",
                   pattern: "{controller}/{action=Index}/{id?}");
            }
            );
            loggerFactory.AddSerilog();
        }
    }
}
