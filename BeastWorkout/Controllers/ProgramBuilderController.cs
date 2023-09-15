using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BeastWorkout.Controllers
{
    public class ProgramBuilderController : Controller
    {
        private readonly IPersonProgramRepositoryDAL _personProgramRepositoryDAL;
        private readonly IPersonRepositoryDAL _personRepositoryDAL;
        private readonly IProgramRepositoryDAL _programRepositoryDAL;
        private readonly IProgramTrainingRepositoryDAL _programTrainingRepositoryDAL;
        private readonly ITrainingRepositoryDAL _trainingRepositoryDAL;
        private readonly IExerciceRepositoryDAL _exerciceRepositoryDAL;
        private readonly ITrainingExerciceRepositoryDAL _trainingExerciceRepositoryDAL;
        private readonly ITypeProgramRepositoryDAL _typeProgramRepositoryDAL;

        public ProgramBuilderController(IPersonProgramRepositoryDAL personProgramRepositoryDAL, IPersonRepositoryDAL personRepositoryDAL, IProgramRepositoryDAL programRepositoryDAL, IProgramTrainingRepositoryDAL programTrainingRepositoryDAL, ITrainingRepositoryDAL trainingRepositoryDAL, IExerciceRepositoryDAL exerciceRepositoryDAL, ITrainingExerciceRepositoryDAL trainingExerciceRepositoryDAL, ITypeProgramRepositoryDAL typeProgramRepositoryDAL)
        {
            _personProgramRepositoryDAL = personProgramRepositoryDAL;
            _personRepositoryDAL = personRepositoryDAL;
            _programRepositoryDAL = programRepositoryDAL;
            _programTrainingRepositoryDAL = programTrainingRepositoryDAL;
            _trainingRepositoryDAL = trainingRepositoryDAL;
            _exerciceRepositoryDAL = exerciceRepositoryDAL;
            _trainingExerciceRepositoryDAL = trainingExerciceRepositoryDAL;
            _typeProgramRepositoryDAL = typeProgramRepositoryDAL;
        }

        //--------------------- METHODE QUI VERIFIE LE TOKEN JWT ---------------------
        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _personProgramRepositoryDAL.CallApiWithJwtToken(token);
            _personRepositoryDAL.CallApiWithJwtToken(token);
            _programRepositoryDAL.CallApiWithJwtToken(token);
            _trainingRepositoryDAL.CallApiWithJwtToken(token);
            _programTrainingRepositoryDAL.CallApiWithJwtToken(token);
            _exerciceRepositoryDAL.CallApiWithJwtToken(token);
            _trainingExerciceRepositoryDAL.CallApiWithJwtToken(token);
            _typeProgramRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        /**
         * 
         * PERSON_PROGRAM
         * 
         */
        //--------------------- AFFICHER LES PROGRAMMES PERSOS DES UTILISATEURS ---------------------
        public IActionResult MyPrograms(int id)
        {
            Person person = Mappers.ToASP(_personRepositoryDAL.GetById(id));

            return View(_personProgramRepositoryDAL.GetByIdPerson(id).Select(p => new PersonProgram
            {
                Id_person = p.Id_person,
                Id_program = p.Id_program,
                Person = person,
                Program = Mappers.ToASP(_programRepositoryDAL.GetById(p.Id_program)),
            }));
        }

        //--------------------- AFFICHER LES TYPES DE PROGRAMME  ---------------------
        public IActionResult GetTypeProgram(int? id)
        {
            if (id.HasValue)
            {
                Person person = Mappers.ToASP(_personRepositoryDAL.GetById(id.Value));
                ViewBag.Person = person;
            }
            return View(_typeProgramRepositoryDAL.GetAll().Select(t => Mappers.ToASP(t)));
        }

        //--------------------- AJOUTER UN NOUVEAU PROGRAMME POUR UNE PERSONNE + AFFICHAGE DES PROGRAMMES ---------------------
        [HttpGet]
        public IActionResult AddMyNewProgram(int id_type_program)
        {
            IEnumerable<Models.Program> listProgram = _programRepositoryDAL.GetAll().Where(p => p.Id_type_program == id_type_program).Select(p => Mappers.ToASP(p));
            ViewBag.List = new SelectList(listProgram, "Id", "Name");

            List<PersonProgram> listPersonProgram = new List<PersonProgram>();

            foreach (var program in listProgram)
            {
                var programTrainings = _programTrainingRepositoryDAL.GetByIdProgram(program.Id).Select(p => new ProgramTraining
                {
                    Id_program = p.Id_program,
                    Id_training = p.Id_training,
                    Program = program,
                    Training = Mappers.ToASP(_trainingRepositoryDAL.GetById(p.Id_training)),
                    Exercices = _trainingExerciceRepositoryDAL.GetByIdTraining(p.Id_training).Select(e => new TrainingExercice
                    {
                        Id_training = e.Id_training,
                        Id_exercice = e.Id_exercice,
                        Serie = e.Serie,
                        Reps = e.Reps,
                        Rest = e.Rest,
                        Weight = e.Weight,
                        Rpe = e.Rpe,
                        Distance = e.Distance,
                        Time = e.Time,
                        Training = Mappers.ToASP(_trainingRepositoryDAL.GetById(e.Id_training)),
                        Exercice = Mappers.ToASP(_exerciceRepositoryDAL.GetById(e.Id_exercice)),
                    }).ToList()
                }).ToList();

                listPersonProgram.Add(new PersonProgram
                {
                    Id_program = program.Id,
                    Program = program,
                    ProgramTrainings = programTrainings
                });
            }

            return View(listPersonProgram);
        }

        [HttpPost]
        public IActionResult AddMyNewProgram(PersonProgram p, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            p.Id_person = id;
            _personProgramRepositoryDAL.Create(Mappers.ToDAL(p));

            return RedirectToAction("MyPrograms", new { id = id });
        }

        //--------------------- CREATE UN PROGRAM ET L'ASSIGNE DIRECTEMENT A LA PERSON QUI LE CREE ---------------------
        [HttpGet]
        public IActionResult CreateMyProgram()
        {
            var listTP = _typeProgramRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listTP, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateMyProgram(Models.Program program, int id_person)
        {
            if (!ModelState.IsValid)
            {
                return View(program);
            }

            _programRepositoryDAL.Create(Mappers.ToDAL(program));

            Models.Program newP = Mappers.ToASP(_programRepositoryDAL.GetLastProgramCreated());

            PersonProgram pp = new PersonProgram();
            id_person = Session.SessionHelper.person.Id;
            pp.Id_person = id_person;
            pp.Id_program = newP.Id;

            _personProgramRepositoryDAL.Create(Mappers.ToDAL(pp));

            return RedirectToAction("MyTrainings", new { id = newP.Id });
        }

        //--------------------- DELETE UN PERSON PROGRAM ---------------------
        public IActionResult DeleteProgram(int id_person, int id_program)
        {
            PersonProgram p = Mappers.ToASP(_personProgramRepositoryDAL.GetById(id_person, id_program));
            _personProgramRepositoryDAL.Delete(Mappers.ToDAL(p));
            return RedirectToAction("MyPrograms", new { id = id_person });
        }

        /**
         * 
         * PROGRAM_TRAINING
         * 
         */
        //--------------------- AFFICHER LES TRAININGS PRESENTS DANS LE PROGRAM ---------------------

        public IActionResult MyTrainings(int id)
        {
            Models.Program program = Mappers.ToASP(_programRepositoryDAL.GetById(id));
            ViewBag.Program = program;

            var programTrainings = _programTrainingRepositoryDAL.GetByIdProgram(id).Select(p => new ProgramTraining
            {
                Id_program = p.Id_program,
                Id_training = p.Id_training,
                Program = program,
                Training = Mappers.ToASP(_trainingRepositoryDAL.GetById(p.Id_training)),
            }).ToList();

            // Récupérer les exercices pour chaque training du programme
            foreach (var pt in programTrainings)
            {
                var exercices = _trainingExerciceRepositoryDAL.GetByIdTraining(pt.Id_training).Select(p => new TrainingExercice
                {
                    Id_training = p.Id_training,
                    Id_exercice = p.Id_exercice,
                    Serie = p.Serie,
                    Reps = p.Reps,
                    Rest = p.Rest,
                    Weight = p.Weight,
                    Rpe = p.Rpe,
                    Distance = p.Distance,
                    Time = p.Time,
                    Cpt = p.Cpt,
                    Training = Mappers.ToASP(_trainingRepositoryDAL.GetById(p.Id_training)),
                    Exercice = Mappers.ToASP(_exerciceRepositoryDAL.GetById(p.Id_exercice)),
                }).OrderBy(e => e.Cpt);
                pt.Exercices = exercices;
            }

            return View(programTrainings);
        }



        //--------------------- AJOUTER UN NOUVEAU TRAINING POUR UN PROGRAMME ---------------------
        [HttpGet]
        public IActionResult AddNewTraining()
        {
            var listT = _trainingRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listT, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddNewTraining(ProgramTraining t, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }
            t.Id_program = id;
            _programTrainingRepositoryDAL.Create(Mappers.ToDAL(t));

            return RedirectToAction("MyTrainings", new { id });
        }

        //--------------------- CREATE UN TRAINING ET L'ASSIGNE DIRECTEMENT A LA PERSON QUI LE CREE ---------------------
        [HttpGet]
        public IActionResult CreateMyTraining()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMyTraining(Training training, int id_program)
        {
            if (!ModelState.IsValid)
            {
                return View(training);
            }
            _trainingRepositoryDAL.Create(Mappers.ToDAL(training));

            Training newT = Mappers.ToASP(_trainingRepositoryDAL.GetLastTraininCreated());

            ProgramTraining pt = new ProgramTraining();

            Models.Program lastP = Mappers.ToASP(_programRepositoryDAL.GetLastProgramCreated());

            pt.Id_program = lastP.Id;
            pt.Id_training = newT.Id;

            _programTrainingRepositoryDAL.Create(Mappers.ToDAL(pt));

            return RedirectToAction("MyTrainings", new { id = lastP.Id });
        }

        //--------------------- DELETE PROGRAM TRAINING ---------------------
        public IActionResult DeleteTraining(int id_program, int id_training)
        {
            ProgramTraining p = Mappers.ToASP(_programTrainingRepositoryDAL.GetById(id_program, id_training));
            _programTrainingRepositoryDAL.Delete(Mappers.ToDAL(p));
            return RedirectToAction("MyTrainings", new { id = id_program });
        }

        /**
         * 
         * TRAINING_EXERCICE
         * 
         */
        //--------------------- AFFICHER LES EXERCICES PRESENTS DANS LE TRAINING ---------------------
        public IActionResult MyExercices(int id)
        {
            Training training = Mappers.ToASP(_trainingRepositoryDAL.GetById(id));
            ViewBag.Training = training;

            return View(_trainingExerciceRepositoryDAL.GetByIdTraining(id).Select(p => new TrainingExercice
            {
                Id_training = p.Id_training,
                Id_exercice = p.Id_exercice,
                Serie = p.Serie,
                Reps = p.Reps,
                Rest = p.Rest,
                Weight = p.Weight,
                Rpe = p.Rpe,
                Distance = p.Distance,
                Time = p.Time,
                Cpt = p.Cpt,
                Training = training,
                Exercice = Mappers.ToASP(_exerciceRepositoryDAL.GetById(p.Id_exercice)),
            }).OrderBy(e => e.Cpt));
        }

        //--------------------- AJOUTER UN NOUVEAU EXERCICE POUR UN TRAINING ---------------------
        [HttpGet]
        public IActionResult AddNewExercice(int id_training, int? id_exercice)
        {
            ViewBag.Id_training = id_training;

            if (id_exercice.HasValue)
            {
                ViewBag.Id_exercice = id_exercice;

                Exercice e = Mappers.ToASP(_exerciceRepositoryDAL.GetById(id_exercice.Value));
                ViewBag.Exercice = e;
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddNewExercice(TrainingExercice t, int id_training, int id_exercice)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            t.Id_training = id_training;
            t.Id_exercice = id_exercice;
            _trainingExerciceRepositoryDAL.Create(Mappers.ToDAL(t));
            return RedirectToAction("MyExercices", new { id = id_training });
        }

        //--------------------- UPDATE TRAINING EXERCICE ---------------------
        [HttpGet]
        public IActionResult EditExercice(int id_training, int id_exercice)
        {
            TrainingExercice t = Mappers.ToASP(_trainingExerciceRepositoryDAL.GetById(id_training, id_exercice));
            return View(t);
        }

        [HttpPost]
        public IActionResult EditExercice(TrainingExercice t)
        {
            if (!ModelState.IsValid)
            {
                return View(t);
            }

            _trainingExerciceRepositoryDAL.Update(Mappers.ToDAL(t));
            return RedirectToAction("MyExercices", new { id = t.Id_training });
        }

        //--------------------- DELETE TRAINING EXERCICE ---------------------

        public IActionResult DeleteExercice(int id_training, int id_exercice)
        {
            TrainingExercice t = Mappers.ToASP(_trainingExerciceRepositoryDAL.GetById(id_training, id_exercice));
            _trainingExerciceRepositoryDAL.Delete(Mappers.ToDAL(t));

            return RedirectToAction("MyExercices", new { id = t.Id_training });
        }
    }
}
