using GCRBA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class Company
    {
        public int CompanyID = 0;
        public string Name = string.Empty;
        public string About = string.Empty;
        public string Year = string.Empty;
        public ActionTypes ActionType = ActionTypes.NoType;

        public enum ActionTypes
        {
            NoType = 0, 
            InsertSuccessful = 1,
            UpdateSuccessful = 2,
            DuplicateName = 3,
            Unknown = 4,
            RequiredFieldMissing = 5
        }

        public Company.ActionTypes Save()
        {
            try
            {
                Database db = new Database();
                this.ActionType = db.InsertNewCompany(this);
                return this.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}