using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BeastWorkout.Controllers
{
    public class ExerciceController : Controller
    {
        private readonly IExerciceRepositoryDAL _exerciceRepositoryDAL;
        private readonly ITypeExerciceRepositoryDAL _typeExerciceRepositoryDAL;

        public ExerciceController(IExerciceRepositoryDAL exerciceRepositoryDAL, ITypeExerciceRepositoryDAL typeExerciceRepositoryDAL)
        {
            _exerciceRepositoryDAL = exerciceRepositoryDAL;
            _typeExerciceRepositoryDAL = typeExerciceRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _exerciceRepositoryDAL.CallApiWithJwtToken(token);
            _typeExerciceRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_exerciceRepositoryDAL.GetAll().Select(e => new Exercice
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Picture = e.Picture,
                Id_type_exercice = e.Id_type_exercice,
                TypeExercice = Mappers.ToASP(_typeExerciceRepositoryDAL.GetById(e.Id_type_exercice)),
            }).OrderBy(e => e.Id_type_exercice));
        }

        [HttpGet]
        public IActionResult Create() 
        {
            var listTE = _typeExerciceRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listTE, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Exercice e)
        {
            if (!ModelState.IsValid)
            {
                return View(e);
            }

            _exerciceRepositoryDAL.Create(Mappers.ToDAL(e));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var listTE = _typeExerciceRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listTE, "Id", "Name");

            Exercice ex = Mappers.ToASP(_exerciceRepositoryDAL.GetById(id));
            if(ex == null)
            {
                return RedirectToAction("ExerciceNotFound", "Error");
            }
            return View(ex);
        }

        [HttpPost]
        public IActionResult Edit(Exercice e)
        {
            if (!ModelState.IsValid)
            {
                return View(e);
            }

            _exerciceRepositoryDAL.Update(Mappers.ToDAL(e));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Exercice e)
        {
            _exerciceRepositoryDAL.Delete(Mappers.ToDAL(e));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_exerciceRepositoryDAL.GetById(id)));
        }
    }
}
