using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepositoryDAL _personRepositoryDAL;
        private readonly ITypePersonRepositoryDAL _typePersonRepositoryDAL;
        private readonly ICountryRepositoryDAL _countryRepositoryDAL;

        public PersonController(IPersonRepositoryDAL personRepositoryDAL, ITypePersonRepositoryDAL typePersonRepositoryDAL, ICountryRepositoryDAL countryRepositoryDAL)
        {
            _personRepositoryDAL = personRepositoryDAL;
            _typePersonRepositoryDAL = typePersonRepositoryDAL;
            _countryRepositoryDAL = countryRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _personRepositoryDAL.CallApiWithJwtToken(token);
            _typePersonRepositoryDAL.CallApiWithJwtToken(token);
            _countryRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_personRepositoryDAL.GetAll().Select(p => new Person
            {
                Id = p.Id,
                Lastname = p.Lastname,
                Firstname = p.Firstname,
                Mail = p.Mail,
                Login = p.Login,
                Password = p.Password,
                Password_reset_token = p.Password_reset_token,
                Auth_key = p.Auth_key,
                Created_at = p.Created_at,
                Updated_at = p.Updated_at,
                Is_validate = p.Is_validate,
                Id_type_person = p.Id_type_person,
                Id_country = p.Id_country,
                TypePerson = Mappers.ToASP(_typePersonRepositoryDAL.GetById(p.Id_type_person ?? 0)),
                Country = Mappers.ToASP(_countryRepositoryDAL.GetById(p.Id_country)),
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var listC = _countryRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listC, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Person p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            if(p.Auth_key == null && p.Created_at == null && p.Is_validate == null && p.Id_type_person == null)
            {
                p.Auth_key = "default_auth_key";
                p.Created_at = DateTime.Now;
                p.Is_validate = false;
                p.Id_type_person = 2;
            }

            //p.Password = BCrypt.Net.BCrypt.HashPassword(p.Password);

            _personRepositoryDAL.Create(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //if (BeastWorkout.Session.SessionHelper.person == null)
            //{
            //    return RedirectToAction("Unauthorized", "Error");
            //}

            //int loggedInUserId = BeastWorkout.Session.SessionHelper.person.Id;
            //if (loggedInUserId != id && BeastWorkout.Session.SessionHelper.person.Id_type_person != 1)
            //{
            //    return RedirectToAction("Unauthorized", "Error");
            //}

            var listT = _typePersonRepositoryDAL.GetAll();
            var listC = _countryRepositoryDAL.GetAll();
            ViewBag.ListT = new SelectList(listT, "Id", "Name");
            ViewBag.ListC = new SelectList(listC, "Id", "Name");

            // Obtenez les informations de l'utilisateur à éditer à partir de l'ID fourni
            Person person = Mappers.ToASP(_personRepositoryDAL.GetById(id));
            if (person == null)
            {
                return RedirectToAction("UserNotFound", "Error");
            }

            return View(person);
        }


        [HttpPost]
        public IActionResult Edit(Person p)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(p);
            //}

            if(p.Auth_key == null)
            {
                p.Auth_key = "default_auth_key";
            }
            //p.Password = BCrypt.Net.BCrypt.HashPassword(p.Password);

            _personRepositoryDAL.Update(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Person p)
        {
            _personRepositoryDAL.Delete(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_personRepositoryDAL.GetById(id)));
        }
    }
}
