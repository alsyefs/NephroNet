﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string username = "", roleId = "", loginId = "", token = "";
        Configuration config = new Configuration();
        protected void Page_Load(object sender, EventArgs e)
        {
            getSession();
            if (!IsPostBack)
            {
                CheckSession session = new CheckSession();
                bool correctSession = session.sessionIsCorrect(username, roleId, token);
                if (!correctSession)
                    clearSession();
                else
                {
                    updateToken();
                }
            }
        }
        protected void updateToken()
        {
            //Generate a Token:
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            token = Convert.ToBase64String(time.Concat(key).ToArray());
            token = token.Replace("'", "''");
            //Store the token in DB:
            SqlConnection connect = new SqlConnection(config.getConnectionString());
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "update logins set login_token = '" + token + "' where loginId = '" + loginId + "' ";
            cmd.ExecuteScalar();
            connect.Close();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool correctInput = checkInputs();
            if (correctInput)
            {
                store();
                goHome();
            }
        }
        protected void goHome()
        {
            updateToken();
            addSession();
            if (roleId.Equals("1"))
            {
                //Admin.
                Response.Redirect("~/Accounts/Admin/Home.aspx");
            }
            else if (roleId.Equals("2"))
            {
                //Physician.
                Response.Redirect("~/Accounts/Physician/Home.aspx");
            }
            else if (roleId.Equals("3"))
            {
                //Patient.
                Response.Redirect("~/Accounts/Patient/Home.aspx");
            }
        }
        protected void store()
        {
            //txtP1.Text = txtP1.Text.Replace("'", "''");
            //txtP2.Text = txtP2.Text.Replace("'", "''");            
            SqlConnection connect = new SqlConnection(config.getConnectionString());
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select loginId from Logins where login_username like '" + username + "' ";
            string temp_loginId = cmd.ExecuteScalar().ToString();
            //Hash the new password:
            //Encryption encrypt = new Encryption();
            string hashedPassword = Encryption.hash(txtP1.Text);
            //Update new password:
            cmd.CommandText = "update Logins set login_password = '"+hashedPassword+"' where loginId = '"+loginId+"' ";
            cmd.ExecuteScalar();
            //Set the status as NOT the initial login:
            cmd.CommandText = "update Logins set login_initial = 0 where loginId = '" + loginId + "' ";
            cmd.ExecuteScalar();
            connect.Close();
        }
        protected bool checkInputs()
        {
            bool correct = true;
            CheckErrors errors = new CheckErrors();
            string p1, p2, p3;
            if (!errors.validPassword(txtP1.Text, out p1))
            {
                correct = false;
                lblP1Error.Visible = true;
                lblP1Error.Text = p1;
                txtP1.Focus();
            }
            if (!errors.validPassword(txtP2.Text, out p2))
            {
                correct = false;
                lblP2Error.Visible = true;
                lblP2Error.Text = p2;
                txtP2.Focus();
            }
            if (!errors.passwordsMatch(txtP1.Text, txtP2.Text, out p3))
            {
                correct = false;
                lblError.Visible = true;
                lblError.Text = p3;
                txtP1.Focus();
            }
            return correct;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
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