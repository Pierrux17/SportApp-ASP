using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class SortExerciceController : Controller
    {
        private readonly ISortExerciceRepositoryDAL _sortExerciceRepositoryDAL;

        public SortExerciceController(ISortExerciceRepositoryDAL sortExerciceRepositoryDAL)
        {
            _sortExerciceRepositoryDAL = sortExerciceRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _sortExerciceRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_sortExerciceRepositoryDAL.GetAll().Select(s => Mappers.ToASP(s)));
        }

        public IActionResult Create(SortExercice s)
        {
            if (!ModelState.IsValid)
            {
                return View(s);
            }

            _sortExerciceRepositoryDAL.Create(Mappers.ToDAL(s));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(SortExercice s)
        {
            if (!ModelState.IsValid)
            {
                SortExercice newS = Mappers.ToASP(_sortExerciceRepositoryDAL.GetById(s.Id));
                return View(newS);
            }

            _sortExerciceRepositoryDAL.Update(Mappers.ToDAL(s));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(SortExercice s)
        {
            _sortExerciceRepositoryDAL.Delete(Mappers.ToDAL(s));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_sortExerciceRepositoryDAL.GetById(id)));
        }
    }
}
