using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastWorkout.Controllers
{
    public class ProgramController : Controller
    {
        private readonly IProgramRepositoryDAL _programRepositoryDAL;
        private readonly ITypeProgramRepositoryDAL _typeProgramRepositoryDAL;

        public ProgramController(IProgramRepositoryDAL programRepositoryDAL, ITypeProgramRepositoryDAL typeProgramRepositoryDAL)
        {
            _programRepositoryDAL = programRepositoryDAL;
            _typeProgramRepositoryDAL = typeProgramRepositoryDAL;
        }

        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _programRepositoryDAL.CallApiWithJwtToken(token);
            _typeProgramRepositoryDAL.CallApiWithJwtToken(token);
        }


        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }
        public IActionResult Index()
        {
            IEnumerable<BeastWorkout.Models.Program> programs = _programRepositoryDAL.GetAll()
                .Select(p => new BeastWorkout.Models.Program
                {
                    Id = p.Id,
                    Name = p.Name,
                    Nbtrainingperweek = p.Nbtrainingperweek,
                    Duration = p.Duration,
                    Objectif = p.Objectif,
                    Id_type_program = p.Id_type_program,
                    Is_my_Program = p.Is_my_Program,
                    Created_by = p.Created_by,
                    TypeProgram = Mappers.ToASP(_typeProgramRepositoryDAL.GetById(p.Id_type_program)),
                });

            return View(programs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var listTP = _typeProgramRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listTP, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(BeastWorkout.Models.Program p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            _programRepositoryDAL.Create(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var listTP = _typeProgramRepositoryDAL.GetAll();
            ViewBag.List = new SelectList(listTP, "Id", "Name");

            Models.Program program = Mappers.ToASP(_programRepositoryDAL.GetById(id));
            if (program == null)
            {
                return RedirectToAction("ProgramNotFound", "Error");
            }

            return View(program);
        }

        [HttpPost]
        public IActionResult Edit(Models.Program p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            _programRepositoryDAL.Update(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        public IActionResult Delete(BeastWorkout.Models.Program p)
        {
            _programRepositoryDAL.Delete(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(Mappers.ToASP(_programRepositoryDAL.GetById(id)));
        }


        // Permet de visualiser tous les programmes, et d'en créé de nouveaux et choisir son program
        //[HttpGet]
        //public IActionResult AllPrograms()
        //{
        //    IEnumerable<BeastWorkout.Models.Program> listPrograms = _programRepositoryDAL.GetAll()
        //        .Select(p => new BeastWorkout.Models.Program
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Nbtrainingperweek = p.Nbtrainingperweek,
        //            Duration = p.Duration,
        //            Objectif = p.Objectif,
        //            Id_type_program = p.Id_type_program,
        //            Is_my_Program = p.Is_my_Program,
        //            Created_by = p.Created_by,
        //            TypeProgram = Mappers.ToASP(_typeProgramRepositoryDAL.GetById(p.Id_type_program)),
        //        });
        //}
    }
}
