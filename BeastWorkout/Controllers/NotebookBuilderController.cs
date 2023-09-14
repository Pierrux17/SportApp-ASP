using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class NotebookBuilderController : Controller
    {
        private readonly ITrainingLogRepositoryDAL _trainingLogRepository;
        private readonly IPersonRepositoryDAL _personRepository;
        private readonly ITrainingRepositoryDAL _trainingRepository;
        private readonly IExerciceLogRepositoryDAL _exerciceLogRepository;
        private readonly IExerciceRepositoryDAL _exerciceRepository;
        private readonly IPersonProgramRepositoryDAL _personProgramRepository;
        private readonly IProgramRepositoryDAL _programRepository;
        private readonly IProgramTrainingRepositoryDAL _programTrainingRepository;
        private readonly ITrainingExerciceRepositoryDAL _trainingExerciceRepository;

        public NotebookBuilderController(ITrainingLogRepositoryDAL trainingLogRepository, IPersonRepositoryDAL personRepository, ITrainingRepositoryDAL trainingRepository, IExerciceLogRepositoryDAL exerciceLogRepository, IExerciceRepositoryDAL exerciceRepository, IPersonProgramRepositoryDAL personProgramRepository, IProgramRepositoryDAL programRepository, IProgramTrainingRepositoryDAL programTrainingRepository, ITrainingExerciceRepositoryDAL trainingExerciceRepository)
        {
            _trainingLogRepository = trainingLogRepository;
            _personRepository = personRepository;
            _trainingRepository = trainingRepository;
            _exerciceLogRepository = exerciceLogRepository;
            _exerciceRepository = exerciceRepository;
            _personProgramRepository = personProgramRepository;
            _programRepository = programRepository;
            _programTrainingRepository = programTrainingRepository;
            _trainingExerciceRepository = trainingExerciceRepository;
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
            _trainingExerciceRepository.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        /**
         * 
         * TRAINING LOG
         * 
         */
        //--------------AFFICHE UNIQUEMENT LA LISTE DE TRAINING UTILISE PAR LA PERSONNE--------------
        public IActionResult MyTrainingLogs(int id)
        {
            Person person = Mappers.ToASP(_personRepository.GetById(id));
            IEnumerable<Models.Program> listProgram = _programRepository.GetAll().Select(p => Mappers.ToASP(p));

            // Affiche les Training Logs par l'id de la personne
            IEnumerable<TrainingLog> trainingLog = _trainingLogRepository.GetByIdPerson(id).Select(t => new TrainingLog
            {
                Id = t.Id,
                Date = t.Date,
                Id_person = person.Id,
                Id_training = t.Id_training,
                Person = person,
                Training = Mappers.ToASP(_trainingRepository.GetById(t.Id_training)),
            }).OrderByDescending(t => t.Id).ToList();

            foreach (var tl in trainingLog)
            {
                foreach (var p in listProgram)
                {
                    //Cherche dans les trainings des programmes et si le training du program est le même que le training de training log, affiche le programme
                    IEnumerable<ProgramTraining> programTraining = _programTrainingRepository.GetByIdProgram(p.Id).Select(p => Mappers.ToASP(p));
                    foreach (var tt in programTraining)
                    {
                        if (tt.Id_training == tl.Id_training)
                        {
                            //Affiche le bon programme et on peut différencier les type de programmes
                            //Si muscu, affiche reps et weight. Et si running, affiche distance et time
                            Models.Program program = Mappers.ToASP(_programRepository.GetById(tt.Id_program));
                            tl.Program = program;
                        }
                    }
                }
                IEnumerable<ExerciceLog> exerciceLog = _exerciceLogRepository.GetByIdTrainingLog(tl.Id).Select(e => new ExerciceLog
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
                });
                tl.ExerciceLogs = exerciceLog;
            }
            return View(trainingLog);
        }


        //--------------CREE UN NOUVEAU TRAININGLOG--------------
        [HttpGet]
        public IActionResult CreateTrainingLog(int id_person)
        {
            // Cherche dans les programmes utilisés par la personne
            IEnumerable<PersonProgram> personPrograms = _personProgramRepository.GetByIdPerson(id_person).Select(p => Mappers.ToASP(p));

            List<SelectListItem> trainingSelectListItems = new List<SelectListItem>();

            foreach (var pp in personPrograms)
            {
                // Cherche dans les formations des programmes et ajoute celles-ci à la liste si elles correspondent
                IEnumerable<ProgramTraining> programTraining = _programTrainingRepository.GetByIdProgram(pp.Id_program).Select(p => Mappers.ToASP(p));

                foreach (var tt in programTraining)
                {
                    var training = _trainingRepository.GetById(tt.Id_training);

                    if (training != null)
                    {
                        var trainingViewModel = Mappers.ToASP(training);
                        trainingSelectListItems.Add(new SelectListItem
                        {
                            Value = trainingViewModel.Id.ToString(),
                            Text = trainingViewModel.Name
                        });
                    }
                }
            }

            ViewBag.List = new SelectList(trainingSelectListItems, "Value", "Text");

            var trainingLog = new TrainingLog { Id_person = id_person };
            return View(trainingLog);
        }


        [HttpPost]
        public IActionResult CreateTrainingLog(TrainingLog t, int id_person)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }
            t.Id_person = id_person;
            t.Date = DateTime.Now;
            _trainingLogRepository.Create(Mappers.ToDAL(t));

            return RedirectToAction("MyTrainingLogs", new { id = id_person });
        }

        //--------------DELETE TRAININGLOG--------------
        public IActionResult DeleteTrainingLog(int id)
        {
            TrainingLog t = Mappers.ToASP(_trainingLogRepository.GetById(id));

            int id_person = t.Id_person;

            _trainingLogRepository.Delete(Mappers.ToDAL(t));
            return RedirectToAction("MyTrainingLogs", new { id = t.Id_person });
        }

        /**
         * 
         * EXERCICE LOG
         * 
         */
        //--------------------- CREE LES EXERCICES LOGS DU TRAINING LOG/ID ---------------------
        [HttpGet]
        public IActionResult CreateExerciceLog(int id_training_log)
        {
            TrainingLog t = Mappers.ToASP(_trainingLogRepository.GetById(id_training_log));

            IEnumerable<TrainingExercice> listE = _trainingExerciceRepository.GetByIdTraining(t.Id_training).Select(p => new TrainingExercice
            {
                Id_training = p.Id_training,
                Id_exercice = p.Id_exercice,
                Exercice = Mappers.ToASP(_exerciceRepository.GetById(p.Id_exercice)),
            });
            ViewBag.ListEx = listE;
            ViewBag.Id_training_log = t.Id;

            return View();
        }

        [HttpPost]
        public IActionResult CreateExerciceLog(int id_training_log, Dictionary<int, ExerciceLog> ExerciceLogs)
        {
            if (ExerciceLogs == null || ExerciceLogs.Count == 0)
            {
                // Gérer le cas où aucun exercice log n'est créé
                return View();
            }

            foreach (var kvp in ExerciceLogs)
            {
                var e = kvp.Value;
                if (ModelState.IsValid)
                {
                    // Utiliser e.Id_exercice et id_training_log pour créer l'exercice_log associé
                    var newExerciceLog = new ExerciceLog
                    {
                        Id_exercice = kvp.Key,
                        Id_training_log = id_training_log,
                        Reps = e.Reps,
                        Weight = e.Weight,
                        Distance = e.Distance,
                        Time = e.Time,
                        Comment = e.Comment
                    };

                    _exerciceLogRepository.Create(Mappers.ToDAL(newExerciceLog));
                }
            }

            // Rediriger vers l'action appropriée, par exemple "MyTrainingLogs"
            return RedirectToAction("MyTrainingLogs", new { id = BeastWorkout.Session.SessionHelper.person.Id });
        }


        //--------------------- MODIFIE LES EXERCICES LOGS DU TRAINING LOG/ID ---------------------
        [HttpGet]
        public IActionResult EditExerciceLog(int id_training_log)
        {
            TrainingLog t = Mappers.ToASP(_trainingLogRepository.GetById(id_training_log));

            IEnumerable<TrainingExercice> listE = _trainingExerciceRepository.GetByIdTraining(t.Id_training).Select(p => new TrainingExercice
            {
                Id_training = p.Id_training,
                Id_exercice = p.Id_exercice,
                Exercice = Mappers.ToASP(_exerciceRepository.GetById(p.Id_exercice)),
            });
            ViewBag.ListEx = listE;
            ViewBag.Id_training_log = t.Id;

            IEnumerable<ExerciceLog> listExerciceLogs = _exerciceLogRepository.GetByIdTrainingLog(id_training_log).Select(e => Mappers.ToASP(e));
            ViewBag.ListExerciceLogs = listExerciceLogs;

            // Pour chaque exercice log existant, crée un dictionnaire avec les valeurs correspondantes
            Dictionary<int, ExerciceLog> exerciceLogValues = new Dictionary<int, ExerciceLog>();
            foreach (var exerciceLog in listExerciceLogs)
            {
                exerciceLogValues[exerciceLog.Id_exercice] = exerciceLog;
            }
            ViewBag.ExerciceLogValues = exerciceLogValues;

            return View();
        }


        [HttpPost]
        public IActionResult EditExerciceLog(int id_training_log, Dictionary<int, ExerciceLog> ExerciceLogs)
        {
            if (ExerciceLogs == null || ExerciceLogs.Count == 0)
            {
                // Gérer le cas où aucun exercice log n'est créé
                return View();
            }

            foreach (var kvp in ExerciceLogs)
            {
                var e = kvp.Value;
                if (ModelState.IsValid)
                {
                    // Si l'exercice log a un ID, c'est une mise à jour
                    if (e.Id != 0)
                    {
                        var existingExerciceLog = _exerciceLogRepository.GetById(e.Id);
                        if (existingExerciceLog != null)
                        {
                            existingExerciceLog.Reps = e.Reps;
                            existingExerciceLog.Weight = e.Weight;
                            existingExerciceLog.Distance = e.Distance;
                            existingExerciceLog.Time = e.Time;
                            existingExerciceLog.Comment = e.Comment;
                            _exerciceLogRepository.Update(existingExerciceLog);
                        }
                    }
                }
            }

            // Rediriger vers l'action appropriée, par exemple "MyTrainingLogs"
            return RedirectToAction("MyTrainingLogs", new { id = BeastWorkout.Session.SessionHelper.person.Id });
        }
    }
}
