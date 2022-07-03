using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GVAVacationManager.Data;
using GVAVacationManager.Models;

namespace GVAVacationManager.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Project> projectList = _db.Projects;
            return View(projectList);
            
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                if(_db.Projects.Any(x => x.ProjectName == project.ProjectName))
                {
                    throw new ArgumentException("A project with the same name already exists");
                }
                else
                {
                    _db.Projects.Add(project);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }              
            }
            return View(project);
        }
    }
}
