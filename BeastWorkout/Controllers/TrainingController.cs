using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingRepositoryDAL _trainingRepositoryDAL;

        public TrainingController(ITrainingRepositoryDAL trainingRepositoryDAL)
        {
            _trainingRepositoryDAL = trainingRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _trainingRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_trainingRepositoryDAL.GetAll().Select(t => Mappers.ToASP(t)));
        }

        public IActionResult Create(Training t)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            _trainingRepositoryDAL.Create(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Training t)
        {
            if (!ModelState.IsValid)
            {
                Training newT = Mappers.ToASP(_trainingRepositoryDAL.GetById(t.Id));
                return View(newT);
            }

            _trainingRepositoryDAL.Update(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Training t)
        {
            _trainingRepositoryDAL.Delete(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_trainingRepositoryDAL.GetById(id)));
        }
    }
}
