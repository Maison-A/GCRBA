using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class MailModel {
		public string To = "winslow.shane2@gmail.com";

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