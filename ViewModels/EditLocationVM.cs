using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.ViewModels
{
    public class EditLocationVM
    {
        public User CurrentUser { get; set; }
        public Company CurrentCompany { get; set; }
        public List<Company> Companies { get; set; }
    }
}