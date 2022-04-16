using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.ViewModels
{
    public class UserCompanyVM
    {
        public User User { get; set; }
        public Company Company { get; set; }
        public List<Company> Companies { get; set; }
    }
} 