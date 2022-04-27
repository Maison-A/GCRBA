using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Photo
        public ActionResult Index()
        {
            Models.NewLocation loc = new Models.NewLocation();
            Models.User u = new Models.User();
            u = u.GetUserSession();
            loc.User = u;

            if (loc.User.IsAuthenticated)
            {
                Models.Database db = new Models.Database();
                long lngLocationID = Convert.ToInt64(RouteData.Values["id"]);
                loc = db.GetLandingLocation(1);
                loc.Images = db.GetLocationImages(1);
            }
            return View(loc);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files, FormCollection col)
        {
            if (col["hiddenLocID"] != null)
            {
                long lngLocTest = Convert.ToInt64(col["hiddenLocID"]);
            }
            long lngLocationID = 1;
            //Models.NewLocation loc = new Models.NewLocation();
            foreach (var file in files)
            {
                Models.NewLocation loc = new Models.NewLocation();
                loc.lngLocationID = lngLocationID;
                loc.AddLocationImage(file, loc);
            }
            return Json("file(s) uploaded successfully");
        }
    }
}