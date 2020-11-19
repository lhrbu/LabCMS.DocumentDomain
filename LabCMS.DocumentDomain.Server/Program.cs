using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.DocumentDomain.Server.Repositories;
using System.IO;
using LabCMS.DocumentDomain.Shared.Models;

namespace LabCMS.DocumentDomain.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TestWriteDocument(args);
            CreateHostBuilder(args).Build().Run();
        }

        public static void TestWriteDocument(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            var repository = host.Services.GetRequiredService<DocumentContentsRepository>();
            var res2 = host.Services.GetRequiredService<DocumentIndiceRepository>();
            using Stream stream = File.OpenRead("appsettings.json");
            byte[] content = new byte[stream.Length];
            stream.Read(content);
            Guid id = Guid.NewGuid();
            //repository.UploadFileAsync(new DocumentContent() { Id = id, Base64Content = content }).Wait();
            repository.DeleteByIdAsync(id).AsTask().Wait();
            //repository.UploadFile(new DocumentContent() { Id = id, Content = content });
            //var value = repository.FindById(id);
            //repository.DeleteById(id);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
