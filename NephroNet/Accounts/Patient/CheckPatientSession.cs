using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NephroNet.Accounts.Patient
{
    public class CheckPatientSession
    {
        string username = "", roleId = "", token = "";
        Configuration config = new Configuration();
        public Boolean sessionIsCorrect(string temp_username, string temp_roleId, string temp_token)
        {
            username = temp_username;
            roleId = temp_roleId;
            token = temp_token;

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
        public int countTotalAlerts()
        {
            SqlConnection connect = new SqlConnection(config.getConnectionString());
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            int count = 0;
            //Count join-topic requests:
            //Get this user's ID:
            cmd.CommandText = "select loginId from logins where login_username = '" + username + "' ";
            string loginId = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string userId = cmd.ExecuteScalar().ToString();
            //Count the approved, not-deleted, not-denied, and not-terminated topics for this user:
            cmd.CommandText = "select count(*) from topics where topic_createdBy = '" + userId + "' and topic_isApproved = 1 and topic_isDeleted = 0 and topic_isDenied = 0 and topic_isTerminated = 0 ";
            int topicsForThisUser = Convert.ToInt32(cmd.ExecuteScalar());
            if (topicsForThisUser > 0)
            {
                int topicsToReview = 0;
                ////Get Topic IDs for this user:
                for (int i = 1; i <= topicsForThisUser; i++)
                {
                    cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_createdBy = '" + userId + "' and topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                    string topicId = cmd.ExecuteScalar().ToString();
                    cmd.CommandText = "select count(*) from [UsersForTopics] where topicId = '" + topicId + "' and isApproved = 0 ";
                    topicsToReview = topicsToReview + Convert.ToInt32(cmd.ExecuteScalar());
                }
                count = count + topicsToReview;
            }
            connect.Close();
            return count;
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
            cmd.CommandText = "select count(*) from logins where login_token like '" + token + "' and roleId = '" + roleId + "' ";
            int countTokenValues = Convert.ToInt32(cmd.ExecuteScalar());
            if (countTokenValues < 1)//session has wrong values for any role with the recieved token.
                correct = false;
            connect.Close();
            return correct;
        }
        protected Boolean checkIfSessionIsEmpty()
        {
            Boolean itIsEmpty = false;
            if (string.IsNullOrWhiteSpace(username) || (!roleId.Equals("3")))//if no session values for either username or roleId, set to false.
            {
                itIsEmpty = true;
            }
            return itIsEmpty;
        }
    }
}