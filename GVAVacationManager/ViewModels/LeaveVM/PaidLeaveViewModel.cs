using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GVAVacationManager.Models;

namespace GVAVacationManager.ViewModels.LeaveVM
{
    public class PaidLeaveViewModel
    {
        [Required(ErrorMessage = "Полето не може да бъде празно!")]
        public DateTime From { get; set; }
        [Required(ErrorMessage = "Полето не може да бъде празно!")]
        public DateTime To { get; set; }
        [Required]
        public bool HalfDayLeave { get; set; }

        public IEnumerable<Leave> Leaves { get; set; }
    }
}
