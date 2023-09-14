using BeastWorkout.Models;
using BeastWorkout.Tools;
using DAL.Entities;
using DAL.Repositories;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BeastWorkout.Controllers
{
    public class PersonProgramController : Controller
    {
        private readonly IPersonProgramRepositoryDAL _personProgramRepositoryDAL;
        private readonly IPersonRepositoryDAL _personRepositoryDAL;
        private readonly IProgramRepositoryDAL _programRepositoryDAL;

        public PersonProgramController(IPersonProgramRepositoryDAL personProgramRepositoryDAL, IPersonRepositoryDAL personRepositoryDAL, IProgramRepositoryDAL programRepositoryDAL)
        {
            _personProgramRepositoryDAL = personProgramRepositoryDAL;
            _personRepositoryDAL = personRepositoryDAL;
            _programRepositoryDAL = programRepositoryDAL;
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
        }

        //--------------------- ASSURE LE TOKEN EST VERIFIE AVANT CHAQUE APPEL D'UNE METHODE DU CONTROLLER ---------------------
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckJwtToken();
        }


        //--------------------- AFFICHER TOUS LES PROGRAMMES TRIES PAR UTILISATEUR ---------------------

        public IActionResult Index()
        {
            if (BeastWorkout.Session.SessionHelper.person == null || BeastWorkout.Session.SessionHelper.person.Id_type_person != 1)
            {
                return RedirectToAction("Unauthorized", "Error");
            }
            return View(_personProgramRepositoryDAL.GetAll().Select(p => new PersonProgram
            {
                Id_person = p.Id_person,
                Id_program = p.Id_program,
                Person = Mappers.ToASP(_personRepositoryDAL.GetById(p.Id_person)),
                Program = Mappers.ToASP(_programRepositoryDAL.GetById(p.Id_program)),
            }).OrderBy(p => p.Id_person));
        }

        //--------------------- AJOUTER UN NOUVEAU PROGRAMME POUR UN UTILISATEUR ---------------------

        [HttpGet]
        public IActionResult Create()
        {
            var listPE = _personRepositoryDAL.GetAll();
            var listPR = _programRepositoryDAL.GetAll();
            ViewBag.ListPER = new SelectList(listPE, "Id", "Lastname", "Firstname");
            ViewBag.ListPRO = new SelectList(listPR, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonProgram p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            _personProgramRepositoryDAL.Create(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        //--------------------- DELETE UN PERSON PROGRAM ---------------------
        public IActionResult Delete(int id_person, int id_program)
        {
            PersonProgram p = Mappers.ToASP(_personProgramRepositoryDAL.GetById(id_person, id_program));
            _personProgramRepositoryDAL.Delete(Mappers.ToDAL(p));
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id_person, int id_program)
        {
            return View(Mappers.ToASP(_personProgramRepositoryDAL.GetById(id_person, id_program)));
        }
    }
}
