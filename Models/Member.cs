using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace GCRBA.Models
{

	public class Member
	{
		public long UID = 0;
		public string FirstName = string.Empty;
		public string LastName = string.Empty;
		public string MemberShipType = string.Empty;
		public string UserName = string.Empty;
		public string UserID = string.Empty;
		public string Password = string.Empty;
		public string Email = string.Empty;
		public ActionTypes ActionType = ActionTypes.NoType;



		// will return action type
		public Member.ActionTypes Save()
		{
			try
			{
				Database db = new Database();
				if (UID == 0)
				{ //insert new user
					this.ActionType = db.InsertMember(this);
				}
				else
				{
					this.ActionType = db.InsertMember(this);
				}
				return this.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool RemoveUserSession()
		{
			try
			{
				HttpContext.Current.Session["CurrentUser"] = null;
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Member GetUserSession()
		{
			try
			{
				Member m = new Member();
				if (HttpContext.Current.Session["CurrentMember"] == null)
				{
					return m;
				}
				m = (Member)HttpContext.Current.Session["CurrentMember"];
				return m;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool SaveMemberSession()
		{
			try
			{
				HttpContext.Current.Session["CurrentMember"] = this;
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
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
}


