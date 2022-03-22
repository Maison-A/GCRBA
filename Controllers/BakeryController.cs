using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GCRBA.Views.Bakery {
    public class BakeryController : Controller {
        private Process firstProcess;
        private Process secondProcess;
        private Process thirdProcess;

        // GET: Bakery
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection col) {
            if (col["btnSubmit"].ToString() == "addLocation") {
                return RedirectToAction("AddNewLocation");
            }
            else return View();
        }

        public ActionResult AddNewLocation() {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewLocation(FormCollection col) {
            if (col["btnSubmit"].ToString() == "NewLocation") {
                using(firstProcess = new Process()) {
                    firstProcess.StartInfo.FileName = "C:\\Users\\winsl\\AppData\\Local\\ESRI\\conda\\envs\\myenv-py3v2\\python.exe";
                    firstProcess.StartInfo.CreateNoWindow = true;
                    firstProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\Python\\Scripts\\client_machine.py";
                    firstProcess.Start();
                    firstProcess.WaitForExit();
                }
                using (secondProcess = new Process()) {
                    secondProcess.StartInfo.FileName = "C:\\Users\\winsl\\AppData\\Local\\ESRI\\conda\\envs\\myenv-py3v2\\python.exe";
                    secondProcess.StartInfo.CreateNoWindow = true;
                    secondProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\Python\\Scripts\\ZipFiles.py";
                    secondProcess.Start();
                    secondProcess.WaitForExit();
                }
                using (thirdProcess = new Process()) {
                    thirdProcess.StartInfo.FileName = "C:\\Users\\winsl\\AppData\\Local\\ESRI\\conda\\envs\\myenv-py3v2\\python.exe";
                    thirdProcess.StartInfo.CreateNoWindow = true;
                    thirdProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\Python\\Scripts\\connect2ArcgisOnline.py";
                    thirdProcess.Start();
                    thirdProcess.WaitForExit();
                }
            }
            return RedirectToAction("Index");
		}
    }
}