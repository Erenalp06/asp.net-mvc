using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication2.Entities;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _databaseContext;

        public HomeController(ILogger<HomeController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetProjects()
        {
            var projects = _databaseContext.Projects.ToList();

            var projectList = new List<Project>();
            foreach (var project in projects)
            {
                var p = new Project
                {
                    Id = project.Id,
                    ProjectImage = project.ProjectImage,
                    Fullname = project.Fullname,
                    Description = project.Description,
                    ProjectURL = project.ProjectURL

                    // diğer özellikleri buraya ekle
                };
                projectList.Add(p);
            }

            var json = JsonConvert.SerializeObject(projectList);
            return Content(json, "application/json");
        }

        public IActionResult AccessDenied()
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