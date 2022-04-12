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
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Location.ActionTypes ActionType = ActionTypes.NoType;

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