using GCRBA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class Company
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Year { get; set; }
        public ActionTypes ActionType = ActionTypes.NoType;

        public Company GetCompanySession()
        {
            try
            {
                // create new Company object
                Company c = new Company();

                // check if CurrentCompany is null
                if (HttpContext.Current.Session["CurrentCompany"] == null)
                {
                    // is null, return blank company object
                    return c;
                }

                // not null, assign CurrentCompany info to company object and return object 
                c = (Company)HttpContext.Current.Session["CurrentCompany"];
                return c;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool  SaveCompanySession()
        {
            try
            {
                // save current company session and return true
                HttpContext.Current.Session["CurrentCompany"] = this;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool RemoveCompanySession()
        {
            try
            {
                // set current company session to null and return true
                HttpContext.Current.Session["CurrentCompany"] = null;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public enum ActionTypes
        {
            NoType = 0, 
            InsertSuccessful = 1,
            UpdateSuccessful = 2,
            DeleteSuccessful = 3, 
            DuplicateName = 4, 
            Unknown = 5,
            RequiredFieldMissing = 6
        }

    }
}