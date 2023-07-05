using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Task1.Data;
using Task1.Models;

namespace Task1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDBContext _context;

        public HomeController(ILogger<HomeController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (name == "admin" && password == "admin123")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("name", name));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, name));
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                //claimIdentity
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //claimPrincipal
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(claimPrincipal);

                return RedirectToAction("GetAll", "Student");
            }

            var user = _context.Users.Include(x => x.Student).Where(x => x.Password == password).FirstOrDefault();
            var userName = "";
            try
            {
                    userName = user.Student.FirstName + " " +
                    user.Student.FatherName + " " +
                    user.Student.GrandFatherName + " " +
                    user.Student.LastName;
            }
            catch (Exception)
            {
                ViewBag.ErrMsg = "Envalid Login, Please Enter Valid Informations.";
                return View();
            }

            if (userName == name && user.Password == password)
            {
                if(user.state == 0)
                {
                    ViewBag.ErrMsg = "Envalid Login, This User Is Not Active.";
                    return View();
                }

                var claims = new List<Claim>();
                claims.Add(new Claim("userName", userName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userName));
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                
                //claimIdentity
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //claimPrincipal
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(claimPrincipal);

                int userId = user.Id;
                return RedirectToAction("UserInfo", new {userId});

            }
            else
            {
                ViewBag.ErrMsg = "Envalid Login, Please Enter Valid Informations.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult UserInfo(int userId)
        {
            var user = _context.Users.Include(x => x.Student).Where(x => x.Id == userId).FirstOrDefault();
            return View(user);  
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
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