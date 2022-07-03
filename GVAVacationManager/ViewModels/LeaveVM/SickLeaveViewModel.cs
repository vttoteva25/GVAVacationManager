using GVAVacationManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.ViewModels.LeaveVM
{
    public class SickLeaveViewModel
    {
        [Required(ErrorMessage = "Полето не може да бъде празно!")]
        public DateTime From { get; set; }
        [Required(ErrorMessage = "Полето не може да бъде празно!")]
        public DateTime To { get; set; }
        [Required(ErrorMessage = "Полето не може да е празно!" +
            "")]
        public string HospitalLeaveUrl { get; set; }
        public IEnumerable<Leave> Leaves { get; set; }
    }
}
