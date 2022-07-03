using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using GVAVacationManager.Data;
using GVAVacationManager.HelpingTools;
using GVAVacationManager.Models;
using GVAVacationManager.ViewModels;

namespace GVAVacationManager.Controllers
{
	public class CEOController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CEOController(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public IActionResult Index()
		{
			if (Logged.User is not null)
			{
				dynamic model = new ExpandoObject();
				model.Role = _db.Roles.FirstOrDefault(x => x.Id == Logged.User.RoleId);

				if (Logged.User.Username == "CEO")
					return View(model);
				else return Unauthorized();
			}
			return RedirectToAction("Login", "User");
		}


		public IActionResult Register()
		{
			if (Logged.User is not null)
			{
				if (Logged.User.Username == "CEO")
					return View();
				else return Unauthorized();
			}
			return RedirectToAction("Login", "User");
		}

		[Route("CEO/DeleteTeam/{id}")]
		[HttpGet]
		public IActionResult DeleteTeam([FromRoute] int? id)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.Username == "CEO")
				{
					if (id == null || id == 0)
					{
						return NotFound();
					}

					if (_db.Teams.FirstOrDefault(x => x.Id == id) == null)
					{
						return NotFound();
					}

					Team team = _db.Teams.FirstOrDefault(x => x.Id == id);
					_db.Teams.Remove(team);
					_db.SaveChanges();
					return RedirectToAction("Index", "Team");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}

		[Route("CEO/DeleteProject/{id}")]
		[HttpGet]
		public IActionResult DeleteProject([FromRoute] int? id)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.Username == "CEO")
				{
					if (id == null || id == 0)
					{
						return NotFound();
					}

					if (_db.Projects.FirstOrDefault(x => x.Id == id) == null)
					{
						return NotFound();
					}

					Project project = _db.Projects.FirstOrDefault(x => x.Id == id);
					_db.Projects.Remove(project);
					_db.SaveChanges();
					return RedirectToAction("Index", "Project");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}
		[Route("CEO/DeleteUser/{username}")]
		[HttpGet]
		public IActionResult DeleteUser([FromRoute] string username)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.Username == "CEO")
				{
					if (string.IsNullOrEmpty(username))
					{
						return NotFound();
					}

					if (_db.Users.FirstOrDefault(x => x.Username == username) == null)
					{
						return NotFound();
					}

					User user = _db.Users.FirstOrDefault(x => x.Username == username);
					_db.Users.Remove(user);
					_db.SaveChanges();
					return RedirectToAction("Index", "User");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}

		#region EditUser
		[Route("ceo/edit/{username}")]
		[HttpGet]
		public IActionResult Edit([FromRoute] string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return NotFound();
			}

			User user = _db.Users.FirstOrDefault(x => x.Username == username);

			if (user == null)
			{
				return NotFound();
			}

			CEOeditUserViewModel model = new CEOeditUserViewModel();
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.Username = user.Username;
			model.Roles = _db.Roles;
			model.Teams = _db.Teams;

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CEOeditUserViewModel model)
		{
			if (ModelState.IsValid)
			{


				User updateUser = _db.Users.FirstOrDefault(x => x.Username == model.Username);
				updateUser.RoleId = model.RoleId;
				updateUser.TeamId = model.TeamId;

				if (updateUser is null)
				{
					return NotFound();
				}

				_db.Users.Update(updateUser);
				_db.SaveChanges();



				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}
		#endregion

		#region ApproveLeave
		public IActionResult ApproveLeaves()
		{
			if (Logged.User is not null)
			{
				dynamic model = new ExpandoObject();

				model.Users = _db.Users;
				model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
				model.Leaves = _db.Leaves.Where(x => x.IsApprovedByTeamLead && !x.IsApproved);

				return View(model);
			}
			return RedirectToAction("Login", "User");
		}


		[Route("CEO/ApproveLeave/{id}")]
		[HttpGet]
		public IActionResult ApproveLeave([FromRoute] int id)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.RoleId == 1)
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
					leave.IsApproved = true;

					_db.Leaves.Update(leave);
					_db.SaveChanges();

					return RedirectToAction("ApproveLeaves", "CEO");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}
		#endregion

		#region RejectLeave
		[Route("CEO/RejectLeave/{id}")]
		[HttpGet]
		public IActionResult RejectLeave([FromRoute] int id)
		{
			if (Logged.User is not null)
			{
				if (Logged.User.RoleId == 1)
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

					return RedirectToAction("ApproveLeaves", "CEO");
				}

				else return Unauthorized();
			}
			else
				return RedirectToAction("Login", "User");
		}
		#endregion
	}
}
