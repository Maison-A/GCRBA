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
                loc = db.GetLandingLocation(lngLocationID);
                TempData["landing"] = new Models.NewLocation();
                TempData["landing"] = loc;

                loc.Images = db.GetLocationImages(lngLocationID);
            }
            return View(loc);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files, Models.NewLocation loc)
        {
            Models.NewLocation loc3 = new Models.NewLocation();
            loc3 = (Models.NewLocation)TempData["landing"];
            foreach (var file in files)
            {
                loc.AddLocationImage(file, loc3);
            }
            return Json("file(s) uploaded successfully");
        }
    }
}