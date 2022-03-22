using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GCRBA {
	public class Connect2Python {
		static void Main(string[] args) {
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = "C:\\Users\\winsl\\AppData\\Local\\ESRI\\conda\\envs\\myenv-py3v2\\python.exe";
			Console.WriteLine(args.Length);
			args[0] = "C:\\Users\\winsl\\PycharmProjects\\pythonProject2\\trial.py";

			startInfo.Arguments = string.Format("{0}", args[0]);
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardOutput = true;
			using (Process process = Process.Start(startInfo)) {
				using (StreamReader reader = process.StandardOutput) {
					string result = reader.ReadToEnd();
					Console.Write(result);
				}		
			}
			Console.Read();
		}
	}
}