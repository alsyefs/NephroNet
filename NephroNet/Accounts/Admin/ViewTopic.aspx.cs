using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class ViewTopic : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        string topicId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Configuration config = new Configuration();
            conn = config.getConnectionString();
            connect = new SqlConnection(conn);
            getSession();
            CheckAdminSession session = new CheckAdminSession();
            bool correctSession = session.sessionIsCorrect(username, roleId, token);
            if (!correctSession)
                clearSession();
            lblAlerts.Text = "(" + session.countTotalAlerts() + ")";
            topicId = Request.QueryString["id"];
            bool authorized = isUserAuthorizedToView();
            if (!authorized)
                unauthorized();
            showInformation();
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
        protected bool isUserAuthorizedToView()
        {
            bool authorized = true;
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Get the userId:
            cmd.CommandText = "select userId from Users where loginId = '"+loginId+"' ";
            string userId = cmd.ExecuteScalar().ToString();
            //Count to check if the user has requested join:
            cmd.CommandText = "select count(*) from UsersForTopics where topicId = '"+topicId+"' and userId = '"+ userId + "'  ";
            int countUserForTopic = Convert.ToInt32(cmd.ExecuteScalar());
            if (countUserForTopic > 0)
            {
                //Check if the user is approved:
                cmd.CommandText = "select isApproved from UsersForTopics where topicId = '" + topicId + "' and userId = '" + userId + "'  ";
                int isApproved = Convert.ToInt32(cmd.ExecuteScalar());
                if (isApproved == 0)
                    authorized = false;
            }
            else
                authorized = false;
            connect.Close();
            return authorized;
        }
        protected void unauthorized()
        {
            addSession();
            Response.Redirect("JoinTopic.aspx?id=" + topicId);
        }
        protected void showInformation()
        {
            //Get information from DB:

            //Display info:
            lblContents.Text = "";

            //Maybe create new labels and place them for each entry:
            //Label label = new Label();
            //label.Visible = true;

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("Home");
        }
    }
}