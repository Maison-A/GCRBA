using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace GCRBA.Models {
	public class MailModel {
		public string Subject = "New Location To Add";
		//[DisplayName("User Name")]
		public string UserName = "GCRBAWebApp@donotreply"; //{ get; set; }
		public string Title = "Testing";// { get; set; }
		public string Url = "Testing.com"; // { get; set; }
		public string Description = "Testing"; //{ get; set; }
		public string Recipient = "gcrbadata@gmail.com";
		public Models.NewLocation Content { get; set; }
	}
}