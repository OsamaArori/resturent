using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context; 

        public HomeController(ILogger<HomeController> logger, ModelContext modelContext)
        {
            _logger = logger;
            _context = modelContext;
        }

        public IActionResult Index()
        {
            Menu menu = new Menu()
            {
                Meal="Mansaf",
                ReleaseDate= new DateTime(2022,8,25)
            };
            return View(menu);
        }

        public IActionResult Privacy()
        { 
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult Book()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search()
        {
            var modelContext = _context.ProductCustomers.Include(p => p.Customer).Include(p => p.Product).ToList();
            return View(modelContext);
        }
        [HttpPost]
        public async Task<IActionResult> Search(DateTime? startDate,DateTime? endDate)
        {
            var modelContext = _context.ProductCustomers.Include(p => p.Customer).Include(p => p.Product);
            if (startDate == null && endDate == null)
            {
                return View(modelContext);
            }
            else if (startDate == null && endDate != null)
            {
                var result = await modelContext.Where(x => x.DateTo.Value.Date == endDate).ToListAsync();
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                var result = await modelContext.Where(x => x.DateFrom.Value.Date == startDate).ToListAsync();
                return View(result);
            }
            else  
            {
                var result = await modelContext.Where(x => x.DateFrom >= startDate && x.DateTo <= endDate).ToListAsync();
                return View(result);
            }
           
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
