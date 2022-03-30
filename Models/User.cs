using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class User
    {
        public int UID = 0;
        public string FirstName = string.Empty;
        public string LastName = string.Empty;
        public string Address = string.Empty;
        public string City = string.Empty;
        public int intStateID = 0;
        public string Zip = string.Empty;
        public string Phone = string.Empty;
        public string Email = string.Empty;
        public string MemberShipType = string.Empty;    
        public string Username = string.Empty;
        public string Password = string.Empty;
        public string PaymentType = string.Empty;
        public int isAdmin = 0;
        public ActionTypes ActionType = ActionTypes.NoType;

        // obtain current session status
        public User GetUserSession()
        {
            try
            {
                // create new instance of User object 
                User user = new User();

                // check if CurrentUser is null
                if (HttpContext.Current.Session["CurrentUser"] == null)
                {
                    // if it is null, return blank user object 
                    return user;
                }

                // else, assign CurrentUser info to user object and return User object 
                user = (User)HttpContext.Current.Session["CurrentUser"];
                return user;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // save current session status 
        public bool SaveUserSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentUser"] = this;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // remove current session status 
        public bool RemoveUserSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentUser"] = null;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // succesful login -  return User object
        // unsuccessful login - return null 
        public User Login()
        {
            try
            {
                Database db = new Database();
                return db.Login(this);
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
            RequiredFieldMissing = 6,
            LoginFailed = 7
            
        }
    }


}