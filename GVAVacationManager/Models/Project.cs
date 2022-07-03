using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Project name")]
        [Required]
        public string ProjectName { get; set; }
        
    }
}
