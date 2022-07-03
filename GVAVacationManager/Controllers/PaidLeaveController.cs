using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GVAVacationManager.Data;
using GVAVacationManager.Models;
using GVAVacationManager.ViewModels.LeaveVM;
using System.Dynamic;
using GVAVacationManager.HelpingTools;

namespace GVAVacationManager.Controllers
{
    public class PaidLeaveController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PaidLeaveController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult IndexPaidLeave()
        {
            dynamic model = new ExpandoObject();

            model.Users = _db.Users;
            model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
            model.PaidLeaves = _db.Leaves.Where(x => x.TypeLeave == "Платен");
            return View(model);

        }

        public IActionResult TeamPaidLeaves()
        {
            dynamic model = new ExpandoObject();

            model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
            Team team = model.Team;
            model.PaidLeaves = _db.Leaves.Where(x => x.TypeLeave == "Платен");
            model.Users = _db.Users.Where(x => x.TeamId == team.Id);
            return View(model);

        }

        public IActionResult CreatePaidLeave()
        {
            PaidLeaveViewModel model = new PaidLeaveViewModel();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("paidleave/createpaidleave/{username}")]
        public IActionResult CreatePaidLeave(PaidLeaveViewModel model, [FromRoute] string username)
        {
            Leave leave = new Leave();
            if (ModelState.IsValid)
            {
                if (model.To < model.From || model.From < DateTime.Today)
                {
                    throw new ArgumentException("");
                }

                leave.From = model.From;
                leave.To = model.To;
                leave.UserId = _db.Users.FirstOrDefault(x => x.Username == username).Id;
                leave.User = _db.Users.FirstOrDefault(x => x.Username == username);
                leave.IsApproved = false;
                leave.TypeLeave = "Платен";
                leave.CreationDate = DateTime.Now;
                leave.HalfDayLeave = model.HalfDayLeave;


                if (Logged.User.Id == 1)
                {
                    leave.IsApprovedByTeamLead = true;
                }
                else
                {
                    leave.IsApprovedByTeamLead = false;
                }

                if (leave.HalfDayLeave == true && (leave.From != leave.To))
                {
                    throw new ArgumentException("You cannot take halfday leave for more than 1 day!");
                }
                else
                {
                    _db.Leaves.Add(leave);
                    _db.SaveChanges();
                    return Redirect($"~/User/Profile/{username}");
                }

            }
            return View(leave);
        }


        public IActionResult UserPaidLeaves()
        {
            IEnumerable<Leave> paidLeavesList = _db.Leaves.Where(x => x.TypeLeave == "Платен");
            return View(paidLeavesList);
        }
    }
}
