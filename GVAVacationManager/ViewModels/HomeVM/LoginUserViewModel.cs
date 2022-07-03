using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.ViewModels.HomeVM
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Полето за потребителско име не може да е празно")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Полето за парола не може да е празно")]
        public string Password { get; set; }
    }
}
