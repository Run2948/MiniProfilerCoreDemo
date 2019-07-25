using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiniProfilerCoreWebApiDemo.Models;
using StackExchange.Profiling;

namespace MiniProfilerCoreWebApiDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        // 首先添加一个配置选项，用于访问分析结果：
        services.AddMiniProfiler(options =>
        {
            // 设定弹出窗口的位置是左下角
            options.PopupRenderPosition = RenderPosition.BottomLeft;
            // 设定在弹出的明细窗口里会显式Time With Children这列
            options.PopupShowTimeWithChildren = true;
            // 设定访问分析结果URL的路由基地址
            options.RouteBasePath = "/profiler";
        })
        // 然后在之前的配置后边加上AddEntityFramework()：
        .AddEntityFramework();
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 最重要的一点是就是配置中间件在管道中的位置，一定要把它放在UseMvc()方法之前。 
            app.UseMiniProfiler();

            app.UseMvc();
        }
    }
}
