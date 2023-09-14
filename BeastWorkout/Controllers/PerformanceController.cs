using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly IPerformanceRepositoryDAL _performanceRepository;
        private readonly IProfilRepositoryDAL _profilRepository;
        private readonly IExerciceRepositoryDAL _exerciceRepository;

        public PerformanceController(IPerformanceRepositoryDAL performanceRepository, IProfilRepositoryDAL profilRepository, IExerciceRepositoryDAL exerciceRepository)
        {
            _performanceRepository = performanceRepository;
            _profilRepository = profilRepository;
            _exerciceRepository = exerciceRepository;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _performanceRepository.CallApiWithJwtToken(token);
            _profilRepository.CallApiWithJwtToken(token);
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
            IEnumerable<Performance> listP = _performanceRepository.GetAll().Select(p => new Performance
            {
                Id = p.Id,
                Description = p.Description,
                Value = p.Value,
                Date = p.Date,
                Id_profil = p.Id_profil,
                Id_exercice = p.Id_exercice,
                Profil = Mappers.ToASP(_profilRepository.GetById(p.Id_profil)),
                Exercice = Mappers.ToASP(_exerciceRepository.GetById(p.Id_exercice)),
            });

            return View(listP);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var listP = _profilRepository.GetAll();
            var listE = _exerciceRepository.GetAll();
            ViewBag.ListProfil = new SelectList(listP, "Id", "Id_person");
            ViewBag.ListExercice = new SelectList(listE, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Performance p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            p.Date = DateTime.Now;
            _performanceRepository.Create(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Performance p)
        {
            _performanceRepository.Delete(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }
    }
}
