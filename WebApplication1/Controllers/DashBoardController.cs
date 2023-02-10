using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ModelContext _context;

        public DashBoardController(ModelContext context) 
        {
            _context = context; 
        }


        public IActionResult Admin()
        {
            //ViewBag.EmployeeName = HttpContext.Session.GetString("AdminName");

            //ViewBag.NumberOfCustomer = _context.Customers.Count();
            //ViewBag.Sales = _context.Products.Sum(x=> x.Sale);
            ViewBag.Customer = _context.Customers.ToList();
            ViewData["Ayman"] = _context.Customers.Count();
            return View();
        }
        public IActionResult Customer()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Accountant()
        {
            var clients = _context.Customers.ToList();
            var products = _context.Products.ToList();

            var model3 = Tuple.Create<IEnumerable<Customer>, IEnumerable<Product>>(clients, products);
            
            return View(model3);
        }
        public IActionResult CatProduct(int id)
        {
            var pro = _context.Products.Where(x => x.CategoryId==id).ToList();
            return View(pro);
        }

        public IActionResult JoinTable()
        {
            var Customers = _context.Customers.ToList();
            var Products =_context.Products.ToList();
            var Categories = _context.Categories.ToList();
            var ProductCustomers =_context.ProductCustomers.ToList();

            //Join

            var multiTable = from c /*variable*/ in Customers
                             join pc /*variable*/ in ProductCustomers on c.Id equals pc.CustomerId
                             join p /*variable*/ in Products on pc.ProductId equals p.Id
                             join cat /*variable*/ in Categories on p.CategoryId equals cat.Id
                             select new JoinTable { product = p, category = cat, customer = c, productCustomer = pc };




            return View(multiTable);
        }
        [HttpGet]
        public IActionResult Report()
        {
            var Customers = _context.Customers.ToList();
            var Products = _context.Products.ToList();
            var Categories = _context.Categories.ToList();
            var ProductCustomers = _context.ProductCustomers.ToList();

            //Join

            var multiTable = from c /*variable*/ in Customers
                             join pc /*variable*/ in ProductCustomers on c.Id equals pc.CustomerId
                             join p /*variable*/ in Products on pc.ProductId equals p.Id
                             join cat /*variable*/ in Categories on p.CategoryId equals cat.Id
                             select new JoinTable { product = p, category = cat, customer = c, productCustomer = pc };

            var modelContext = _context.ProductCustomers.Include(p => p.Customer).Include(p => p.Product).ToList();
            ViewBag.TotalQuantity = modelContext.Sum(x => x.Quantity);
            ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price);

            var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<ProductCustomer>>(multiTable, modelContext);
            return View(model3);
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            var Customers = _context.Customers.ToList();
            var Products = _context.Products.ToList();
            var Categories = _context.Categories.ToList();
            var ProductCustomers = _context.ProductCustomers.ToList();

            var multiTable = from c /*variable*/ in Customers
                             join pc /*variable*/ in ProductCustomers on c.Id equals pc.CustomerId
                             join p /*variable*/ in Products on pc.ProductId equals p.Id
                             join cat /*variable*/ in Categories on p.CategoryId equals cat.Id
                             select new JoinTable { product = p, category = cat, customer = c, productCustomer = pc };


            var modelContext = _context.ProductCustomers.Include(p => p.Customer).Include(p => p.Product);

            if (startDate == null && endDate == null)
            {
                ViewBag.TotalQuantity = modelContext.Sum(x => x.Quantity);
                ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price);

                var model3 =  Tuple.Create<IEnumerable<JoinTable>, IEnumerable<ProductCustomer>>(multiTable, await modelContext.ToListAsync());

                return View(model3);
            }
            else if (startDate == null && endDate != null)
            {
                ViewBag.TotalQuantity = modelContext.Sum(x => x.Quantity);
                ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price);

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<ProductCustomer>>(multiTable, await modelContext.Where(x => x.DateTo.Value.Date == endDate).ToListAsync());
               
                return View(model3);
            }
            else if (startDate != null && endDate == null)
            {
                ViewBag.TotalQuantity = modelContext.Sum(x => x.Quantity);
                ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price);

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<ProductCustomer>>(multiTable, await modelContext.Where(x => x.DateFrom.Value.Date == startDate).ToListAsync());
                return View(model3);
            }
            else
            {
                ViewBag.TotalQuantity = modelContext.Sum(x => x.Quantity);
                ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price);

                var model3 = Tuple.Create<IEnumerable<JoinTable>, IEnumerable<ProductCustomer>>(multiTable, await modelContext.Where(x => x.DateFrom >= startDate && x.DateTo <= endDate).ToListAsync());
                
                return View(model3);
            }

        }

    }
}
