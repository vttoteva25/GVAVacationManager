using GVAVacationManager.Data;
using GVAVacationManager.HelpingTools;
using GVAVacationManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Controllers
{
	public class TeamLeadController : Controller
	{

		private readonly ApplicationDbContext _db;

		public TeamLeadController(ApplicationDbContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult TeamUsers()
		{
			if (Logged.User is not null)
			{
				dynamic model = new ExpandoObject();

				model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
				Team team = model.Team;
				model.Users = _db.Users.Where(x => x.TeamId == team.Id);
			
				return View(model);
			}
			return RedirectToAction("Login", "User");
		}
		public IActionResult ApproveLeaves()
		{
			if (Logged.User is not null)
			{
				dynamic model = new ExpandoObject();

				model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
				Team team = model.Team;
				model.Users = _db.Users.Where(x => x.TeamId == team.Id);
				model.Leaves = _db.Leaves.Where(x => !x.IsApprovedByTeamLead);

				return View(model);
			}
			return RedirectToAction("Login", "User");
		}

		[Route("TeamLead/ApproveLeave/{id}")]
		[HttpGet]
		public IActionResult ApproveLeave([FromRoute] int id)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.RoleId == 2)
				{
					if (id == null || id <= 0)
					{
						return NotFound();
					}

					if (_db.Leaves.FirstOrDefault(x => x.Id == id) == null)
					{
						return NotFound();
					}

					Leave leave = _db.Leaves.FirstOrDefault(x => x.Id == id);
					leave.IsApprovedByTeamLead = true;

					_db.Leaves.Update(leave);
					_db.SaveChanges();

					return RedirectToAction("ApproveLeaves", "TeamLead");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}

		[Route("TeamLead/RejectLeave/{id}")]
		[HttpGet]
		public IActionResult RejectLeave([FromRoute] int id)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.RoleId == 2)
				{
					if (id == null || id <= 0)
					{
						return NotFound();
					}

					if (_db.Leaves.FirstOrDefault(x => x.Id == id) == null)
					{
						return NotFound();
					}
					Leave leave = _db.Leaves.FirstOrDefault(x => x.Id == id);

					_db.Leaves.Remove(leave);
					_db.SaveChanges();

					return RedirectToAction("ApproveLeaves", "TeamLead");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}
	}
}
