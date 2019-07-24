using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProfilerCoreDemo.Models;
using MiniProfilerCoreDemo.Models.Entities;
using StackExchange.Profiling;

namespace MiniProfilerCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
#if DEBUG
            // 这里我们使用了using语句，里面使用了 MiniProfiler 类的 Current 属性，在该属性上面有一个Step()方法，
            // 它可以用来分析using语句里面的代码，在Step方法里，要提供一个具有描述性的名称来表示该段代码做的是什么动作，这个名称会显示在结果里。

            // 通常，我会使用 using 语句块来嵌套着使用

            using (MiniProfiler.Current.Step("第1步"))
            {
                var users = await _context.Users.ToListAsync();

                using (MiniProfiler.Current.Step("第1.1步"))
                {
                    // ... 相关操作
                    Thread.Sleep(30);
                }

                using (MiniProfiler.Current.Step("第1.2步"))
                {
                    // ... 相关操作
                    Thread.Sleep(30);
                }

                return View(users);
            }

#else
            // 但是如果你只想分析一句话，那么使用using语句就显得太麻烦了，这种情况下可以使用 Inline() 方法：
            var users = await MiniProfiler.Current.Inline(async () => await _context.Users.ToListAsync(), "计算第一步");
            return View(users);
#endif
        }

        public async Task<IActionResult> Privacy()
        {
            // 这个例子里，我们使用 MiniProfiler.Current.CustomTiming() 方法。
            //  第一个参数是一个用于分类的字符串，这里我用的是http请求，所以写了http；
            //  第二个参数是命令字符串，这里我用不上，暂时留空；
            //  第三个参数是执行类型，这里我用的是Get请求，所以写了GET；
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
