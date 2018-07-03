using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet
{
    public partial class Login : System.Web.UI.Page
    {
        static string connection_string = "";
        SqlConnection connect = new SqlConnection(connection_string);
        protected void Page_Load(object sender, EventArgs e)
        {
            Configuration config = new Configuration();
            connection_string = config.getConnectionString();
            connect = new SqlConnection(connection_string);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Replace("'", "''");            
            string password = txtPassword.Text;
            check(username, password);
        }
        protected Boolean checkEndDate(string username)
        {
            Boolean correctDate = true;
            //connect.Open();
            //SqlCommand cmd = connect.CreateCommand();
            //cmd.CommandText = "select loginId from Login where username like '" + username + "' ";
            //string loginId = cmd.ExecuteScalar().ToString();
            ////The end_date value might be null, the below checks for null to avoid Database errors.
            //cmd.CommandText = "select count(end_date) from [VSS].[dbo].[User] where loginId = " + loginId + " ";
            //int countEnd_date = Convert.ToInt32(cmd.ExecuteScalar());
            //if (countEnd_date > 0)
            //{
            //    cmd.CommandText = "select end_date from [VSS].[dbo].[User] where loginId = " + loginId + " ";
            //    string end_date = cmd.ExecuteScalar().ToString();
            //    DateTime dt_end_date = DateTime.Parse(end_date);
            //    DateTime today = DateTime.Now;
            //    if (dt_end_date != DateTime.MinValue)
            //    {
            //        if (dt_end_date <= today)
            //            correctDate = false;
            //    }
            //}
            //connect.Close();
            return correctDate;
        }
        protected void check(string username, string password)
        {
            string errorMessage = "Value Error: Make sure you have entered the correct username and password.";
            int flag = 1;// flag 1 means everything is good.
            flag = checkIfEmpty();
            if (flag == 1)//if input is correct.
            {
                Boolean exists = checkIfExists(username); //check if username in DB.
                if (exists)//if user exists in the DB.
                {
                    Boolean correctPassword = checkPassword(username, password); //check if password is correct.
                    //Boolean correctEndDate = checkEndDate(username);
                    //if (correctPassword && correctEndDate)
                    if (correctPassword)
                    {
                        string roleId = findRole(username); //find the roleId.                        
                        success(username, roleId);
                    }
                    else if (!correctPassword)//if password incorrect, display the same message for security reasons.
                    {
                        lblError.Visible = true;
                        lblError.Text = errorMessage;
                    }
                    //else if (!correctEndDate)
                    //{
                    //    lblError.Visible = true;
                    //    lblError.Text = "Your account (" + username + ") has been disabled by the system administrator. Please, contact the support the restore your account.";
                    //}
                }
                else // if user does not exist in DB.
                {
                    lblError.Visible = true;
                    lblError.Text = errorMessage;
                }

            }
        }
        protected int checkIfEmpty()
        {
            int flag = 1;
            if (string.IsNullOrWhiteSpace(txtUsername.Text))//if user leaves blank.
            {
                flag = 0;
                lblUsernameError.Visible = true;
                lblUsernameError.Text = "Input Error: Type a username.";
            }
            else
            {
                lblUsernameError.Visible = false;
                lblUsernameError.Text = "";//clear text in case for another try username is filled but password not filled.
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                flag = 0;
                lblPasswordError.Visible = true;
                lblPasswordError.Text = "Input Error: Type a password.";
            }
            else
            {
                lblPasswordError.Visible = false;
                lblPasswordError.Text = "";
            }
            return flag;
        }
        protected Boolean checkIfExists(string username)
        {
            Boolean exists = true;
            //connect.Open();
            //SqlCommand cmd = connect.CreateCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "select count(*) from login where username like '" + username + "' ";
            //int countUser = Convert.ToInt32(cmd.ExecuteScalar());
            //if (countUser < 1) //user does not exist.
            //{
            //    exists = false;
            //    //lblError.Visible = true;
            //    //lblError.Text = username+" does not exist";
            //}

            //connect.Close();
            return exists;
        }
        protected Boolean checkPassword(string username, string password)
        {
            Boolean correct = true;
            string hashed = Encryption.hash(password);
            password = "";            
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select count (*) from logins where login_username like '" + username + "' and login_password like '" + hashed + "' ";
            int correctCombination = Convert.ToInt32(cmd.ExecuteScalar()); //count matching. result either 0 or 1.
            if (correctCombination == 0)
            {
                correct = false;
            }
            connect.Close();
            return correct;
        }

        protected string findRole(string username)
        {
            string roleId = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select count(roleId) from logins where login_username like '" + username + "'";
            int isThereRoleInDB = Convert.ToInt32(cmd.ExecuteScalar());
            if (isThereRoleInDB > 0) //means the user has a stored roleId in DB. This is to prevent DB error.
            {
                cmd.CommandText = "select roleId from logins where login_username like '" + username + "'";
                roleId = cmd.ExecuteScalar().ToString();
            }
            else //DB error: roleId was not stored for user. It is an extreme case, but it can happen somehow.
            {
                lblError.Visible = true;
                lblError.Text = "Database Error: Username has no role. Please contact the support.";
            }
            connect.Close();
            return roleId;
        }
        protected bool checkIfUserHasSecurityQuestions()
        {
            bool hasThem = true;
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select loginId from Logins where login_username like '" + txtUsername.Text+"' ";
            string loginId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select count(*) from SecurityQuestions where loginId = '"+loginId+"' ";
            int questionsCount = Convert.ToInt32(cmd.ExecuteScalar());
            if (questionsCount < 3)
                hasThem = false;
            connect.Close();
            return hasThem;
        }
        protected void success(string username, string roleId)
        {
            Session.Add("username", username);
            Session.Add("roleId", roleId);
            //check if user has three security questions:
            bool hasSecurityQuestions = checkIfUserHasSecurityQuestions();
            //if the user has three security questions, answer them:
            if(hasSecurityQuestions)
                Response.Redirect("~/SecurityQuestions.aspx");
            //if the user doesn't have three security questions, create them:
            else
                Response.Redirect("~/CreateSecurityQuestions.aspx");
            //if (roleId.Equals("1"))
            //{
            //    //Admin.
            //    Response.Redirect("~/Accounts/Admin/Home.aspx");
            //}
            //else if (roleId.Equals("2"))
            //{
            //    //Physician.
            //    Response.Redirect("~/Accounts/Physician/Home.aspx");
            //}
            //else if (roleId.Equals("3"))
            //{
            //    //Patient.
            //    Response.Redirect("~/Accounts/Patient/Home.aspx");
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Go home:
            Response.Redirect("~/");
        }
    }
}