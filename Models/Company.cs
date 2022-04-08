using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class Company
    {
        public int intCompanyID { get; set; }
        public string strCompanyName { get; set; }
        public string About = string.Empty;
        public string Year = string.Empty;

    }
}