using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GVAVacationManager.Models
{
	public class Leave
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public DateTime From { get; set; }
		[Required]
		public DateTime To { get; set; }
		[Required]
		public DateTime CreationDate { get; set; }
		public bool HalfDayLeave { get; set; }
		public string TypeLeave { get; set; }
		public bool IsApproved { get; set; }

		[ForeignKey("User Id")]
		public int UserId { get; set; }
		public User User { get; set; }
		public string HospitalLeaveUrl { get; set; }	
		public bool IsApprovedByTeamLead { get; set; }

	}
}
