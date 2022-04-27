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
		public Website.ActionTypes ActionType { get; set; } = ActionTypes.NoType;

        // obtain current session status
        public List<Website> GetWebsitesSession()
        {
            try
            {
                // create new instance of User object 
                List<Website> websites = new List<Website>();

                // check if CurrentUser is null
                if (HttpContext.Current.Session["CurrentWebsites"] == null)
                {
                    // if it is null, return blank user object 
                    return websites;
                }   

                websites = (List<Website>)HttpContext.Current.Session["CurrentWebsites"];
                return websites;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // save current session status 
        public bool SaveWebsitesSession(List<Website> websites)
        {
            try
            {
                HttpContext.Current.Session["CurrentWebsites"] = websites;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // remove current session status 
        public bool RemoveWebsitesSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentWebsites"] = null;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public enum WebsiteTypes {
			MainPage = 1,
			OrderingPage = 2,
			DonationPage = 3,
			NoType = 4
		}

		public enum ActionTypes
		{
			NoType = 1,
			InsertSuccessful = 2,
			UpdateSuccessful = 3, 
			DeleteSuccessful = 4, 
			Unknown = 5
		}
	}
}