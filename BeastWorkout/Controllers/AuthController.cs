using BeastWorkout.Models;
using BeastWorkout.Session;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepositoryDAL _authRepositoryDAL;
        private readonly IPersonRepositoryDAL _personRepositoryDAL;
        private readonly ICountryRepositoryDAL _countryRepositoryDAL;

        public AuthController(IAuthRepositoryDAL authRepositoryDAL, IPersonRepositoryDAL personRepositoryDAL, ICountryRepositoryDAL countryRepositoryDAL)
        {
            _authRepositoryDAL = authRepositoryDAL;
            _personRepositoryDAL = personRepositoryDAL;
            _countryRepositoryDAL = countryRepositoryDAL;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login(LoginForm form)
        {
            ViewBag.Title = "Login";

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            try
            {
                Person p = Mappers.ToASP(_authRepositoryDAL.Login(Mappers.ToDAL(form)));

                if (p != null)
                {
                    // Appeler l'API d'authentification distant pour obtenir le token JWT
                    var token = _authRepositoryDAL.GetJwtToken(Mappers.ToDAL(form));

                    // Stocker le token dans la session ou dans un cookie sécurisé
                    // (Assurez-vous de gérer correctement le stockage du token pour la sécurité)
                    HttpContext.Session.SetString("AccessToken", token);

                    HttpContext.Session.SetUser(p);

                    //_authRepositoryDAL.CallApiWithJwtToken(token);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Disconnect();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var listC = _countryRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listC, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Register(Person p)
        {
            if (!ModelState.IsValid)
            {
                var listC = _countryRepositoryDAL.GetAll();
                ViewBag.List = new SelectList(listC, "Id", "Name");
                return View(p);
            }

            if (p.Auth_key == null && p.Created_at == null && p.Is_validate == null && p.Id_type_person == null)
            {
                p.Auth_key = "default_auth_key";
                p.Created_at = DateTime.Now;
                p.Is_validate = false;
                p.Id_type_person = 2;
            }

            //p.Password = BCrypt.Net.BCrypt.HashPassword(p.Password);

            _authRepositoryDAL.Register(Mappers.ToDAL(p));
            return RedirectToAction("Login");
        }
    }
}
