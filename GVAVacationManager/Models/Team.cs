using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Team name")]
        [Required(ErrorMessage = "Полето не може да е празно")]
        public string TeamName { get; set; }

        [ForeignKey("Project Id")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        List<User>? users = new List<User>();      

    }
}
