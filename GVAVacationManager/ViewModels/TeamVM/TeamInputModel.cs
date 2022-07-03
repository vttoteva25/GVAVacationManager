using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GVAVacationManager.Data;
using GVAVacationManager.Models;

namespace GVAVacationManager.ViewModels.TeamVM
{
    public class TeamInputModel
    {
        public string TeamName { get; set; }

        [Required(ErrorMessage = "Полето не може да е празно")]
        public int ProjectId { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Team> Teams { get; set; }

        [Required(ErrorMessage = "Полето не може да е празно")]
        public int UserId { get; set; }
        public IEnumerable<User> Users { get; set; }

       
    }
}



