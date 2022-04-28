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
		public int intPlatformID = 0;
		public bool blnAvailable = false;
		public SocialMedia.ActionTypes ActionType { get; set; } = ActionTypes.NoType;

        public List<SocialMedia> GetSocialMediaListSession()
        {
            try
            {
                // create new instance of User object 
                List<SocialMedia> links = new List<SocialMedia>();

                // check if CurrentUser is null
                if (HttpContext.Current.Session["CurrentLinks"] == null)
                {
                    // if it is null, return blank user object 
                    return links;
                }

                links = (List<SocialMedia>)HttpContext.Current.Session["CurrentLinks"];
                return links;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // save current session status 
        public bool SaveSocialMediaListSession(List<SocialMedia> links)
        {
            try
            {
                HttpContext.Current.Session["CurrentLinks"] = links;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // remove current session status 
        public bool RemoveSocialMediaListSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentLinks"] = null;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
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