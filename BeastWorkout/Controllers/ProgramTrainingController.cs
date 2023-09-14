using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BeastWorkout.Controllers
{
    public class ProgramTrainingController : Controller
    {
        private readonly IProgramTrainingRepositoryDAL _programTrainingRepositoryDAL;
        private readonly IProgramRepositoryDAL _programRepositoryDAL;
        private readonly ITrainingRepositoryDAL _trainingRepositoryDAL;

        public ProgramTrainingController(IProgramTrainingRepositoryDAL programTrainingRepositoryDAL, IProgramRepositoryDAL programRepositoryDAL, ITrainingRepositoryDAL trainingRepositoryDAL)
        {
            _programTrainingRepositoryDAL = programTrainingRepositoryDAL;
            _programRepositoryDAL = programRepositoryDAL;
            _trainingRepositoryDAL = trainingRepositoryDAL;
        }


        //--------------------- METHODE QUI VERIFIE LE TOKEN JWT ---------------------
        private void CheckJwtToken()
        {
            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Le token JWT n'est pas disponible.");
            }
            _programTrainingRepositoryDAL.CallApiWithJwtToken(token);
            _programRepositoryDAL.CallApiWithJwtToken(token);
            _trainingRepositoryDAL.CallApiWithJwtToken(token);
        }

        //--------------------- ASSURE QUE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }

        public IActionResult Index()
        {
            return View(_programTrainingRepositoryDAL.GetAll().Select(p => new ProgramTraining
            {
                Id_program = p.Id_program,
                Id_training = p.Id_training,
                Program = Mappers.ToASP(_programRepositoryDAL.GetById(p.Id_program)),
                Training = Mappers.ToASP(_trainingRepositoryDAL.GetById(p.Id_training)),
            }).OrderBy(p => p.Id_program));
        }

        //--------------------- AJOUTER UN NOUVEAU TRAINING POUR UN PROGRAMME ---------------------
        [HttpGet]
        public IActionResult Create()
        {
            var listP = _programRepositoryDAL.GetAll();
            var listT = _trainingRepositoryDAL.GetAll();
            ViewBag.ListPR = new SelectList(listP, "Id", "Name");
            ViewBag.ListTR = new SelectList(listT, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProgramTraining p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }
            _programTrainingRepositoryDAL.Create(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        //--------------------- DELETE PROGRAM TRAINING ---------------------
        public IActionResult Delete(int id_program, int id_training)
        {
            ProgramTraining p = Mappers.ToASP(_programTrainingRepositoryDAL.GetById(id_program, id_training));
            _programTrainingRepositoryDAL.Delete(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }
    }
}
