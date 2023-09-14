using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class TypePersonController : Controller
    {
        private readonly ITypePersonRepositoryDAL _typePersonRepositoryDAL;

        public TypePersonController(ITypePersonRepositoryDAL typePersonRepositoryDAL)
        {
            _typePersonRepositoryDAL = typePersonRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _typePersonRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_typePersonRepositoryDAL.GetAll().Select(t => Mappers.ToASP(t)));
        }

        public IActionResult Create(TypePerson t)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            _typePersonRepositoryDAL.Create(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(TypePerson t)
        {
            if (!ModelState.IsValid)
            {
                TypePerson newT = Mappers.ToASP(_typePersonRepositoryDAL.GetById(t.Id));
                return View(newT);
            }

            _typePersonRepositoryDAL.Update(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(TypePerson t)
        {
            _typePersonRepositoryDAL.Delete(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_typePersonRepositoryDAL.GetById(id)));
        }
    }
}
