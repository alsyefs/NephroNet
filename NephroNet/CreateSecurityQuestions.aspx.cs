using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet
{
    public partial class CreateSecurityQuestions : System.Web.UI.Page
    {
        string username = "", roleId = "", g_loginId = "";
        Configuration config = new Configuration();        
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
            Session.Add("username", username);
            Session.Add("roleId", roleId);
            Session.Add("loginId", g_loginId);
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
            txtQ1.Text = txtQ1.Text.Replace("'", "''");
            txtQ2.Text = txtQ2.Text.Replace("'", "''");
            txtQ3.Text = txtQ3.Text.Replace("'", "''");
            txtA1.Text = txtA1.Text.Replace("'", "''");
            txtA2.Text = txtA2.Text.Replace("'", "''");
            txtA3.Text = txtA3.Text.Replace("'", "''");
            SqlConnection connect = new SqlConnection(config.getConnectionString());
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select loginId from Logins where login_username like '"+username+"' ";
            string loginId = cmd.ExecuteScalar().ToString();
            g_loginId = loginId;
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
                "('"+loginId+"', '"+q1Id+"', '"+txtA1.Text+"')";
            cmd.ExecuteScalar();
            cmd.CommandText = "insert into SecurityQuestions (loginId, questionId, securityQuestion_answer) values " +
                "('" + loginId + "', '" + q2Id + "', '" + txtA2.Text + "')";
            cmd.ExecuteScalar();
            cmd.CommandText = "insert into SecurityQuestions (loginId, questionId, securityQuestion_answer) values " +
                "('" + loginId + "', '" + q3Id + "', '" + txtA3.Text + "')";
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
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            username = (string)(Session["username"]);
            roleId = (string)(Session["roleId"]);
            bool correctSession = sessionIsCorrect();
            if (!correctSession)
            {
                Session.RemoveAll();
                Session.Clear();
                Session.Abandon();
                Response.Redirect("~/");
            }
        }
        public Boolean sessionIsCorrect()
        {                       
            Boolean correctSession = true;
            Boolean isEmptySession = checkIfSessionIsEmpty();
            if (isEmptySession)
                correctSession = false;
            Boolean correctSessionValues = checkSeesionValues();
            if (correctSessionValues == false)
            {
                correctSession = false;
            }
            return correctSession;
        }
        protected Boolean checkSeesionValues()
        {
            SqlConnection connect = new SqlConnection(config.getConnectionString());
            Boolean correct = true;
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();            
            cmd.CommandText = "select count(*) from logins where login_username like '" + username + "' and roleId = '" + roleId + "' ";
            int countValues = Convert.ToInt32(cmd.ExecuteScalar());
            if (countValues < 1)//session has wrong values for any role.
                correct = false;
            connect.Close();
            return correct;
        }
        protected Boolean checkIfSessionIsEmpty()
        {
            Boolean itIsEmpty = false;
            if (string.IsNullOrWhiteSpace(username) || (!roleId.Equals("1") && !roleId.Equals("2") && !roleId.Equals("3")))//if no session values for either username or roleId, set to false.
            {
                itIsEmpty = true;
            }
            return itIsEmpty;
        }



    }
}