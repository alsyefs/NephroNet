﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Physician
{
    public partial class ReviewJoinTopic : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        string topicId = "";
        string requestId = "";
        //Globals for "Topics" table:
        int g_topic_isApproved, g_topic_isDenied;
        string g_creator, g_topic_title, g_email;        
        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            requestId = Request.QueryString["id"];
            topicId = getTopicId();
            showUserInformation();
            showTopicInformation();
        }
        protected string getTopicId()
        {
            string temp_topicId = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select topicId from UsersForTopics where usersForTopicsId = '" + requestId + "' ";
            temp_topicId = cmd.ExecuteScalar().ToString();
            connect.Close();
            return temp_topicId;
        }
        protected void showUserInformation()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Get user ID:
            cmd.CommandText = "select userId from UsersForTopics where usersForTopicsId = '" + requestId + "' ";
            string requesterUserId = cmd.ExecuteScalar().ToString();
            //Get login ID:
            cmd.CommandText = "select loginid from users where userId = '" + requesterUserId + "' ";
            string requesterLoginId = cmd.ExecuteScalar().ToString();
            //Check if the user's account is still active:
            cmd.CommandText = "select login_isActive from Logins where loginid = '" + requesterLoginId + "' ";
            int active = Convert.ToInt32(cmd.ExecuteScalar());
            if (active == 1)//if account is active, active = 1
            {
                //Get name:
                cmd.CommandText = "select user_firstname from users where userId = '" + requesterUserId + "' ";
                string requesterName = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + requesterUserId + "' ";
                requesterName = requesterName + " " + cmd.ExecuteScalar().ToString();
                //Get short profile:
                lblRequesterInfo.Text = "Requester: " + requesterName + "<br />";
                lblRequesterInfo.ForeColor = System.Drawing.Color.Black;
                btnApprove.Visible = true;
            }
            else
            {
                lblRequesterInfo.Text = "The user account for the person requested to join your topic is inactive.";
                lblRequesterInfo.ForeColor = System.Drawing.Color.Red;
                btnApprove.Visible = false;
                //addSession();
                //Response.Redirect("ApproveJoinTopics");
            }
            connect.Close();
        }
        protected void showTopicInformation()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Check if the ID exists in the database:
            cmd.CommandText = "select count(*) from topics where topicId = '" + topicId + "' ";
            int countTopic = Convert.ToInt32(cmd.ExecuteScalar());
            if (countTopic > 0)//if ID exists, countTopic = 1
            {
                //Get topic_createdBy:
                cmd.CommandText = "select topic_createdBy from [Topics] where [topicId] = '" + topicId + "' ";
                string topic_createdBy = cmd.ExecuteScalar().ToString();
                //Get creator's email:
                cmd.CommandText = "select user_email from users where userId = '" + topic_createdBy + "' ";
                string email = cmd.ExecuteScalar().ToString();
                //Get creator's fullname:
                cmd.CommandText = "select user_firstname from users where userId = '" + topic_createdBy + "' ";
                string creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + topic_createdBy + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                //Get topic_type:
                cmd.CommandText = "select topic_type from [Topics] where [topicId] = '" + topicId + "' ";
                string topic_type = cmd.ExecuteScalar().ToString();
                //Get topic_title:
                cmd.CommandText = "select topic_title from [Topics] where [topicId] = '" + topicId + "' ";
                string topic_title = cmd.ExecuteScalar().ToString();
                //Get topic_time:
                cmd.CommandText = "select topic_time from [Topics] where [topicId] = '" + topicId + "' ";
                string topic_time = cmd.ExecuteScalar().ToString();
                //Get topic_description:
                cmd.CommandText = "select topic_description from [Topics] where [topicId] = '" + topicId + "' ";
                string topic_description = cmd.ExecuteScalar().ToString();
                //Get "Yes" or "No" for topic_hasImage:
                cmd.CommandText = "select topic_hasImage from [Topics] where [topicId] = '" + topicId + "' ";
                int topic_hasImage = Convert.ToInt32(cmd.ExecuteScalar());
                
                //Get topic_isDeleted ?:
                cmd.CommandText = "select topic_isDeleted from [Topics] where [topicId] = '" + topicId + "' ";
                int int_topic_isDeleted = Convert.ToInt32(cmd.ExecuteScalar());
                
                //Get topic_isApproved ?:
                cmd.CommandText = "select topic_isApproved from [Topics] where [topicId] = '" + topicId + "' ";
                int int_topic_isApproved = Convert.ToInt32(cmd.ExecuteScalar());
                
                //Get topic_isDenied ?:
                cmd.CommandText = "select topic_isDenied from [Topics] where [topicId] = '" + topicId + "' ";
                int int_topic_isDenied = Convert.ToInt32(cmd.ExecuteScalar());
                
                //Get topic_isTerminated ?:
                cmd.CommandText = "select topic_isTerminated from [Topics] where [topicId] = '" + topicId + "' ";
                int int_topic_isTerminated = Convert.ToInt32(cmd.ExecuteScalar());
                
                //Get tags:
                string tagNames = "";
                cmd.CommandText = "select count(*) from TagsForTopics where topicId = '" + topicId + "' ";
                int totalTags = Convert.ToInt32(cmd.ExecuteScalar());
                if (totalTags == 0)
                    tagNames = "There are no tags for the selected topic";
                for (int i = 1; i <= totalTags; i++)
                {
                    cmd.CommandText = "select [tagId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY tagId ASC), * FROM [TagsForTopics] where topicId = '" + topicId + "') as t where rowNum = '" + i + "'";
                    string tagId = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select tag_name from Tags where tagId = '" + tagId + "' ";
                    if (totalTags == 1)
                        tagNames = cmd.ExecuteScalar().ToString();
                    else if (totalTags > 1)
                    {
                        if (i == 0)
                            tagNames = cmd.ExecuteScalar().ToString();
                        else
                            tagNames = tagNames + ", " + cmd.ExecuteScalar().ToString();
                    }
                }
                //Create an informative message containing all information for the selected user:
                string imagesHTML = "";
                if (topic_hasImage == 1)
                {
                    cmd.CommandText = "select count(*) from ImagesForTopics where topicId = '" + topicId + "' ";
                    int totalImages = Convert.ToInt32(cmd.ExecuteScalar());

                    for (int i = 1; i <= totalImages; i++)
                    {
                        cmd.CommandText = "select [imageId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY imageId ASC), * FROM [ImagesForTopics] where topicId = '" + topicId + "') as t where rowNum = '" + i + "'";
                        string imageId = cmd.ExecuteScalar().ToString();
                        cmd.CommandText = "select image_name from Images where imageId = '" + imageId + "' ";
                        string image_name = cmd.ExecuteScalar().ToString();
                        imagesHTML = imagesHTML + "<img src='../../images/" + image_name + "'></img> <br /><br />";
                    }
                }
                lblTopicInformation.Text = 
                    //"Creator: " + creator + "<br />" +
                    "Type: " + topic_type + "<br />" +
                    "Title: " + topic_title + "<br />" +
                    "Time: " + Layouts.getTimeFormat(topic_time) + "<br />" +
                    //"Has image?: " + str_topic_hasImage + "<br />" +
                    //"Deleted?: " + topic_isDeleted + "<br />" +
                    //"Approved?: " + topic_isApproved + "<br />" +
                    //"Denied?: " + topic_isDenied + "<br />" +
                    //"Terminated?: " + topic_isTerminated + "<br />" +
                    "Description: \"" + topic_description + "\"<br />" +
                    //"Tags: \"" + tagNames + "\"<br />" +
                    imagesHTML;
                lblTopicInformation.Visible = true;
                //Copy values to globals:
                g_topic_isApproved = int_topic_isApproved; g_topic_isDenied = int_topic_isDenied;
                g_creator = creator; g_topic_title = topic_title; g_email = email;
            }
            else
            {
                addSession();
                Response.Redirect("ApproveJoinTopics");
            }
            connect.Close();
        }
        protected void initialPageAccess()
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

        protected void btnDeny_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Hide the success message:
            lblMessage.Visible = false;
            //Delete the request record from the database:
            cmd.CommandText = "delete from UsersForTopics where UsersForTopicsId = '" + requestId + "' ";
            cmd.ExecuteScalar();
            //Get the topic's creator:
            cmd.CommandText = "select topic_createdBy from Topics where topicId = '" + topicId + "' ";
            string creatorId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_firstname from Users where userId = '" + creatorId + "' ";
            string creator_name = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_lastname from Users where userId = '" + creatorId + "' ";
            creator_name = creator_name + " " + cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_email from Users where userId = '" + creatorId + "' ";
            string creator_email = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select topic_title from Topics where topicId = '" + topicId + "' ";
            string title = cmd.ExecuteScalar().ToString();
            //Get requester info:
            cmd.CommandText = "select userId from UsersForTopics where UsersForTopicsId = '" + requestId + "' ";
            string requesterId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_firstname from Users where userId = '" + requesterId + "' ";
            string requester_name = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_lastname from Users where userId = '" + requesterId + "' ";
            requester_name = requester_name + " " + cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_email from Users where userId = '" + requesterId + "' ";
            string requester_email = cmd.ExecuteScalar().ToString();
            connect.Close();
            Email emailClass = new Email();
            //email the creator:
            string emailCreator = "Hello " + creator_name + ",\n\n" +
                "This email is to inform you that you have denied the user (" + requester_name + ") to join your topic (" + title + ").\n\n" +
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification to the user using the stored email:
            emailClass.sendEmail(creator_email, emailCreator);
            //Email the user requested to join:
            string emailRequester = "Hello " + requester_name + ",\n\n" +
                "This email is to inform you that your request to join the topic with the title (" + title + ") has been denied. If you wish to participate in that topic, you may request to join again, or contact the support.\n\n" +
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification to the user using the stored email:
            emailClass.sendEmail(requester_email, emailRequester);
            //Show a successfull message:
            lblMessage.Text = "The selected user has been successfully denied and a notification email has been sent to the user!";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Visible = true;
            //Hide "Approve" and "Deny" buttons:
            hideApproveDeny();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Hide the success message:
            lblMessage.Visible = false;
            //Set isApproved = 1: (1 in bit = true)
            cmd.CommandText = "update UsersForTopics set isApproved = 1 where UsersForTopicsId = '" + requestId + "' ";
            cmd.ExecuteScalar();
            //Get the topic's creator:
            cmd.CommandText = "select topic_createdBy from Topics where topicId = '" + topicId + "' ";
            string creatorId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_firstname from Users where userId = '" + creatorId + "' ";
            string creator_name = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_lastname from Users where userId = '" + creatorId + "' ";
            creator_name = creator_name + " " + cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_email from Users where userId = '" + creatorId + "' ";
            string creator_email = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select topic_title from Topics where topicId = '" + topicId + "' ";
            string title = cmd.ExecuteScalar().ToString();
            //Get requester info:
            cmd.CommandText = "select userId from UsersForTopics where UsersForTopicsId = '" + requestId + "' ";
            string requesterId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_firstname from Users where userId = '" + requesterId + "' ";
            string requester_name = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_lastname from Users where userId = '" + requesterId + "' ";
            requester_name = requester_name + " " + cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_email from Users where userId = '" + requesterId + "' ";
            string requester_email = cmd.ExecuteScalar().ToString();
            connect.Close();
            Email emailClass = new Email();
            //email the creator:
            string emailCreator = "Hello " + creator_name + ",\n\n" +
                "This email is to inform you that you have accpeted the user (" + requester_name + ") to join your topic (" + title + ") .\n\n" +
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification to the user using the stored email:
            emailClass.sendEmail(creator_email, emailCreator);
            //Email the user requested to join:
            string emailRequester = "Hello " + requester_name + ",\n\n" +
                "This email is to inform you that your request to join the topic with the title (" + title + ") has been accepted. You are now permitted to access and participate in that topic.\n\n" +
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification to the user using the stored email:
            emailClass.sendEmail(requester_email, emailRequester);
            //Show a successfull message:
            lblMessage.Text = "The selected user has been successfully accepted and a notification email has been sent to the user!";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Visible = true;
            //Hide "Approve" and "Deny" buttons:
            hideApproveDeny();
        }
        protected void hideApproveDeny()
        {
            btnApprove.Visible = false;
            btnDeny.Visible = false;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveJoinTopics");
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