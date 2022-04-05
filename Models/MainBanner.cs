using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class MainBanner
    {
        public int BannerID = 0;
        public string Banner = string.Empty;
        public ActionTypes ActionType = ActionTypes.NoType;
    }

    public enum ActionTypes
    {
        NoType = 0,
        InsertSuccessful = 1,
        Unknown = 2
    }
}