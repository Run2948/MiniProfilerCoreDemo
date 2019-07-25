# ʹ�� MiniProfiler ������ ASP.NET Core Ӧ��

 MiniProfiler��[https://miniprofiler.com/](https://miniprofiler.com/)����һ���������Ҽ����õķ������߿⣬��������������ASP.NET CoreӦ�á�

## �ŵ�

���ASP.NET Core MVCӦ�ã�ʹ��MiniProfiler���ŵ��ǣ�����ѽ��ֱ�ӷ���ҳ������½ǣ���ʱ���Ե���鿴�������Ļ��Ϳ��Ը�֪����ĳ������е���ô����ͬʱ��Ҳ��ζ�ţ����㿪���¹��ܵ�ͬʱ�����Ժܿ��ٵĵõ�������


## һ����װ����MiniProfiler

�����е�ASP.NET Core MVC��Ŀ�ͨ��Nuget��װMiniProfiler ��

`Install-Package MiniProfiler.AspNetCore.Mvc`

��ȻҲ����ͨ��`Nuget Package Manager`���ӻ����߰�װ

![](./imgs/01.png)

����������MiniProfiler���ܹ���������

#### ��һ��������`Startup.cs`��`ConfigureServices`��������`services.AddMiniProfiler();`

```csharp
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });


        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        // ��Ȼ����������������һ��lambda���ʽ��Ϊ�������Ӷ���һЩ�Զ�������ã�
        services.AddMiniProfiler(options =>
        {
            // �趨�������ڵ�λ�������½�
            options.PopupRenderPosition = RenderPosition.BottomLeft;
            // �趨�ڵ�������ϸ���������ʽTime With Children����
            options.PopupShowTimeWithChildren = true;
        });
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

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });
    }
```

#### ����������`MiniProfiler`��`Tag Helper`�ŵ�ҳ����

* _ViewImports ҳ������ MiniProfiler �� Tag Helper ��

```csharp
    ...

    @using StackExchange.Profiling

    ...
    @addTagHelper *, MiniProfiler.AspNetCore.Mvc
```

* �� MiniProfiler ��Tag Helper ���� _Layout.cshtml �У�

```csharp
    ...

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2019 - MiniProfilerCoreDemo - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>

    @* ��ʵ����ҳ�������ط���Ӧ�ÿ��ԣ����������������һЩ�ű��ļ������Խ������footer���棺 *@@* ��ʵ����ҳ�������ط���Ӧ�ÿ��ԣ����������������һЩ�ű��ļ������Խ������footer���棺 *@
    <mini-profiler />

    <environment include="Development">
        <script src="~/lib/jquery/dist/jquery.js"></script>
        <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.js"></script>
    </environment>

    ...

    @RenderSection("Scripts", required: false)
</body>
</html>

```

 ����Ӧ�ã����Կ������½Ǿ���MiniProfiler��

![](./imgs/02.png)

 �����֮��ᵯ�����ڣ�������ÿ���������ĺ���ʱ�䡣

![](./imgs/03.png)


## ����MiniProfiler ����ʹ��

#### �����ֲ�����

ǰ������������ʹ��`MiniProfiler`������ҳ���������̵�ʱ�䡣��`MiniProfiler`Ҳ������������һ�δ��������õ�ʱ�䡣�����ӣ�

```csharp
    public async Task<IActionResult> Index()
    {
#if !DEBUG
        // ��������ʹ����using��䣬����ʹ���� MiniProfiler ��� Current ���ԣ��ڸ�����������һ��Step()������
        // ��������������using�������Ĵ��룬��Step�����Ҫ�ṩһ�����������Ե���������ʾ�öδ���������ʲô������������ƻ���ʾ�ڽ���
        using (MiniProfiler.Current.Step("�����һ��"))
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
#else
        // ͨ�����һ�ʹ�� using ������Ƕ����ʹ��

        using(MiniProfiler.Current.Step("��1��"))
        {

            // ... ��ز���
            Thread.Sleep(30);

            using(MiniProfiler.Current.Step("��1.1��"))
            {
                // ... ��ز���
                Thread.Sleep(30);
            }

            using(MiniProfiler.Current.Step("��1.2��"))
            {
                // ... ��ز���
                Thread.Sleep(30);
            }
       }

        // ���������ֻ�����һ�仰����ôʹ��using�����Ե�̫�鷳�ˣ���������¿���ʹ�� Inline() ������
        var users = await MiniProfiler.Current.Inline(async () => await _context.Users.ToListAsync(), "�����һ��");
        return View(users);
#endif
    }
```

* ʹ�� using ����Ƕ�׽��չʾ��
![](./imgs/04.png)

* ʹ�� Inline() �������չʾ��
![](./imgs/05.png)

#### �Զ������ CustomTiming

 ��ʱ�򣬷���һЩ���������ⲿ������ʱ�����潲���������ܲ�̫��⣬�������ǾͿ���ʹ��`CustomTime()`����

```csharp
    public async Task<IActionResult> Privacy()
    {
        // ������������ʹ�� MiniProfiler.Current.CustomTiming() ������
        //  ��һ��������һ�����ڷ�����ַ������������õ���http��������д��http��
        //  �ڶ��������������ַ������������ò��ϣ���ʱ���գ�
        //  ������������ִ�����ͣ��������õ���Get��������д��GET��
        using (CustomTiming timing = MiniProfiler.Current.CustomTiming("http", string.Empty, "GET"))
        {
            var url = "http://27.24.159.155";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("/system/resource/code/news/click/dynclicks.jsp?clickid=1478&owner=1220352265&clicktype=wbnews");

            timing.CommandString = $"URL:{url}\n\r Response Code:{response.StatusCode}";

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching data from API");
            }

            var clickTimes = await response.Content.ReadAsStringAsync();

            ViewData["clickTimes"] = clickTimes;
        }

        return View();
    }
```

* ���г��򣬿��Կ������ڵ��Ҳ������`http`��һ�У�
 
![](./imgs/06.png)

* ��� http �����е�`153.1 (1)`�������ʹ��CustomTiming�������Ƕδ��룬�������URL�ͷ����붼��ʾ�˳�����

![](./imgs/07.png)


## ������WebApi��Ŀ��ʹ��MiniProfiler

#####  ������WebApi��Ŀ��ʹ��MiniProfiler�Ľ̳̣����Ʋ���[WEBAPI.md](./WEBAPI.md)


## �ġ�����Դ�룺
##### [MiniProfilerCoreDemo](https://github.com/Run2948/MiniProfilerCoreDemo)
