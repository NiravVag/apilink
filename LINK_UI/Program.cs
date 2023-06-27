using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace LINK_UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
                 webBuilder.UseKestrel(serverOptions =>
                 {
                     serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                     serverOptions.Limits.MinRequestBodyDataRate = null;
                     serverOptions.Limits.MinResponseDataRate = null;
                 });
             });
    }
}
