using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GVAVacationManager.Models;
using Utf8Json;

namespace GVAVacationManager.ViewModels
{
	public class CEOeditUserViewModel
	{
		[Required(ErrorMessage = "Потребителското име не може да е празно")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Името не може да е празно")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Фамилията не може да е празна")]
		public string LastName { get; set; }

		public int? TeamId { get; set; }

		public int? RoleId { get; set; }

		public IEnumerable<Role> Roles { get; set; }
		public IEnumerable<Team> Teams { get; set; }



	}
}
