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
    }
}