using MongoDB.Driver;
using Serilog.Events;
using System.IO;

namespace Mbill;

public class Program
{
    public static async Task Main(string[] args)
    {
        SerilogConfig();
        try
        {
            SnowFlake.SnowFlakeConfig();
            IHost webHost = CreateHostBuilder(args).Build();
            try
            {
                using var scope = webHost.Services.CreateScope();                
                var ipPolicyStore = scope.ServiceProvider.GetRequiredService<IIpPolicyStore>();                
                await ipPolicyStore.SeedAsync(); 
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "IIpPolicyStore RUN Error");
            }
            await webHost.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())//添加Autofac服务工厂
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
#if DEBUG
            .UseUrls("http://*:9000");
#endif
                ;
            }).UseSerilog();//构建Serilog;
    private static void SerilogConfig()
    {
        var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("serilogSetting.json", optional: false, reloadOnChange: true)
         .AddEnvironmentVariables()
         .Build();
        var loggerConfig = new LoggerConfiguration().ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("Application", $"Mbill");
        Log.Logger = loggerConfig.CreateLogger();
        //Log.Logger = new LoggerConfiguration()
        //    .WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}")
        //    .WriteTo.MongoDBBson(cfg =>
        //    {
        //        var mongoDbInstance = new MongoClient(Appsettings.MongoDBCon).GetDatabase(Appsettings.MongoDBName);
        //        cfg.SetMongoDatabase(mongoDbInstance);
        //        cfg.SetCollectionName("logs");      
        //    }, LogEventLevel.Warning)
        //    .Enrich.FromLogContext()
        //    .CreateLogger();
    }
}
