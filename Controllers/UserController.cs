﻿using System;
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

                // only using FirstName, LastName, Email, Username, and Password because
                // those are the only ones on the form for a new user 
                u.FirstName = col["FirstName"];
                u.LastName = col["LastName"];
                u.Email = col["Email"];
                u.Username = col["Username"];
                u.Password = col["Password"];

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


        // TODO: bring up how to manage initilization
        // will we be forcing members to become Users? 
        public ActionResult AddNewMember()
        {
            // if user exists: update user value to member
            Models.User u = new Models.User();
            return View(u);

            // if user doesnt exist - redirect to sign up?
        }


        [HttpPost]
        public ActionResult AddNewMember(FormCollection col)
        {
            if (col["btnSignUp"].ToString() == "submit")
            {
                //validate data

                // send data if valid to db

                // return to member page - use generated user id as 
                // param
                return RedirectToAction("Index", "Member");
            }
            return View();
        }
    }
}
