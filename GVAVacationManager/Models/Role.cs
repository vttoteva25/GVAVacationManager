using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Models
{
    public class Role
    {

        [Key]
        public int Id { get; set; }

        [DisplayName("Role name")]
        [Required(ErrorMessage = "Полето не може да е празно")]
        public string RoleName { get; set; }
    }
}
