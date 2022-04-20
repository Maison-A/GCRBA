using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GCRBA.Models;

namespace GCRBA.Controllers
{
    public class UserController : Controller
    {

        /// TODO
        /// - redirecting home after authentication removes home/index within url
        /// - no logout func
        /// - redirecting home after authenticated redirects to login?
        /// tbc..
        /// - MAISON

        public ActionResult Index()
        {
            Models.User u = new Models.User();
            u = u.GetUserSession();

            return View(u);
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            try
            {
                Models.User u = new Models.User();
                u = u.GetUserSession();
                u.FirstName = col["FirstName"];
                u.LastName = col["LastName"];
                u.Email = col["Email"];
                //u.UserID = col["UserID"];
                u.Password = col["Password"];

                if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.Password.Length == 0)
                {
                    u.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                    return View(u);
                }

                else
                {
                    if (col["btnSubmit"] == "newuser")
                    { //sign up button pressed
                        Models.User.ActionTypes at = Models.User.ActionTypes.NoType;
                        at = u.Save();
                        switch (at)
                        {
                            case Models.User.ActionTypes.InsertSuccessful:
                                u.SaveUserSession();
                                // TODO: return to user profile page/index
                                return RedirectToAction("Home", "Index");
                            //break;
                            default:
                                return View(u);
                                //break;
                        }
                    }
                    else
                    {
                        return View(u);
                    }
                }
            }
            catch (Exception)
            {
                Models.User u = new Models.User();
                return View(u);
            }
        }

        public ActionResult AddNewUser()
        {
            Models.User u = new Models.User();
            return View(u);
        }

        [HttpPost]
        public ActionResult AddNewUser(FormCollection col)
        {
            try
            {
                Models.User u = new Models.User();

                // only using FirstName, LastName, Email, Username, and Password because
                // those are the only ones on the form for a new user 
                u.FirstName = col["FirstName"];
                u.LastName = col["LastName"];
                u.Email = col["Email"];
                u.Username = col["Username"];
                u.Password = col["Password"];
                
                // permissions control
                u.isMember = 0;
                u.isAdmin = 0;

                // some blank values to return to db
                u.Address = "NO DATA";
                u.City = "NO DATA";
                u.State = "NO DATA";
                u.Zip = "NO DATA";
                u.Phone = "NO DATA";
                u.MemberShipType = "NO DATA";

                // submit new user button pressed
                if (col["btnSubmit"].ToString() == "newuser")
                {
                    // initialize action type
                    Models.User.ActionTypes at = Models.User.ActionTypes.NoType;
                       
                    // make sure none of the fields are empty
                    if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.Username.Length == 0
                            || u.Password.Length == 0)
                    {
                        // empty field(s), access action type on view to display relevant error message
                        u.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                        return View(u);
                    }

                    // create database object 
                    Database db = new Database();

                    // save action type based on what Save() returns 
                    at = u.Save();

                    switch (at)
                    {
                        // redirect to interface based on member/nonmember
                        case Models.User.ActionTypes.InsertSuccessful:
                            u.SaveUserSession();
                            // check to see if user is a member or not 
                            db.IsUserMember(u);
                            if (u.isMember == 0)
                            {
                                return RedirectToAction("NonMember", "Profile");
                            }
                            else
                            {
                                return RedirectToAction("Member", "Profile");
                            }
                        default:
                            return View(u);
                    }
                }
                if (col["btnSubmit"].ToString() == "home")
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(u);
                
            }
            catch (Exception)
            {
                Models.User u = new Models.User();
                return View(u);
            }
        }

        public ActionResult AddNewMember()
        {
            Models.User u = new Models.User(); 
            Models.Database db = new Models.Database();
            u = u.GetUserSession();
            u.lstStates = db.GetStates();
            return View(u);
        }

        [HttpPost]
        public ActionResult AddNewMember(FormCollection col)
        {
            try
            {
               
                // get current user session
                Models.User user = new Models.User();
                user = user.GetUserSession();


                // contact info
                user.FirstName = col["FirstName"];
                user.LastName = col["LastName"];
                user.Address = col["Address"];
                user.Phone = col["userPhone.AreaCode"] + col["userPhone.Prefix"] + col["userPhone.Suffix"];
                user.City = col["City"];
                user.intState = Convert.ToInt16(col["intState"]);

                //Just doing something quick here... probably should be changed to something more dynamic.
                if (user.intState == 1) user.State = "Indiana";
                else if (user.intState == 2) user.State = "Kentucky";
                else if (user.intState == 3) user.State = "Ohio";                
                user.Zip = col["Zip"];

                // set sign in info if user is new
                if (user.UID == 0)
                {
                    // username/pass setup
                    user.Email = col["Email"];
                    user.Username = col["Username"];
                    user.Password = col["Password"];
                }


                // membership type
                user.MemberShipType = col["MemberShipType"];
                user.PaymentType = col["PaymentType"];

                //permissions
                user.isMember = 1;
                user.isAdmin = 0;

                


                // once submit is hit, process member data
                if (col["btnSubmit"] == "submit")
                {
                    //validate data
                    if (user.FirstName.Length == 0 || user.LastName.Length == 0 || user.Email.Length == 0 ||
                        user.Phone.Length == 0 || user.Address.Length == 0 || user.City.Length == 0 || user.State.Length == 0 ||
                        user.Zip.Length == 0)
                    {
                        // empty field(s), access action type on view to display relevant error message
                        user.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                        return View(user);
                    }

                    // send data if valid to db
                    else
                    {
                        // initialize action type
                        Models.User.ActionTypes at = Models.User.ActionTypes.NoType;
                        
    
                        // create database object 
                        Database db = new Database();

                        // if user is authenticated then update member table
                        if (user.UID > 0)
                        {
                            // update member table
                            db.InsertUserToMember(user);

                            // update user info

                            // send Grace email
// -------------------------------------------------------------------------------------- //
// SHANE
// -------------------------------------------------------------------------------------- //

                        }
                        else
                        {

                            // save action type based on what Save() returns 
                            at = user.Save();

                            switch (at)
                            {
                                // insert successful- redirect as necessary 
                                case Models.User.ActionTypes.InsertSuccessful:

                                    user.SaveUserSession();

                                    return RedirectToAction("Member", "Profile");
                                    

                                default:
                                    return View(user);
                            }
                        
                        }
                    }
                }
 
            }
            catch (Exception)
            {
                Models.User u = new Models.User();
                return View(u);
            }
            return View();
        }



    }
}
