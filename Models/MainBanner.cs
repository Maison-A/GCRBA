using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class MainBanner
    {
        public int BannerID { get; set; }
		public string Banner { get; set; }
		public ActionTypes ActionType { get; set; } = ActionTypes.NoType;
    }


	public enum ActionTypes
	{
		NoType = 0,
		InsertSuccessful = 1,
		UpdateSuccessful = 2,
		DuplicateEmail = 3,
		DuplicateUserID = 4,
		Unknown = 5,
		RequiredFieldsMissing = 6,
		LoginFailed = 7
	}

}