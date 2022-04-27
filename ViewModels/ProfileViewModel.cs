using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.ViewModels
{
	public class ProfileViewModel
	{
		public User User { get; set; }
		public State State { get; set; }
		public List<State> States { get; set; }
		public Company Company { get; set; }
		public List<Company> Companies { get; set; }
		public SocialMedia SocialMediaLink { get; set; }
		public List<SocialMedia> SocialMediaLinks { get; set; }
		public Website Website { get; set; }
		public List<Website> Websites { get; set; }
		public Button Button { get; set; }
	}
}