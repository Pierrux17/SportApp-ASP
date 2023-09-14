using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace BeastWorkout.Controllers
{
    public class TrainingExerciceController : Controller
    {
        private readonly ITrainingExerciceRepositoryDAL _trainingExerciceRepositoryDAL;
        private readonly ITrainingRepositoryDAL _trainingRepositoryDAL;
        private readonly IExerciceRepositoryDAL _exerciceRepositoryDAL;

        public TrainingExerciceController(ITrainingExerciceRepositoryDAL trainingExerciceRepositoryDAL, ITrainingRepositoryDAL trainingRepositoryDAL, IExerciceRepositoryDAL exerciceRepositoryDAL)
        {
            _trainingExerciceRepositoryDAL = trainingExerciceRepositoryDAL;
            _trainingRepositoryDAL = trainingRepositoryDAL;
            _exerciceRepositoryDAL = exerciceRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _trainingExerciceRepositoryDAL.CallApiWithJwtToken(token);
            _trainingRepositoryDAL.CallApiWithJwtToken(token);
            _exerciceRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View();
        }

        //--------------------- UPDATE TRAINING EXERCICE ---------------------
        [HttpGet]
        public IActionResult Edit(int id_training, int id_exercice)
        {
            TrainingExercice t = Mappers.ToASP(_trainingExerciceRepositoryDAL.GetById(id_training, id_exercice));
            return View(t);
        }

        [HttpPost]
        public IActionResult Edit(TrainingExercice t)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            _trainingExerciceRepositoryDAL.Update(Mappers.ToDAL(t));
            return RedirectToAction("MyExercices", new { id = t.Id_training });
        }

        //--------------------- DELETE TRAINING EXERCICE ---------------------
        public IActionResult Delete(int id_training, int id_exercice)
        {
            TrainingExercice t = Mappers.ToASP(_trainingExerciceRepositoryDAL.GetById(id_training, id_exercice));
            _trainingExerciceRepositoryDAL.Delete(Mappers.ToDAL(t));

            return RedirectToAction("MyExercices", new { id = t.Id_training });
        }
    }
}
