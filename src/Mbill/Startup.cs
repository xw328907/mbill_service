namespace Mbill;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddController();//����ע��Controller
        services.AddJwtBearer();//����Jwt
        services.AddSwagger();//����ע��Swagger
        services.AddMapper(Assembly.Load("Mbill.Service"));//����ʵ��ӳ��
        services.AddEasyCaching();//����ע��EasyCaching����
        //services.AddMongoClient();//����ע��MongoDB Client
        services.AddMiniProfilerSetup();//����ע����
        services.AddIpRateLimiting();//����ע������
        services.AddHealthChecks();//����ע�ὡ�����
        services.AddCorsConfig();//���ÿ���
        services.AddFileStorage(Configuration); // ������ţ�Ʒ���
        services.AddHttpClients();//����HttpClient
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new AutofacModule());//ע��һЩ����
        builder.RegisterModule(new RepositoryModule());//ע��ִ�
        builder.RegisterModule(new ServiceModule());//ע�����
        builder.RegisterModule(new DependencyModule());//�Զ�ע�ᣬ����Abp�еļ̳ж�Ӧ�Ľӿھͻ�ע���Ӧ�ӿڵ���������
        builder.RegisterModule(new FreeSqlModule());//ע��FreeSql
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger().UseSwaggerUI(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Mbill.index.html"));
        }
        //����
        app.UseCors(Appsettings.Cors.CorsName);

        //��̬�ļ�
        app.UseStaticFiles();

        // Ip����
        app.UseMiddleware<IpLimitMiddleware>();

        // ��¼ip����
        app.UseMiddleware<IPLogMilddleware>();

        ////�쳣�����м��
        //app.UseMiddleware<ExceptionHandlerMiddleware>();

        //��֤�м��
        app.UseAuthentication();

        // ���ܷ���
        app.UseMiniProfiler();

        app.UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
    }
}
