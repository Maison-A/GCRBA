using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class MemberMailModel {
		public string Subject = "New Member Request";
		//[DisplayName("User Name")]
		public string UserName = "GCRBAWebApp@donotreply";
		public string UserFullName { get; set; }
		public string UserEmail { get; set; }
		public string UserTelephone { get; set; }
		public string Title = "Return to Admin Portal";// { get; set; }
		public string Url = "http://localhost:62536/Profile/AdminLogin"; // { get; set; }
		public string Description = "Please review this new member request and approve/deny."; //{ get; set; }
		public string Recipient = "gcrbadata@gmail.com";
		public Models.User Content { get; set; }
	}
}