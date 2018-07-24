using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NephroNet
{
    public class Layouts
    {
        public static string postMessage(int i, string creator_name, string entry_time, string entry_text, string imagesHtml, 
            string entry_creatorId, string topic_creatorId, string userId, string entryId)
        {
            string deleteCommand = "";
            //Check if the user viewing has an entry he/she created, or if the current user viewing is the topic creator::
            if (entry_creatorId.Equals(userId) || topic_creatorId.Equals(userId))
                deleteCommand = "<a href=\"DeleteEntry.aspx?entryId=" + entryId + "\"> Remove Entry " + i + "</a> <br />";
            string background_color = "";
            if (i % 2 == 0)
                background_color = "style = \"background-color:#CCCC99; width: 100%;\"";
            else
                background_color = "style = \"background-color:#F7F7DE; width: 100%;\"";
            string message = "<p " + background_color + " >Message " + i + " - added by " + creator_name + " on " + entry_time + "<br />" +
                    entry_text + "<br /> " +
                        imagesHtml +
                        "<br /> " +
                    deleteCommand +
                    "--------------------------------------------------------------------------------------------" +
                        "<br /></p>";
            return message;
        }
    }
}