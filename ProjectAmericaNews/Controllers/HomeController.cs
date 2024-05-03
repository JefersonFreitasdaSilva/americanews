using Microsoft.AspNetCore.Mvc;
using ProjectAmericaNews.Models;
using System.Diagnostics;
using AmericaNews.Services;

namespace ProjectAmericaNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var service = new HomeService();
            var titulo = service.GetTitulo();
            var data = new IndexViewData() { Titulo = titulo };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
