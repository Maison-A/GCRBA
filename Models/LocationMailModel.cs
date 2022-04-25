using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace GCRBA.Models {
	public class LocationMailModel {
		public string Subject = "New Location Request";
		//[DisplayName("User Name")]
		public string UserName = "GCRBAWebApp@donotreply";
		public string UserFullName { get; set; }
		public string UserEmail { get; set; }
		public string UserTelephone { get; set; }
		public string Title = "Return to Admin Portal";// { get; set; }
		public string Url = "http://localhost:62536/Profile/AdminLogin"; // { get; set; }
		public string Description = "Please review this new location request and approve/deny."; //{ get; set; }
		public string Recipient = "gcrbadata@gmail.com";
		public Models.LocationList Content { get; set; }
	}
}