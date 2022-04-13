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
                // check if checkbox is checked, if so then submit all data and redirect to new member form?
                // or maybe pull a partial view up?
               
                Models.User u = new Models.User();

                u.FirstName = col["FirstName"];
                u.LastName = col["LastName"];
                u.Email = col["Email"];
                u.Username = col["Username"];
                u.Password = col["Password"];
<<<<<<< HEAD
                u.isMember = 0;
                u.isAdmin = 0;
                u.Address = string.Empty;
                u.City = string.Empty;
                u.Zip = string.Empty;
                u.Phone = string.Empty;
                u.MemberShipType = string.Empty;
                u.PaymentType = string.Empty;
=======
>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)

                // make sure fields are filled out
                if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.Username.Length == 0
                       || u.Password.Length == 0)
                {
                    u.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                    return View(u);
                }

<<<<<<< HEAD

                // send data if valid to db
=======
             // send data if valid to db
>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)
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
<<<<<<< HEAD
                    }      
=======
                    }
                    
>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)
                }
            }
            catch (Exception)
            {
                Models.User u = new Models.User();
                return View(u);
            }
        }

       
        // TODO: bring up how to manage initilization
        // will we be forcing members to become Users? 
        public ActionResult AddNewMember()
        {
            // if user exists: update user value to member
            Models.User u = new Models.User();
            return View(u);

            // if user doesnt exist - redirect to sign up?
        }

        //wip
        [HttpPost]
        public ActionResult AddNewMember(FormCollection col)
        {
            try
            {
                // get current user session
                Models.User user = new Models.User();
                user = user.GetUserSession();
<<<<<<< HEAD
                
                if (user.UID == 0)
=======
                // if user data exists - populate within form
                if (user.UID > 0)
                {
                    col[ "Firstname"] = user.FirstName;
                    col["Lastname"] = user.LastName;
                    col["Email"] = user.Email;
                }
                else
>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)
                {
                    // contact info
                    user.FirstName = col["Firstname"];
                    user.LastName = col["Lastname"];
<<<<<<< HEAD
                    user.Address = col["Address"];
                    user.Phone = col["Phone"];
                    user.City = col["City"];
                    user.State = col["State"];
                    user.Zip = col["Zip"];

                    // username/pass setup
                    user.Email = col["Email"];
                    user.Username = col["Username"];
                    user.Password = col["Password"];

                    // membership type
                    user.MemberShipType = col["MemberShipType"];
                    user.PaymentType = col["PaymentType"];

                    //permissions
                    user.isMember = 1;
                    user.isAdmin = 0;

                }
                else 
                {
                    user.isMember = 1;                    
                }
                
=======
                    user.Email = col["Email"];
                }
>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)
                // once submit is hit, process member data
                if (col["btnSubmit"] == "submit")
                {

                    //validate data
<<<<<<< HEAD
                    if (user.FirstName.Length == 0 || user.LastName.Length == 0 || user.Email.Length == 0 ||
                        user.Phone.Length == 0 || user.Address.Length == 0 || user.City.Length == 0 || user.State.Length == 0 ||
                        user.Zip.Length == 0)
=======
                    if (user.FirstName.Length == 0 || user.LastName.Length == 0 || user.Email.Length == 0)
>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)
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
<<<<<<< HEAD
=======

         
      


>>>>>>> parent of c653a21 (Merge branch 'master' of https://github.com/Maison-A/GCRBA)
    }


}
