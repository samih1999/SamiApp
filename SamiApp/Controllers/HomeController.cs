using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SamiApp.Entity;
using SamiApp.Models;
using SamiApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SamiApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotyfService _notyf;

        private readonly IHomePage _homePage;
        public readonly ILoginService _loginService;
        private readonly ILogger<HomeController> _logger;
        private readonly IRegisterService _Register;

        public HomeController(INotyfService notyf, IHomePage homePage, ILoginService loginService, ILogger<HomeController> logger, IRegisterService register)
        {
            _notyf = notyf;
            _homePage = homePage;
            _loginService = loginService;
            _logger = logger;
            _Register = register;
        }

        public IActionResult Index()
        {
            ViewBag.usernae_secc = HttpContext.Session.GetString("usernae_secc");
            var home = _homePage.GetAll().Select(ho => new HomePageViewModel
            {
                Id = ho.Id,
                Content = ho.Content,
                Email = ho.Email,
                Name = ho.Name

            });

            return View(home);
        }








        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {

            if (ModelState.IsValid)
            {
                using var hmac = new HMACSHA512();

                var reu = new Register
                {
                   
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    Email = reg.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(reg.Password)),
                    PasswordSalt = hmac.Key,
                };

                await _Register.CreateAsync(reu);
                return RedirectToAction(nameof(Login));
            }
            return View();

        }














        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewmodel model = new LoginViewmodel();
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {

               
              
                const string id = "id";
                const string usernae_secc = "usernae_secc";
        


                var check = _Register.logcheck(Username, Password);

                //var auth = _userContext.User.Where(x => x.UerName == UserName && x.Password == Password).SingleOrDefault();
                if (check != null)
                {
                   
                    HttpContext.Session.SetString(usernae_secc, check.Email.ToString());
                    HttpContext.Session.SetString(id, Convert.ToString(check.Id.ToString()));
                    if (_Register.GetRole(Username) == "Admin") return RedirectToAction("Content", "Home");
                    return RedirectToAction("Index");
                }

                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View();
                }
            }
            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("usernae_secc") != null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //public IActionResult Login(LoginViewmodel log)
        //{
        //  if(_loginService.CheckEmail(log.Username) && _loginService.CheckPass(log.Password))
        //   return View();
        //    return View();


        //}

        public IActionResult Content()
        {
            ViewBag.usernae_secc = HttpContext.Session.GetString("usernae_secc");
            var home = _homePage.GetAll().Select(ho => new HomePageViewModel
            {
                Id = ho.Id,
                Content = ho.Content,
                Email = ho.Email,
                Name = ho.Name

            });

            return View(home);
        }



        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.usernae_secc = HttpContext.Session.GetString("usernae_secc");
            var model = new HomePageViewModel();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Create(HomePageViewModel model)
        {
            ViewBag.usernae_secc = HttpContext.Session.GetString("usernae_secc");
            if (ModelState.IsValid)
            {
                var employee = new homePage
                {
                    Id = model.Id,
                    Content = model.Content,
                    Email = model.Email,
                    Name = model.Name
                };

                await _homePage.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }





        public IActionResult Edit(int id)
        { var employee = _homePage.GetById(id);
           
                ViewBag.usernae_secc = HttpContext.Session.GetString("usernae_secc");

            if (_Register.GetRole(ViewBag.usernae_secc) == "Admin")
            {
                if (employee == null)
                {
                    return NotFound();
                }
                var model = new HomePageViewModel()
                {
                    Id = employee.Id,
                    Content = employee.Content,
                    Email = employee.Email,
                    Name = employee.Name
                };
        
                return View(model);

            }
         
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(HomePageViewModel model)
        {
            ViewBag.usernae_secc = HttpContext.Session.GetString("usernae_secc");

            if (ModelState.IsValid)
            {
                if (_Register.GetRole(ViewBag.usernae_secc) == "Admin") { 
                var employee = _homePage.GetById(model.Id);
                if (employee == null)
                {
                    return NotFound();
                }
                employee.Content = model.Content;
                employee.Email = model.Email;
                employee.Id = model.Id;
                employee.Name = model.Name;


                await _homePage.UpdateAsync(employee);
                    _notyf.Success("edited ");

                    return RedirectToAction(nameof(Index));}
            }
            _notyf.Error("not edited");
            return View();
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
