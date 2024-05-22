using Demo.PL.Models;
using Demo.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Demo.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IScopedService scoped1;
        //private readonly IScopedService scoped2;
        //private readonly ITransientService trnsient1;
        //private readonly ITransientService trnsient2;
        //private readonly ISingletonSrevice singleton1;
        //private readonly ISingletonSrevice singleton2;

        public HomeController(ILogger<HomeController> logger
        //              ,IScopedService scoped1
        //              ,IScopedService scoped2
        //              ,ITransientService trnsient1
        //              ,ITransientService trnsient2
        //              ,ISingletonSrevice singleton1
        //              ,ISingletonSrevice singleton2
                     )
        {
            _logger = logger;
        //    this.scoped1 = scoped1;
        //    this.scoped2 = scoped2;
        //    this.trnsient1 = trnsient1;
        //    this.trnsient2 = trnsient2;
        //    this.singleton1 = singleton1;
        //    this.singleton2 = singleton2;
       }

        public IActionResult Index()
        {
            return View();
        }

        //public string TestLifeTime()
        // {
        //   StringBuilder builder = new StringBuilder();
        //    builder.Append($"scoped1    :: {scoped1.GetGuid()}\n");
        //    builder.Append($"scoped2    :: {scoped2.GetGuid()}\n\n");
        //    builder.Append($"trnsient1  :: {trnsient1.GetGuid()} \n");
        //    builder.Append($"trnsient2  :: {trnsient2.GetGuid()}\n\n");
        //    builder.Append($"singleton1 :: {singleton1.GetGuid()} \n");
        //    builder.Append($"singleton2 :: {singleton2.GetGuid()}\n");

        //    return builder.ToString();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
