using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class MailModel {
		public string To = "gcrbadata@gmail.com";

		public string From {
			get;
			set;
		}
		public string Subject {
			get;
			set;
		}
		public string Body {
			get;
			set;
		} 

	}
}