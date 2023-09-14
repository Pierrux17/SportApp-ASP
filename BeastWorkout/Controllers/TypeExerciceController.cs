using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class TypeExerciceController : Controller
    {
        private readonly ITypeExerciceRepositoryDAL _typeExerciceRepositoryDAL;
        private readonly ISortExerciceRepositoryDAL _sortExerciceRepositoryDAL;

        public TypeExerciceController(ITypeExerciceRepositoryDAL typeExerciceRepositoryDAL, ISortExerciceRepositoryDAL sortExerciceRepositoryDAL)
        {
            _typeExerciceRepositoryDAL = typeExerciceRepositoryDAL;
            _sortExerciceRepositoryDAL = sortExerciceRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _typeExerciceRepositoryDAL.CallApiWithJwtToken(token);
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
            return View(_typeExerciceRepositoryDAL.GetAll().Select(t => new TypeExercice
            {
                Id = t.Id,
                Name = t.Name,
                Picture = t.Picture,
                Id_sort_exercice = t.Id_sort_exercice,
                SortExercice = Mappers.ToASP(_sortExerciceRepositoryDAL.GetById(t.Id_sort_exercice)),
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var listSE = _sortExerciceRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listSE, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(TypeExercice t)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            _typeExerciceRepositoryDAL.Create(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var listSE = _sortExerciceRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listSE, "Id", "Name");

            TypeExercice te = Mappers.ToASP(_typeExerciceRepositoryDAL.GetById(id));
            if (te == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(te);
        }

        [HttpPost]
        public IActionResult Edit(TypeExercice t)
        {
            if (!ModelState.IsValid)
            {
                TypeExercice newT = Mappers.ToASP(_typeExerciceRepositoryDAL.GetById(t.Id));
                return View(newT);
            }

            _typeExerciceRepositoryDAL.Update(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(TypeExercice t)
        {
            _typeExerciceRepositoryDAL.Delete(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_typeExerciceRepositoryDAL.GetById(id)));
        }
    }
}
