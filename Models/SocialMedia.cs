using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class SocialMedia {
		public long intCompanySocialMediaID = 0;
		public Int16 intSocialMediaID = 0;
		public long intCompanyID = 0;
		public string strSocialMediaLink = string.Empty;
		public string strPlatform = string.Empty;
		public bool blnAvailable = false;
	}
}