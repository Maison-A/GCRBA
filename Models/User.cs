using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class User
    {
        public int UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
 
        public string Username { get; set; }
        public string Password { get; set; }
        public string PaymentType = string.Empty;
        public ActionTypes ActionType { get; set; } = ActionTypes.NoType;
        
        // member type control
        public int isAdmin { get; set; }
        public int isMember { get; set; }
        public string MemberShipType = string.Empty;
        

        // tells us if current user is logged in 
        public bool IsAuthenticated
        {
            get
            {
                if (UID > 0) return true;
                return false;
            }
        }

        public User.ActionTypes Save()
        {
            try
            {
                Database db = new Database();
                if (UID == 0)
                { //insert new user
                    this.ActionType = db.InsertUser(this);
                }
                // TODO: handle duplicate sign up creds
                else
                {
                    this.ActionType = db.UpdateUser(this);
                }
                return this.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

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

        public bool GetAdminStatus()
        {
            // declare variable
            User u = new User();

            try
            {
                // get current user
                u = u.GetUserSession();

                // is user admin?
                if (u.isAdmin == 1)
                {
                    // yes, return true
                    return true;
                }

                // user is not admin, return false
                return false;
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
            LoginFailed = 7,
            
        }
    }


}