using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.ViewModels
{
	public class MemberVM
	{
		public User User { get; set; }
		public State State { get; set; }
		public List<State> States { get; set; }
	}
}