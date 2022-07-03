using GVAVacationManager.Data;
using GVAVacationManager.HelpingTools;
using GVAVacationManager.Models;
using GVAVacationManager.ViewModels.LeaveVM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Controllers
{
    public class UnpaidLeaveController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UnpaidLeaveController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult IndexUnpaidLeave()
        {
            dynamic model = new ExpandoObject();

            model.Users = _db.Users;
            model.UnpaidLeaves = _db.Leaves.Where(x => x.TypeLeave == "Неплатен");
            return View(model);

        }

        public IActionResult TeamUnpaidLeaves()
        {
            dynamic model = new ExpandoObject();

            model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
            Team team = model.Team;
            model.UnpaidLeaves = _db.Leaves.Where(x => x.TypeLeave == "Неплатен");
            model.Users = _db.Users.Where(x => x.TeamId == team.Id);
            return View(model);
        }

        public IActionResult CreateUnpaidLeave()
        {
            UnpaidLeaveViewModel model = new UnpaidLeaveViewModel();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("unpaidleave/createunpaidleave/{username}")]
        public IActionResult CreateUnpaidLeave(UnpaidLeaveViewModel model, [FromRoute] string username)
        {
            Leave leave = new Leave();
            if (ModelState.IsValid)
            {
                if (model.To < model.From || model.From < DateTime.Today)
                {
                    throw new ArgumentException("");
                }

                if (Logged.User.Id == 1)
                {
                    leave.IsApprovedByTeamLead = true;
                }
                else
                {
                    leave.IsApprovedByTeamLead = false;
                }

                leave.From = model.From;
                leave.To = model.To;
                leave.UserId = _db.Users.FirstOrDefault(x => x.Username == username).Id;
                leave.User = _db.Users.FirstOrDefault(x => x.Username == username);
                leave.IsApproved = false;
                leave.TypeLeave = "Неплатен";
                leave.CreationDate = DateTime.Now;
                leave.HalfDayLeave = model.HalfDayLeave;

                if (leave.HalfDayLeave == true && (leave.From != leave.To))
                {
                    throw new ArgumentException("You cannot take halfday leave for more than 1 day!");
                }
                else
                {
                    _db.Leaves.Add(leave);
                    _db.SaveChanges();
                    /// Важнооооо!!!
                    return Redirect($"~/User/Profile/{username}");
                }
            }
            return View(leave);
        }

        public IActionResult UserUnpaidLeaves()
        {
            IEnumerable<Leave> unpaidLeavesList = _db.Leaves.Where(x => x.TypeLeave == "Неплатен");
            return View(unpaidLeavesList);
        }
    }
}
