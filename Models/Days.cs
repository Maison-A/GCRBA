using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class Days {
		public long intLocationHoursID = 0;
		public short intDayID = 0;
		public DateTime dtClosedTime;
		public DateTime dtOpenTime;
		public string strOpenTime = string.Empty;
		public string strClosedTime = string.Empty;
		public string strDay = string.Empty;
		public bool blnOperational = false;
	}
}