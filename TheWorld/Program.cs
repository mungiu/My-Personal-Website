using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace TheWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //listening on network ports for web requests
            //
            var host = new WebHostBuilder()
                //weserver type
                .UseKestrel()
                //where should the content deliver
                .UseContentRoot(Directory.GetCurrentDirectory())
                //IIS hosting
                .UseIISIntegration()
                //how to handle web requests
                .UseStartup<Startup>()
                //telemetry about application
                .UseApplicationInsights()
                //build into a host
                .Build();
            //start listening to web requests
            host.Run();
        }
    }
}
