using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class Notifications : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        protected void Page_Load(object sender, EventArgs e)
        {            
            initialPageAccess();
            showAllFields();
            countNewUsers();
            countNewTopics();
            countNewMessages();
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
            lblAlerts.Text = "("+session.countTotalAlerts()+")";
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
        protected void showAllFields()
        {
            btnNewUsers.Visible = true;
            btnNewTopics.Visible = true;
            btnNewMessages.Visible = true;
        }
        protected void countNewUsers()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select count(*) from Registrations";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if(count == 0)
            {
                lblNewUsers.Text = "There are no new users to review.";
                btnNewUsers.Visible = false;
            }
            else if(count == 1)
            {
                lblNewUsers.Text = "There is one new user to review.";
                btnNewUsers.Visible = true;
            }
            else
            {
                lblNewUsers.Text = "There are "+count+" new users to review.";
                btnNewUsers.Visible = true;
            }
            connect.Close();
        }
        protected void countNewTopics()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select count(*) from [Topics] where topic_isApproved = 0 and topic_isDenied = 0 and topic_isTerminated = 0";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 0)
            {
                lblNewTopics.Text = "There are no new topics to review.";
                btnNewTopics.Visible = false;
            }
            else if (count == 1)
            {
                lblNewTopics.Text = "There is one new topic to review.";
                btnNewTopics.Visible = true;
            }
            else
            {
                lblNewTopics.Text = "There are " + count + " new topics to review.";
                btnNewTopics.Visible = true;
            }
            connect.Close();
        }
        protected void countNewMessages()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //count messages that are not approved and have not been denied.
            cmd.CommandText = "select count(*) from [Entries] where entry_isApproved = 0 and entry_isDenied = 0 and entry_isDeleted = 0";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 0)
            {
                lblNewMessages.Text = "There are no new messages to review.";
                btnNewMessages.Visible = false;
            }
            else if (count == 1)
            {
                lblNewMessages.Text = "There is one new message to review.";
                btnNewMessages.Visible = true;
            }
            else
            {
                lblNewMessages.Text = "There are " + count + " new messages to review.";
                btnNewMessages.Visible = true;
            }
            connect.Close();
        }
        protected void btnNewUsers_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveUsers");
        }
        protected void btnNewTopics_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveTopics");
        }
        protected void btnNewMessages_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveMessages");
        }
       
    }
}