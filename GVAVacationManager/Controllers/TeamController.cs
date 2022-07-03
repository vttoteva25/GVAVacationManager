using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using GVAVacationManager.Data;
using GVAVacationManager.Models;
using GVAVacationManager.ViewModels.TeamVM;

namespace GVAVacationManager.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TeamController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
           
            IEnumerable<Team> teamList = _db.Teams;
            return View(teamList);

        }

        public IActionResult Create()
        {
            TeamInputModel model = new TeamInputModel();
            model.Projects = _db.Projects;
            model.Users = _db.Users.Where(x => x.RoleId !=1 && x.RoleId != 2);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]     
        public IActionResult Create(TeamInputModel model)
        {
            Team team = new Team();
            User user = new User();
            if (ModelState.IsValid)
            {                               
                team.TeamName = model.TeamName;
                team.ProjectId = model.ProjectId;
                user = _db.Users.FirstOrDefault(x => x.Id == model.UserId);
                

                if (_db.Teams.Any(x => x.TeamName == team.TeamName))
                {
                    throw new ArgumentException("A team with the same name already exists");
                }
                if(_db.Teams.Any(x => x.ProjectId == team.ProjectId))
                {
                    throw new ArgumentException("There is another team working on this project");
                }
                else
                {                    
                    _db.Teams.Add(team);
                    _db.SaveChanges();

                    UpdateUser(team, user);
                    return RedirectToAction("Index");
                }
            }
            return View(team);
        }

        private void UpdateUser(Team team, User user)
        {
            user.RoleId = 2;
            user.TeamId = team.Id;

            _db.Users.Update(user);
            _db.SaveChanges();         
        }

        [HttpGet]
        [Route("team/open/{id}")]
        public IActionResult Open([FromRoute] int id)
        {
            Team team = _db.Teams.FirstOrDefault(x => x.Id == id);

            if (team is null)
            {
                return NotFound();
            }
            
            dynamic model = new ExpandoObject();

            model.Team = team;
            model.TeamName = team.TeamName;

            return View(model);
        }
    }
}

