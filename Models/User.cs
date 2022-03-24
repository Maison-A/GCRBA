using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace GCRBA.Models
{

	public class User
	{
		public long UID = 0;
		public string FirstName = string.Empty;
		public string LastName = string.Empty;
		// membership type can be set to a certain value if creating account and not a member
		public string MemberShipType = string.Empty;
		public string UserName = string.Empty;
		public string Password = string.Empty;
		public string Email = string.Empty;
		public ActionTypes ActionType = ActionTypes.NoType;

		// Determines if user is logged in - a "read only property procedure"
		public bool IsAuthenticated
		{
			// get is what makes read only - this property is critical to determining if logged in
			get
			{
				// once we log in the uid changes from 0
				if (UID > 0) return true;
				return false;
			}
		}

	
		// a method to return user object  
		public User Login()
		{
			try
			{
				Database db = new Database();
				return db.Login(this);
			}
			// error trap
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// will return action type
		public User.ActionTypes Save()
		{
			try
			{
				Database db = new Database();
				if (UID == 0)
				{ //insert new user
					this.ActionType = db.InsertUser(this);
				}
				else
				{
					this.ActionType = db.UpdateUser(this);
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

		public User GetUserSession()
		{
			try
			{
				User u = new User();
				if (HttpContext.Current.Session["CurrentUser"] == null)
				{
					return u;
				}
				u = (User)HttpContext.Current.Session["CurrentUser"];
				return u;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool SaveUserSession()
		{
			try
			{
				HttpContext.Current.Session["CurrentUser"] = this;
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		// enum gives a numerical value an English name so we can understand what it means
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