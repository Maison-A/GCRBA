using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class Website {
		public long intWebsiteID = 0;
		public long intCompanyID = 0;
		public string strURL = string.Empty;
		public short intWebsiteTypeID = 0;
		public string strWebsiteType = string.Empty;
		public Website.WebsiteTypes WebsiteType = WebsiteTypes.NoType;

		public enum WebsiteTypes {
			MainPage = 1,
			OrderingPage = 2,
			DonationPage = 3,
			NoType = 4
		}
	}
}