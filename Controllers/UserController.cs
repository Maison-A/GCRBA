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
                                return RedirectToAction("Home","Index");
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
                u.isMember = 0;
                u.isAdmin = 0;

                // make sure none of the fields are empty
                if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.Username.Length == 0
                       || u.Password.Length == 0)
                {
                    // empty field(s), access action type on view to display relevant error message
                    u.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                    return View(u);
                }
             // send data if valid to db
                else
                {
                    // submit new user button pressed
                    if (col["btnSubmit"].ToString() == "newuser")
                    {
                        // initialize action type
                        Models.User.ActionTypes at = Models.User.ActionTypes.NoType;

                        // create database object 
                        Database db = new Database();

                        // save action type based on what Save() returns 
                        at = u.Save();

                        switch (at)
                        {
                            // insert successful
                            // save user session so they are logged in 
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
        
        public ActionResult AddNewMember()
        {     
            Models.User u = new Models.User();
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
                
                // check if not authenticated - create new user
                if (user.UID == 0)
                {
                    user.FirstName = col["Firstname"];
                    user.LastName = col["Lastname"];
                    user.Email = col["Email"];
                    user.Address = col["Address"];
                    user.City = col["City"];
                    user.State = col["State"];
                    user.Zip = col["Zip"];
                    user.MemberShipType = col[""];
                    user.isMember = 1;
                    user.isAdmin = 0;
                    
                }
               
                // once submit is hit, process member data
                if (col["btnSubmit"].ToString() == "submit")
                {
                    //validate data
                    if (user.FirstName.Length == 0 || user.LastName.Length == 0 || user.Email.Length == 0 ||
                        user.Address.Length == 0 || user.City.Length == 0 || user.State.Length == 0 ||
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

                        // save action type based on what Save() returns 
                        at = user.Save();

                        switch (at)
                        {
                            // insert successful
                            // save user session so they are logged in
                            // redirect to interface based on member/nonmember
                            case Models.User.ActionTypes.InsertSuccessful:
                                
                                user.SaveUserSession();
                                // check to see if user is a member or not 
                                db.IsUserMember(user);

                                if (user.isMember == 0)
                                {
                                    return RedirectToAction("NonMember", "Profile");
                                }
                                else
                                {
                                    return RedirectToAction("Member", "Profile");
                                }

                            default:
                                return View(user);
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
