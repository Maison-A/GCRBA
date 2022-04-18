using System.Web.Optimization;

namespace GCRBA {
	public class BundleConfig {
		public static void RegisterBundles(BundleCollection bundles) {
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/modernizr-{version}.js",
						"~/Scripts/jquery.filedrop.js",
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/gcrba.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/site.css"));
		}
	}
}
