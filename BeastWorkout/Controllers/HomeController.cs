using BeastWorkout.Models;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeastWorkout.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfilRepositoryDAL _profilRepository;

        public HomeController(ILogger<HomeController> logger, IProfilRepositoryDAL profilRepository)
        {
            _logger = logger;
            _profilRepository = profilRepository;
        }

        public IActionResult Index()
        {
            if (BeastWorkout.Session.SessionHelper.person == null)
            {
                return View("Index"); // Utilisateur non connecté
            }
            else
            {

                return View("LoggedIndex"); // Utilisateur connecté avec un profil
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}