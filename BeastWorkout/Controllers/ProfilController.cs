using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class ProfilController : Controller
    {
        private readonly IProfilRepositoryDAL _profilRepository;
        private readonly IPersonRepositoryDAL _personRepository;
        private readonly IPerformanceRepositoryDAL _performanceRepository;
        private readonly IExerciceRepositoryDAL _exerciceRepository;

        public ProfilController(IProfilRepositoryDAL profilRepository, IPersonRepositoryDAL personRepository, IPerformanceRepositoryDAL performanceRepository, IExerciceRepositoryDAL exerciceRepository)
        {
            _profilRepository = profilRepository;
            _personRepository = personRepository;
            _performanceRepository = performanceRepository;
            _exerciceRepository = exerciceRepository;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _profilRepository.CallApiWithJwtToken(token);
            _personRepository.CallApiWithJwtToken(token);
            _performanceRepository.CallApiWithJwtToken(token);
            _exerciceRepository.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_profilRepository.GetAll().Select(p => new Profil
            {
                Id = p.Id,
                Age = p.Age,
                Height = p.Height,
                Weight = p.Weight,
                Total_xp = p.Total_xp,
                Id_person = p.Id_person,
                Person = Mappers.ToASP(_personRepository.GetById(p.Id_person)),
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var listP = _personRepository.GetAll();
            ViewBag.List = new SelectList(listP, "Id", "Firstname", "Lastname");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Profil p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            if (p.Total_xp == null)
            {
                p.Total_xp = 0;
            }

            _profilRepository.Create(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Profil p = Mappers.ToASP(_profilRepository.GetById(id));
            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(Profil p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            _profilRepository.Update(Mappers.ToDAL(p));
            return RedirectToAction("MyProfil", new { id = p.Id_person });
        }


        // Pour l'admin
        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_profilRepository.GetById(id)));
        }

        // Pour tout le monde quand on clique sur mon profil
        public IActionResult MyProfil(int id)
        {
            Profil profil = Mappers.ToASP(_profilRepository.GetByIdPerson(id));
            IEnumerable<Performance> listPerf = _performanceRepository.GetAll().Where(p => p.Id_profil == id).Select(p => new Performance
            {
                Id = p.Id,
                Description = p.Description,
                Value = p.Value,
                Date = p.Date,
                Id_profil = p.Id_profil,
                Id_exercice = p.Id_exercice,
                Profil = Mappers.ToASP(_profilRepository.GetById(p.Id_profil)),
                Exercice = Mappers.ToASP(_exerciceRepository.GetById(p.Id_exercice)),
            }).OrderByDescending(p => p.Date);
            ViewBag.ListPerf = listPerf;

            if (profil.Id == 0)
            {
                return RedirectToAction("CreateMyProfil", new { id });
            }
            return View(profil);
        }

        // Si l'utilisateur n'a pas encore de profil, il arrive sur la viex CreateMyProfil

        [HttpGet]
        public IActionResult CreateMyProfil(int id)
        {
            var person = _personRepository.GetById(id);
            if (person == null)
            {
                return RedirectToAction("Error");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateMyProfil(Profil p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            if (p.Total_xp == null)
            {
                p.Total_xp = 0;
            }

            p.Id_person = Session.SessionHelper.person.Id;
            p.Id = 0;  // Si je n'initialise pas p.Id, l'Id prend la valeur d'Id_person donc ne fonctionne pas
            _profilRepository.Create(Mappers.ToDAL(p));
            return RedirectToAction("MyProfil", new { id = p.Id_person });
        }

        /**
         * 
         * PERFORMANCE
         * 
         */

        [HttpGet]
        public IActionResult CreateMyPerformance(int id_profil, int? id_exercice)
        {
            var profil = _profilRepository.GetById(id_profil);
            if (profil == null)
            {
                return RedirectToAction("Error");
            }

            var listE = _exerciceRepository.GetAll();

            ViewBag.ListExercice = new SelectList(listE, "Id", "Name");
            ViewBag.Id_profil = id_profil;

            if (id_exercice.HasValue)
            {
                ViewBag.Id_exercice = id_exercice;

                Exercice e = Mappers.ToASP(_exerciceRepository.GetById(id_exercice.Value));
                ViewBag.Exercice = e;
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateMyPerformance(Performance p, int id_exercice)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            int id_person = Session.SessionHelper.person.Id;

            p.Date = DateTime.Now;
            p.Id_exercice = id_exercice;
            _performanceRepository.Create(Mappers.ToDAL(p));
            return RedirectToAction("MyProfil", new { id = id_person });
        }

        [HttpGet]
        public IActionResult EditPerformance(int id)
        {
            Performance p = Mappers.ToASP(_performanceRepository.GetById(id));

            Exercice e = Mappers.ToASP(_exerciceRepository.GetById(p.Id_exercice));
            ViewBag.Exercice = e;

            return View(p);
        }

        [HttpPost]
        public IActionResult EditPerformance(Performance p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }
            p.Date = DateTime.Now;
            _performanceRepository.Update(Mappers.ToDAL(p));

            int id_person = Session.SessionHelper.person.Id;
            return RedirectToAction("MyProfil", new { id = id_person });
        }

        public IActionResult DeletePerformance(Performance p)
        {
            _performanceRepository.Delete(Mappers.ToDAL(p));

            int id_person = Session.SessionHelper.person.Id;
            return RedirectToAction("MyProfil", new { id = id_person });
        }
    }
}
