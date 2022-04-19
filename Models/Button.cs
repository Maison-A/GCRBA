using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
    public class Button
    {
        public string CurrentButton { get; set; }

        public Button GetButtonSession()
        {
            try
            {
                // create new button object 
                Button currentButton = new Button();

                // is CurrentButton null?
                if (HttpContext.Current.Session["CurrentButton"] == null)
                {
                    // yes, return blank object 
                    return currentButton;
                }

                // else, assign current session info to object and return object 
                currentButton = (Button)HttpContext.Current.Session["CurrentButton"];
                return currentButton;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool SaveButtonSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentButton"] = this;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool RemoveButtonSession()
        {
            try
            {
                HttpContext.Current.Session["CurrentButton"] = null;
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}