using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.ViewModels
{
    public class EditCompaniesViewModel
    {
        public User CurrentUser { get; set; }
        public Company CurrentCompany { get; set; }
        public List<Company> Companies { get; set; }
        public Location CurrentLocation { get; set; }
        public List<Location> Locations { get; set; }
        public State CurrentState { get; set; }
        public List<State> States { get; set; }
        public NewLocation NewLocation { get; set; }
        public ContactPerson ContactPerson { get; set; }
        public List<ContactPerson> Contacts { get; set; }
    }
}