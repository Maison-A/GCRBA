using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.Models
{
	public class MemberRequest
	{
        public int UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int MemberID { get; set; }
		public string MemberLevel { get; set; }
		public string PaymentType { get; set; }
		public string PaymentStatus { get; set; }
		public MemberRequest.ActionTypes ActionType { get; set; } = ActionTypes.NoType;

        // obtain current session status
        public MemberRequest GetMemberRequestSession()
        {
            try
            {
                // create new instance of MemberRequest object 
                MemberRequest m = new MemberRequest();

                // check if CurrentRequest is null
                if (HttpContext.Current.Session["CurrentRequest"] == null)
                {
                    // if it is null, return blank MemberRequest object 
                    return m;
                }

                // else, assign CurrentRequest info to MemberRequest object and return MemberRequest object 
                m = (MemberRequest)HttpContext.Current.Session["CurrentRequest"];
                return m;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // save current session status 
        public bool SaveMemberRequestSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentRequest"] = this;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // remove current session status 
        public bool RemoveMemberRequestSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentRequest"] = null;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public enum ActionTypes
		{
			NoType = 0,
			InsertSuccessful = 1, 
			UpdateSuccessful = 2, 
			DeleteSuccessful = 3, 
			RequiredFieldMissing = 4, 
			Unknown = 5
		}
	}
}