# ��WebApi��Ŀ��ʹ��MiniProfiler���ҷ��� Entity Framework Core

## һ����װ����MiniProfiler

�����е�ASP.NET Core MVC WebApi ��Ŀ�ͨ��Nuget��װ`MiniProfiler`��

`Install-Package MiniProfiler.AspNetCore.Mvc MiniProfiler.EntityFrameworkCore`

��ȻҲ����ͨ��`Nuget Package Manager`���ӻ����߰�װ

![](./imgs/11.png)

����������������ú�ʹ�� MiniProfiler �ˣ��ܹ���������

#### ��һ��������`Startup.cs`��`ConfigureServices`��������`services.AddMiniProfiler();`

```csharp
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        // �������һ������ѡ����ڷ��ʷ��������
        services.AddMiniProfiler(options =>
        {
            // �趨�������ڵ�λ�������½�
            options.PopupRenderPosition = RenderPosition.BottomLeft;
            // �趨�ڵ�������ϸ���������ʽTime With Children����
            options.PopupShowTimeWithChildren = true;
            // �趨���ʷ������URL��·�ɻ���ַ
            options.RouteBasePath = "/profiler";
        })
        // Ȼ����֮ǰ�����ú�߼���AddEntityFramework()��
        .AddEntityFramework();
    }
```

#### �ڶ�������������`Startup.cs`��`Configure`��������`app.UseMiniProfiler();`

```csharp
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        ...

        // ����Ҫ��һ���Ǿ��������м���ڹܵ��е�λ�ã�һ��Ҫ��������UseMvc()����֮ǰ�� 
        app.UseMiniProfiler();

        app.UseMvc();
    }
```

#### �����������г���һ����3���ɲ鿴���������ص�URL��ַ��

####  1.`/profiler/results-index`

* �ȿ�results-indexҳ�棺 

![](./imgs/12.png)

> ����ʾÿ�ε���API�ļ�¼��������Կ������ε���API����ʱ��Ϊ1578.4���롣

####  2.`/profiler/results` 

* ��result-indexҳ�������ӽ������API���õ���ϸ���ҳ�棬Ҳ����resultҳ�棺

![](./imgs/13.png)

> ����ʾÿ�ε���API�Ĺ��̷�����������嵽ÿһ��SQL�������ݺ�ִ��ʱ�䡣

####  3.`/profiler/results-list`

* �ٿ�result-listҳ�棺

![](./imgs/14.png)

> ����ʵ�ͱ�ʾÿ��API�����е��ü�¼����ļ��ϡ�

## ��������Դ�룺
##### [MiniProfilerCoreWebApiDemo](https://github.com/Run2948/MiniProfilerCoreDemo/tree/master/MiniProfilerCoreWebApiDemo)
