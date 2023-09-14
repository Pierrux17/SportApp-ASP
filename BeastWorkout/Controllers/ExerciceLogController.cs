using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeastWorkout.Controllers
{
    public class ExerciceLogController : Controller
    {
        private readonly IExerciceLogRepositoryDAL _exerciceLogRepository;
        private readonly ITrainingLogRepositoryDAL _trainingLogRepository;
        private readonly IExerciceRepositoryDAL _exerciceRepository;
        private readonly ITrainingExerciceRepositoryDAL _trainingExerciceRepository;


        public ExerciceLogController(IExerciceLogRepositoryDAL exerciceLogRepository, ITrainingLogRepositoryDAL trainingLogRepository, IExerciceRepositoryDAL exerciceRepository, ITrainingExerciceRepositoryDAL trainingExerciceRepository)
        {
            _exerciceLogRepository = exerciceLogRepository;
            _trainingLogRepository = trainingLogRepository;
            _exerciceRepository = exerciceRepository;
            _trainingExerciceRepository = trainingExerciceRepository;   
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _exerciceLogRepository.CallApiWithJwtToken(token);
            _trainingLogRepository.CallApiWithJwtToken(token);
            _exerciceRepository.CallApiWithJwtToken(token);
            _trainingExerciceRepository.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_exerciceLogRepository.GetAll().Select(e => new ExerciceLog
            {
                Id = e.Id,
                Reps = e.Reps,
                Weight = e.Weight,
                Distance = e.Distance,
                Time = e.Time,
                Comment = e.Comment,
                Id_training_log = e.Id_training_log,
                Id_exercice = e.Id_exercice,
                Exercice = Mappers.ToASP(_exerciceRepository.GetById(e.Id_exercice)),
            }));
        }
    }
}
