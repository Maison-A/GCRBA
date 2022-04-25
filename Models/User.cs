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
        public short intState { get; set; }
        public List<Models.State> lstStates = new List<Models.State>();
        public string Zip { get; set; }
        public string Phone { get; set; }
        public Models.PhoneNumber userPhone;
        public string Email { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string PaymentType = string.Empty;
        public short intPaymentType = 0;
        public short intMembershipType = 0;
        public short intMemberID = 0;
        public ActionTypes ActionType { get; set; } = ActionTypes.NoType;

        // member type control
        public int isAdmin { get; set; }
        public int isMember { get; set; }
        public string MemberShipType = string.Empty;

        public List<NewLocation> lstMemberLocations = new List<NewLocation>();
        public bool myLocation { get; set; }


        // tells us if current user is logged in 
        public bool IsAuthenticated
        {
            get
            {
                if (UID > 0) return true;
                return false;
            }
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
        public User NonAdminLogin()
        {
            try
            {
                Database db = new Database();
                return db.NonAdminLogin(this);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public User AdminLogin()
        {
            try
            {
                Database db = new Database();
                return db.AdminLogin(this);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public User.ActionTypes Save()
        {
            try
            {
                // create database object 
                Database db = new Database();

                // if current user object UID is 0
                if (UID == 0)
                {
                    // call method to insert user to database
                    this.ActionType = db.AddNewUser(this);
                }

                // return status of insert (success, duplicate email, duplicate username, unknown)
                return this.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public enum ActionTypes
        {
            NoType = 0,
            InsertSuccessful = 1,
            UpdateSuccessful = 2,
            DuplicateEmail = 3,
            DuplicateUsername = 4,
            Unknown = 5,
            RequiredFieldMissing = 6,
            LoginFailed = 7,

        }
    }


}