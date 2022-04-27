using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Models
{
    public class LandingLocationList
    {
        public List<Models.NewLocation> lstLandingLocations = new List<Models.NewLocation>();
        public int[] SelectedLandingLocationRequests { get; set; }
        public IEnumerable<SelectListItem> LandingLocations { get; set; }
    }
}