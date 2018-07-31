﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Physician
{
    public partial class ChangeSecurityQuestions : System.Web.UI.Page
    {
        string username = "", roleId = "", loginId = "", token = "";
        Configuration config = new Configuration();
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool correctInput = checkInputs();
            if (correctInput)
            {
                store();
                ShowSuccessMessage();
            }
        }
        protected void ShowSuccessMessage()
        {
            lblSuccess.Visible = true;
            lblSuccess.Text = "You have successfully changed your Security questions and answers!";
            btnSubmit.Visible = false;
        }
        protected bool checkIfUsersInitialLogin()
        {
            bool initial = true;            
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select login_initial from Logins where loginId = '" + loginId + "' ";
            int initialValue = Convert.ToInt32(cmd.ExecuteScalar());
            if (initialValue == 0)
                initial = false;
            connect.Close();
            return initial;
        }
        protected void store()
        {
            txtQ1.Text = txtQ1.Text.Replace("'", "''");
            txtQ2.Text = txtQ2.Text.Replace("'", "''");
            txtQ3.Text = txtQ3.Text.Replace("'", "''");
            txtA1.Text = txtA1.Text.Replace("'", "''");
            txtA2.Text = txtA2.Text.Replace("'", "''");
            txtA3.Text = txtA3.Text.Replace("'", "''");            
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select loginId from Logins where login_username like '"+username+"' ";
            string temp_loginId = cmd.ExecuteScalar().ToString();
            //g_loginId = loginId;
            //Insert questions:
            cmd.CommandText = "insert into Questions (question_text) values ('"+txtQ1.Text+"') ";
            cmd.ExecuteScalar();
            cmd.CommandText = "insert into Questions (question_text) values ('" + txtQ2.Text + "') ";
            cmd.ExecuteScalar();
            cmd.CommandText = "insert into Questions (question_text) values ('" + txtQ3.Text + "') ";
            cmd.ExecuteScalar();
            //Get questions' IDs:
            cmd.CommandText = "select questionId from questions where question_text like '"+txtQ1.Text+"' ";
            string q1Id = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select questionId from questions where question_text like '" + txtQ2.Text + "' ";
            string q2Id = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select questionId from questions where question_text like '" + txtQ3.Text + "' ";
            string q3Id = cmd.ExecuteScalar().ToString();
            //Insert answers with their questions' IDs:
            cmd.CommandText = "insert into SecurityQuestions (loginId, questionId, securityQuestion_answer) values " +
                "('"+ temp_loginId + "', '"+q1Id+"', '"+txtA1.Text+"')";
            cmd.ExecuteScalar();
            cmd.CommandText = "insert into SecurityQuestions (loginId, questionId, securityQuestion_answer) values " +
                "('" + temp_loginId + "', '" + q2Id + "', '" + txtA2.Text + "')";
            cmd.ExecuteScalar();
            cmd.CommandText = "insert into SecurityQuestions (loginId, questionId, securityQuestion_answer) values " +
                "('" + temp_loginId + "', '" + q3Id + "', '" + txtA3.Text + "')";
            cmd.ExecuteScalar();
            connect.Close();
        }
        protected bool checkInputs()
        {
            bool correct = true;
            CheckErrors errors = new CheckErrors();
            string q1, q2, q3, a1, a2, a3;
            if(!errors.validQuestion(txtQ1.Text, out q1))
            {
                correct = false;
                lblQ1Error.Visible = true;
                lblQ1Error.Text = q1;
                txtQ1.Focus();
            }
            if (!errors.validQuestion(txtQ2.Text, out q2))
            {
                correct = false;
                lblQ2Error.Visible = true;
                lblQ2Error.Text = q2;
                txtQ2.Focus();
            }
            if (!errors.validQuestion(txtQ3.Text, out q3))
            {
                correct = false;
                lblQ3Error.Visible = true;
                lblQ3Error.Text = q3;
                txtQ3.Focus();
            }
            if (!errors.validAnswer(txtA1.Text, out a1))
            {
                correct = false;
                lblA1Error.Visible = true;
                lblA1Error.Text = a1;
                txtA1.Focus();
            }
            if (!errors.validAnswer(txtA2.Text, out a2))
            {
                correct = false;
                lblA2Error.Visible = true;
                lblA2Error.Text = a2;
                txtA2.Focus();
            }
            if (!errors.validAnswer(txtA3.Text, out a3))
            {
                correct = false;
                lblA3Error.Visible = true;
                lblA3Error.Text = a3;
                txtA3.Focus();
            }
            return correct;
        }
        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            hideAllErrors();
            txtQ1.Text = "";
            txtA1.Text = "";
            txtQ2.Text = "";
            txtA2.Text = "";
            txtQ3.Text = "";
            txtA3.Text = "";
        }
        protected void hideAllErrors()
        {
            lblQ1Error.Visible = false;
            lblA1Error.Visible = false;
            lblQ2Error.Visible = false;
            lblA2Error.Visible = false;
            lblQ3Error.Visible = false;
            lblA3Error.Visible = false;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Account");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Configuration config = new Configuration();
            conn = config.getConnectionString();
            connect = new SqlConnection(conn);
            getSession();
            CheckPhysicianSession session = new CheckPhysicianSession();
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
            Session.Add("loginId", token);
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