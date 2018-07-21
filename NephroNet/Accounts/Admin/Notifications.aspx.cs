﻿using System;
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
            countNewJoinTopicRequests();
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
            if (session.countTotalAlerts() == 0)
            {
                lblError.Visible = true;
                lblError.Text = "There are no new alerts!";
            }
            else
            {
                lblError.Visible = false;
            }
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
            btnNewJoinTopicRequests.Visible = true;
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
                //lblNewUsers.Visible = false;
            }
            else if(count == 1)
            {
                lblNewUsers.Text = "There is one new user to review.";
                btnNewUsers.Visible = true;
                lblNewUsers.Visible = true;
            }
            else
            {
                lblNewUsers.Text = "There are "+count+" new users to review.";
                btnNewUsers.Visible = true;
                lblNewUsers.Visible = true;
            }
            connect.Close();
        }
        protected void countNewJoinTopicRequests()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Get this user's ID:
            cmd.CommandText = "select userId from Users where loginId = '"+loginId+"' ";
            string userId = cmd.ExecuteScalar().ToString();
            //Count the approved, not-deleted, not-denied, and not-terminated topics for this user:
            cmd.CommandText = "select count(*) from topics where topic_createdBy = '"+userId+"' and topic_isApproved = 1 and topic_isDeleted = 0 and topic_isDenied = 0 and topic_isTerminated = 0 ";
            int topicsForThisUser = Convert.ToInt32(cmd.ExecuteScalar());
            if (topicsForThisUser > 0)
            {
                int topicsToReview = 0;
                ////Get Topic IDs for this user:
                for (int i = 1; i <= topicsForThisUser; i++ )
                {
                    cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_createdBy = '" + userId + "' and topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                    string topicId = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select count(*) from [UsersForTopics] where topicId = '"+topicId+"' and isApproved = 0 ";
                    topicsToReview = topicsToReview + Convert.ToInt32(cmd.ExecuteScalar());
                }
                if (topicsToReview == 0)
                {
                    lblNewJoinTopicRequests.Text = "There are no new join-topic requests to review.";
                    btnNewJoinTopicRequests.Visible = false;
                    //lblNewJoinTopicRequests.Visible = false;
                }
                else if (topicsToReview == 1)
                {
                    lblNewJoinTopicRequests.Text = "There is one new join-topic request to review.";
                    btnNewJoinTopicRequests.Visible = true;
                    lblNewJoinTopicRequests.Visible = true;
                }
                else
                {
                    lblNewJoinTopicRequests.Text = "There are " + topicsToReview + " new join-topic requests to review.";
                    btnNewJoinTopicRequests.Visible = true;
                    lblNewJoinTopicRequests.Visible = true;
                }
            }
            else
            {
                lblNewJoinTopicRequests.Text = "There are no new join-topic requests to review.";
                btnNewJoinTopicRequests.Visible = false;
                lblNewJoinTopicRequests.Visible = false;
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
                //lblNewTopics.Visible = false;
            }
            else if (count == 1)
            {
                lblNewTopics.Text = "There is one new topic to review.";
                btnNewTopics.Visible = true;
                lblNewTopics.Visible = true;
            }
            else
            {
                lblNewTopics.Text = "There are " + count + " new topics to review.";
                btnNewTopics.Visible = true;
                lblNewTopics.Visible = true;
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
                //lblNewMessages.Visible = false;
            }
            else if (count == 1)
            {
                lblNewMessages.Text = "There is one new message to review.";
                btnNewMessages.Visible = true;
                lblNewMessages.Visible = true;
            }
            else
            {
                lblNewMessages.Text = "There are " + count + " new messages to review.";
                btnNewMessages.Visible = true;
                lblNewMessages.Visible = true;
            }
            connect.Close();
        }
        protected void btnNewJoinTopicRequests_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveJoinTopics");
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