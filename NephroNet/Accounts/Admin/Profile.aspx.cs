﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class Profile : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        string profileId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            profileId = Request.QueryString["id"];
            bool userIdExists = isUserCorrect();
            if (!userIdExists)
                goHome();
            showInformation();
        }
        protected void showInformation()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Get the current user's ID who is trying to access the profile:
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string current_userId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select loginId from users where userId = '"+profileId+"' ";
            string account_loginId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select login_isActive from Logins where loginId = '"+account_loginId+"' ";
            int isActive = Convert.ToInt32(cmd.ExecuteScalar());
            connect.Close();
            ShortProfile shortProfile = new ShortProfile(profileId, current_userId);
            string name = shortProfile.name;
            //do the same for the rest of information...
            //...
            //Display the information:
            lblShortProfileInformation.Text = name;
            string terminateCommand = "<br/><button id='terminate_button'type='button' onmousedown=\"OpenPopup('TerminateAccount.aspx?id=" + profileId + "')\">Terminate Account</button>";
            string unlockCommand = "<br/><button id='unlock_button'type='button' onmousedown=\"OpenPopup('UnlockAccount.aspx?id=" + profileId + "')\">Unlock Account</button>";
            if(isActive == 1 && account_loginId != loginId)
                lblShortProfileInformation.Text += terminateCommand;
            else if(isActive == 0 && account_loginId != loginId)
                lblShortProfileInformation.Text += unlockCommand;
        }
        protected bool isUserCorrect()
        {
            bool correct = true;
            CheckErrors errors = new CheckErrors();
            //check if id contains a special character:
            if (!errors.isDigit(profileId))
                correct = false;
            //check if id contains an id that does not exist in DB:
            else if (errors.ContainsSpecialChars(profileId))
                correct = false;
            if (correct)
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                //Count the existance of the user:
                cmd.CommandText = "select count(*) from Users where userId = '" + profileId + "' ";
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)//if count > 0, then the user ID exists in DB.
                {
                    //Get the current user's ID who is trying to access the profile:
                    cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
                    string current_userId = cmd.ExecuteScalar().ToString();
                    //Maybe later use the current user's ID to check if the current user has access to view the selected profile.
                }
                else
                    correct = false; // means that the user ID does not exists in DB.
                connect.Close();
            }
            return correct;
        }
        protected void goHome()
        {
            Response.Redirect("Home.aspx");
        }
        protected void initialPageAccess()
        {
            Configuration config = new Configuration();
            conn = config.getConnectionString();
            connect = new SqlConnection(conn);
            getSession();
            CheckAdminSession session = new CheckAdminSession();
            bool correctSession = session.sessionIsCorrect(username, roleId, token);
            if (!correctSession)
                clearSession();
        }
        protected void clearSession()
        {

            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/");
        }
        protected void addSession()
        {
            Session.Add("username", username);
            Session.Add("roleId", roleId);
            Session.Add("loginId", loginId);
            Session.Add("token", token);
        }
        protected void getSession()
        {
            username = (string)(Session["username"]);
            roleId = (string)(Session["roleId"]);
            loginId = (string)(Session["loginId"]);
            token = (string)(Session["token"]);
        }
    }
}