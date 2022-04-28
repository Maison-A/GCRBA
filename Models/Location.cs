using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public int CompanyID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int intState { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<State> States { get; set; }
        public Location.ActionTypes ActionType = ActionTypes.NoType;

        // obtain current session status
        public Location GetLocationSession() {
            try {
                // create new instance of User object 
                Location loc = new Location();

                // check if CurrentUser is null
                if (HttpContext.Current.Session["CurrentLocation"] == null) {
                    // if it is null, return blank user object 
                    return loc;
                }

                // else, assign CurrentUser info to user object and return User object 
                loc = (Location)HttpContext.Current.Session["CurrentLocation"];
                return loc;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // save current session status 
        public bool SaveLocationSession() {
            try {
                HttpContext.Current.Session["CurrentLocation"] = this;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // remove current session status 
        public bool RemoveLocationSession() {
            try {
                HttpContext.Current.Session["CurrentLocation"] = null;
                return true;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public enum ActionTypes
        {
            NoType = 1,
            InsertSuccessful = 2,
            UpdateSuccessful = 3,
            DeleteSuccessful = 4,
            Unknown = 5,
            RequiredFieldMissing = 6
        }
    }
}