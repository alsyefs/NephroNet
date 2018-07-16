﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class ReviewMessage : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        string messageId = "";
        //Globals to be used later:
        int g_topic_isApproved, g_topic_isDenied;
        string g_creator, g_topic_title, g_email, g_entry_text;

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //Hide the success message:
            lblMessage.Visible = false;
            //Set entry_isApproved = 1, entry_isDenied = 0: (1 in bit = true)            
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "update Entries set entry_isApproved = 1 and entry_isDenied = 0 where entryId = '" + messageId + "' ";
            cmd.ExecuteScalar();
            connect.Close();
            //Create an email message to be sent:
            string emailMessage = "Hello " + g_creator + ",\n\n" +
                "This email is to inform you that your message to the title (" + g_topic_title + ") has been approved for NephroNet." +
                "Your message was:\n\n \""+ g_entry_text + "\"\n\n"+
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification to the user using the stored email:
            Email emailClass = new Email();
            emailClass.sendEmail(g_email, emailMessage);
            //Display a success message:
            lblMessage.Visible = true;
            lblMessage.Text = "The selected message has been successfully approved and a notification email has been sent to the user!";
            //Hide "Approve" and "Deny" buttons:
            hideApproveDeny();
        }
        protected void hideApproveDeny()
        {
            btnApprove.Visible = false;
            btnDeny.Visible = false;
        }
        protected void btnDeny_Click(object sender, EventArgs e)
        {
            //Hide the success message:
            lblMessage.Visible = false;
            //Set entry_isApproved = 1, entry_isDenied = 0: (1 in bit = true)            
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "update Entries set entry_isApproved = 0 and entry_isDenied = 1 where entryId = '" + messageId + "' ";
            cmd.ExecuteScalar();
            connect.Close();
            //Create an email message to be sent:
            string emailMessage = "Hello " + g_creator + ",\n\n" +
                "This email is to inform you that your message to the title (" + g_topic_title + ") has been denied for NephroNet." +
                "Your message was:\n\n \"" + g_entry_text + "\"\n\n" +
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification to the user using the stored email:
            Email emailClass = new Email();
            emailClass.sendEmail(g_email, emailMessage);
            //Display a success message:
            lblMessage.Visible = true;
            lblMessage.Text = "The selected message has been successfully denied and a notification email has been sent to the user!";
            //Hide "Approve" and "Deny" buttons:
            hideApproveDeny();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveMessages");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            messageId = Request.QueryString["id"];
            showMessageInformation();
        }
        protected void showMessageInformation()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Check if the ID exists in the database:
            cmd.CommandText = "select count(*) from topics where topicId = '" + messageId + "' ";
            int countMessage = Convert.ToInt32(cmd.ExecuteScalar());
            if (countMessage > 0)//if ID exists, countMessage = 1
            {
                //Get topic ID:
                cmd.CommandText = "select topicId from [Entries] where [entryId] = '" + messageId + "' ";
                string topicId = cmd.ExecuteScalar().ToString();
                //Get topic title:
                cmd.CommandText = "select topic_title from [Topics] where [topicId] = '" + topicId + "' ";
                string topic_title = cmd.ExecuteScalar().ToString();
                //Get entry userId:
                cmd.CommandText = "select userId from [Entries] where [entryId] = '" + messageId + "' ";
                string userId = cmd.ExecuteScalar().ToString();
                //Get user full name: 
                cmd.CommandText = "select user_firstname from users where userId = '" + userId + "' ";
                string creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + userId + "' ";
                creator = creator = " " + cmd.ExecuteScalar().ToString();
                //Get user's email:
                cmd.CommandText = "select user_email from users where userId = '" + userId + "' ";
                string email = cmd.ExecuteScalar().ToString();
                //Get entry_time:
                cmd.CommandText = "select entry_time from [Entries] where [entryId] = '" + messageId + "' ";
                string entry_time = cmd.ExecuteScalar().ToString();
                //Get entry_text:
                cmd.CommandText = "select entry_text from [Entries] where [entryId] = '" + messageId + "' ";
                string entry_text = cmd.ExecuteScalar().ToString();
                //Get entry_isDeleted:
                cmd.CommandText = "select entry_isDeleted from [Entries] where [entryId] = '" + messageId + "' ";
                int int_entry_isDeleted = Convert.ToInt32(cmd.ExecuteScalar());
                string entry_isDeleted = "";
                if (int_entry_isDeleted == 0)
                    entry_isDeleted = "Topic has not been deleted.";
                else
                    entry_isDeleted = "Topic has been deleted.";
                //Get entry_isApproved:
                cmd.CommandText = "select entry_isApproved from [Entries] where [entryId] = '" + messageId + "' ";
                int int_entry_isApproved = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                string entry_isApproved;
                if (int_entry_isApproved == 0)
                    entry_isApproved = "Message has not been approved.";
                else
                    entry_isApproved = "Message has been approved.";
                //Get entry_isDenied:
                cmd.CommandText = "select entry_isDenied from [Entries] where [entryId] = '" + messageId + "' ";
                int int_entry_isDenied = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                string entry_isDenied;
                if (int_entry_isDenied == 0)
                    entry_isDenied = "Message has not been approved.";
                else
                    entry_isDenied = "Message has been approved.";
                //Get "Yes" or "No" for entry_hasImage:
                cmd.CommandText = "select entry_hasImage from [Entries] where [entryId] = '" + messageId + "' ";
                int entry_hasImage = Convert.ToInt32(cmd.ExecuteScalar());
                string str_entry_hasImage = "";
                if (entry_hasImage == 0)
                    str_entry_hasImage = "Message does not have an image.";
                else
                    str_entry_hasImage = "Message has an image.";
                //Get tags:
                string tagNames = "";
                cmd.CommandText = "select count(*) from TagsForMessages where entryId = '" + messageId + "' ";
                int totalTags = Convert.ToInt32(cmd.ExecuteScalar());
                if (totalTags == 0)
                    tagNames = "There are no tags for the selected message";
                for (int i = 1; i <= totalTags; i++)
                {
                    cmd.CommandText = "select [tagId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY tagId ASC), * FROM [TagsForEntries] where entryId = '" + messageId + "') as t where rowNum = '" + i + "'";
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
                if (entry_hasImage == 1)
                {
                    cmd.CommandText = "select count(*) from ImagesForEntries where entryId = '" + messageId + "' ";
                    int totalImages = Convert.ToInt32(cmd.ExecuteScalar());
                    string imagesHTML = "";
                    for (int i = 1; i <= totalImages; i++)
                    {
                        cmd.CommandText = "select [imageId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY imageId ASC), * FROM [ImagesForEntries] where entryId = '" + messageId + "') as t where rowNum = '" + i + "'";
                        string imageId = cmd.ExecuteScalar().ToString();
                        cmd.CommandText = "select image_name from Images where imageId = '" + imageId + "' ";
                        string image_name = cmd.ExecuteScalar().ToString();
                        imagesHTML = imagesHTML + "<img src='../../images/" + image_name + "'></img> <br />";
                    }
                    lblMessageInformation.Text = "Creator: " + creator + "<br />" +
                        "Topic related: " + topic_title + "<br />" +
                        "Message time: " + entry_time + "<br />" +
                        "Deleted?: " + entry_isDeleted + "<br />" +
                        "Has image?: " + str_entry_hasImage + "<br />" +
                        "Approved?: " + entry_isApproved + "<br />" +
                        "Denied?: " + entry_isDenied + "<br />" +
                        "Messages: " + entry_text + "<br />"+
                        "Tags: \"" + tagNames + "\"<br />" +
                        imagesHTML;
                }
                else
                {
                    lblMessageInformation.Text = "Creator: " + creator + "<br />" +
                        "Topic related: " + topic_title + "<br />" +
                        "Message time: " + entry_time + "<br />" +
                        "Deleted?: " + entry_isDeleted + "<br />" +
                        "Has image?: " + str_entry_hasImage + "<br />" +
                        "Approved?: " + entry_isApproved + "<br />" +
                        "Denied?: " + entry_isDenied + "<br />" +
                        "Messages: " + entry_text + "<br />";
                }
                lblMessageInformation.Visible = true;
                //Copy values to globals:
                g_topic_isApproved = int_entry_isApproved; g_topic_isDenied = int_entry_isDenied;
                g_creator = creator; g_topic_title = topic_title; g_email = email; g_entry_text = entry_text;
            }
            else
            {
                addSession();
                Response.Redirect("ApproveMessages");
            }
            connect.Close();
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
            lblAlerts.Text = "(" + session.countTotalAlerts() + ")";
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