using System;
using System.Diagnostics;
using Dex.Web.Areas.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dex.Web.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //throw new InvalidOperationException();
            return View();
            //return RedirectToPage("/Home/About");
        }

        public IActionResult Privacy()
        {
            //return RedirectToPage("/Home/About");
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return RedirectToPage("/Home/About");
        }

        [HttpGet]
        public IActionResult Blog()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllDownloads()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return RedirectToPage("/Error", new { area = "Shared", });
        }
    }
}
