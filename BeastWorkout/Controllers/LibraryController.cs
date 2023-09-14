using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IExerciceRepositoryDAL _exerciceRepositoryDAL;
        private readonly ITypeExerciceRepositoryDAL _typeExerciceRepositoryDAL;
        private readonly ISortExerciceRepositoryDAL _sortExerciceRepositoryDAL;
        private readonly ITrainingRepositoryDAL _trainingRepositoryDAL;

        public LibraryController(IExerciceRepositoryDAL exerciceRepositoryDAL, ITypeExerciceRepositoryDAL typeExerciceRepositoryDAL, ISortExerciceRepositoryDAL sortExerciceRepositoryDAL, ITrainingRepositoryDAL trainingRepositoryDAL)
        {
            _exerciceRepositoryDAL = exerciceRepositoryDAL;
            _typeExerciceRepositoryDAL = typeExerciceRepositoryDAL;
            _sortExerciceRepositoryDAL = sortExerciceRepositoryDAL;
            _trainingRepositoryDAL = trainingRepositoryDAL;
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
            _sortExerciceRepositoryDAL.CallApiWithJwtToken(token);
            _trainingRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult GetCategorieExercice(int? id_training, int? id_profil)
        {
            if(id_training != null)
            {
                ViewBag.Id_training = id_training;
            }
            if (id_profil != null)
            {
                ViewBag.Id_profil = id_profil;
            }
            return View(_sortExerciceRepositoryDAL.GetAll().Select(s => Mappers.ToASP(s)));
        }

        public IActionResult GetTypeExercice(int id_sort_exercice, int? id_training, int? id_profil)
        {
            if (id_training != null)
            {
                ViewBag.Id_training = id_training;
            }
            if (id_profil != null)
            {
                ViewBag.Id_profil = id_profil;
            }
            return View(_typeExerciceRepositoryDAL.GetAll().Where(t => t.Id_sort_exercice == id_sort_exercice).Select(t => Mappers.ToASP(t)));
        }
        public IActionResult GetExercice(int id_type_exercice, int? id_training, int? id_profil)
        {
            if (id_training != null)
            {
                ViewBag.Id_training = id_training;
            }
            if (id_profil != null)
            {
                ViewBag.Id_profil = id_profil;
            }
            return View(_exerciceRepositoryDAL.GetAll().Where(e => e.Id_type_exercice == id_type_exercice).Select(e => new Exercice
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Picture = e.Picture,
                Id_type_exercice = e.Id_type_exercice,
                TypeExercice = Mappers.ToASP(_typeExerciceRepositoryDAL.GetById(e.Id_type_exercice)),
            }));
        }

        public IActionResult DetailExercice(int id_exercice, int? id_training, int? id_profil)
        {
            if (id_training != null)
            {
                ViewBag.Id_training = id_training;
            }
            if (id_profil != null)
            {
                ViewBag.Id_profil = id_profil;
            }
            Exercice e = Mappers.ToASP(_exerciceRepositoryDAL.GetById(id_exercice));
            TypeExercice t = Mappers.ToASP(_typeExerciceRepositoryDAL.GetById(e.Id_type_exercice));

            e.TypeExercice = t;

            return View(e);
        }
    }
}
