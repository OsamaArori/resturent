using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginAndRegestrationController : Controller
    {

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoginAndRegestrationController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fname,Lname,ImagePath,ImageFile")] Customer customer, string username, string password)
        {
            if (ModelState.IsValid)
            {
                
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                string extension = Path.GetExtension(customer.ImageFile.FileName);
                string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                customer.ImagePath = fileName;
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await customer.ImageFile.CopyToAsync(filestream);
                }
                _context.Add(customer);
                await _context.SaveChangesAsync();
                var LastId = _context.Customers.OrderByDescending(p => p.Id).FirstOrDefault().Id;
                UserLogin login1 = new UserLogin();
                login1.RoleId = 2;
                login1.UserName = username;
                login1.Password = password;
                login1.CustomerId = LastId;
                _context.Add(login1);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "LoginAndRegestration");
            }
            return View(customer);
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([Bind("UserName,Password")] UserLogin userLogin)
        {
            var auth = _context.UserLogins.Where(x => x.UserName == userLogin.UserName && x.Password == userLogin.Password).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetInt32("CustomerId", (int)auth.CustomerId);
                        HttpContext.Session.SetString("AdminName", auth.UserName);
                        return RedirectToAction("Admin", "DashBoard");

                    case 2:
                        HttpContext.Session.SetInt32("CustomerId", (int)auth.CustomerId);
                        HttpContext.Session.SetString("AdminName", auth.UserName);
                        return RedirectToAction("Customer", "DashBoard");
                    case 3:
                        HttpContext.Session.SetInt32("CustomerId", (int)auth.CustomerId);
                        HttpContext.Session.SetString("AdminName", auth.UserName);
                        return RedirectToAction("Accountant", "DashBoard");
                }
            }
            return View();
        }


    }
}
