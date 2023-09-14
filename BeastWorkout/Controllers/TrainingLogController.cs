using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using System.Net;

namespace BeastWorkout.Controllers
{
    public class TrainingLogController : Controller
    {
        private readonly ITrainingLogRepositoryDAL _trainingLogRepository;
        private readonly IPersonRepositoryDAL _personRepository;
        private readonly ITrainingRepositoryDAL _trainingRepository;
        private readonly IExerciceLogRepositoryDAL _exerciceLogRepository;
        private readonly IExerciceRepositoryDAL _exerciceRepository;
        private readonly IPersonProgramRepositoryDAL _personProgramRepository;
        private readonly IProgramRepositoryDAL _programRepository;
        private readonly IProgramTrainingRepositoryDAL _programTrainingRepository;

        public TrainingLogController(ITrainingLogRepositoryDAL trainingLogRepository, IPersonRepositoryDAL personRepository, ITrainingRepositoryDAL trainingRepository, IExerciceLogRepositoryDAL exerciceLogRepository, IExerciceRepositoryDAL exerciceRepository, IPersonProgramRepositoryDAL personProgramRepository, IProgramRepositoryDAL programRepository, IProgramTrainingRepositoryDAL programTrainingRepository)
        {
            _trainingLogRepository = trainingLogRepository;
            _personRepository = personRepository;
            _trainingRepository = trainingRepository;
            _exerciceLogRepository = exerciceLogRepository;
            _exerciceRepository = exerciceRepository;
            _personProgramRepository = personProgramRepository;
            _programRepository = programRepository;
            _programTrainingRepository = programTrainingRepository;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _trainingLogRepository.CallApiWithJwtToken(token);
            _personRepository.CallApiWithJwtToken(token);
            _trainingRepository.CallApiWithJwtToken(token);
            _exerciceLogRepository.CallApiWithJwtToken(token);
            _exerciceRepository.CallApiWithJwtToken(token);
            _personProgramRepository.CallApiWithJwtToken(token);
            _programRepository.CallApiWithJwtToken(token);
            _programTrainingRepository.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }


        public IActionResult Index()
        {
            return View(_trainingLogRepository.GetAll().Select(t => new TrainingLog
            {
                Id = t.Id,
                Date = t.Date,
                Id_person = t.Id_person,
                Id_training = t.Id_training,
                Person = Mappers.ToASP(_personRepository.GetById(t.Id_person)),
                Training = Mappers.ToASP(_trainingRepository.GetById(t.Id_training)),
            }));
        }

        public IActionResult Delete(TrainingLog t)
        {
            _trainingLogRepository.Delete(Mappers.ToDAL(t));
            return RedirectToAction("Index");
        }
    }
}
