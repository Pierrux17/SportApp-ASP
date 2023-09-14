using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class TypeProgramController : Controller
    {
        private readonly ITypeProgramRepositoryDAL _typeProgramRepositoryDAL;

        public TypeProgramController(ITypeProgramRepositoryDAL typeProgramRepositoryDAL)
        {
            _typeProgramRepositoryDAL = typeProgramRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _typeProgramRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_typeProgramRepositoryDAL.GetAll().Select(t => Mappers.ToASP(t)));
        }

        public IActionResult Create(TypeProgram t)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            _typeProgramRepositoryDAL.Create(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(TypeProgram t)
        {
            if (!ModelState.IsValid)
            {
                TypeProgram newT = Mappers.ToASP(_typeProgramRepositoryDAL.GetById(t.Id));
                return View(newT);
            }

            _typeProgramRepositoryDAL.Update(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(TypeProgram t)
        {
            _typeProgramRepositoryDAL.Delete(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_typeProgramRepositoryDAL.GetById(id)));
        }
    }
}
