using Microsoft.AspNetCore.Mvc;
using DAL.Repositories;
using BeastWorkout.Tools;
using BeastWorkout.Models;
using DAL.Services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryRepositoryDAL _countryRepository;
        public CountryController(ICountryRepositoryDAL countryRepository)
        {
            _countryRepository = countryRepository;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _countryRepository.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE QUE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_countryRepository.GetAll().Select(c => Mappers.ToASP(c)));
        }

        public IActionResult Create(Country c)
        {
            if (!ModelState.IsValid)
            {
                return View(c);
            }

            _countryRepository.Create(Mappers.ToDAL(c));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Country c)
        {
            if (!ModelState.IsValid)
            {
                Country viewC = Mappers.ToASP(_countryRepository.GetById(c.Id));
                return View(viewC);
            }

            _countryRepository.Update(Mappers.ToDAL(c));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Country c) 
        {
            _countryRepository.Delete(Mappers.ToDAL(c));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_countryRepository.GetById(id)));
        }
    }
}
