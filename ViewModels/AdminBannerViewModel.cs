using GCRBA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.ViewModels
{
    public class AdminBannerViewModel
    {
        public User CurrentUser { get; set; }
        public MainBanner MainBanner { get; set; }
        public List<MainBanner> MainBanners { get; set; }
    }
}