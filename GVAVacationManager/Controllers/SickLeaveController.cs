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
    public class SickLeaveController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SickLeaveController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult IndexSickLeave()
        {
            dynamic model = new ExpandoObject();

            model.Users = _db.Users;
            model.SickLeaves = _db.Leaves.Where(x => x.TypeLeave == "Болничен");
            return View(model);
        }
        public IActionResult TeamSickLeaves()
        {
            dynamic model = new ExpandoObject();

            model.Team = _db.Teams.FirstOrDefault(x => x.Id == Logged.User.TeamId);
            Team team = model.Team;
            model.SickLeaves = _db.Leaves.Where(x => x.TypeLeave == "Болничен");
            model.Users = _db.Users.Where(x => x.TeamId == team.Id);
            return View(model);
        }

        public IActionResult CreateSickLeave()
        {
            SickLeaveViewModel model = new SickLeaveViewModel();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("sickleave/createsickleave/{username}")]
        public IActionResult CreateSickLeave(SickLeaveViewModel model, [FromRoute] string username)
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
                leave.TypeLeave = "Болничен";
                leave.CreationDate = DateTime.Now;
                leave.HalfDayLeave = false;
                leave.HospitalLeaveUrl = model.HospitalLeaveUrl;


                if (leave.HospitalLeaveUrl != null)
                {
                    _db.Leaves.Add(leave);
                    _db.SaveChanges();
                    return Redirect($"~/User/Profile/{username}");
                }
                else
                {
                    throw new ArgumentException("You cannot ask for sick leave without hospital documents");
                }
            }
            return View(leave);
        }

        public IActionResult UserSickLeaves()
        {
            IEnumerable<Leave> paidLeavesList = _db.Leaves.Where(x => x.TypeLeave == "Болничен");
            return View(paidLeavesList);

        }
    }
}
