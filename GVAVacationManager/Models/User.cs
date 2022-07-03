using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето не може да е празно")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Полето не може да е празно")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [ForeignKey("Role Id")]
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("Team Id")]
        public int? TeamId { get; set; }
        public Team Team { get; set; }
    }

}
