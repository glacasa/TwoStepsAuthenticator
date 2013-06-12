using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoStepsAuthenticator.TestWebsite.Users
{
    public class WebsiteUserStorage
    {
        private static Dictionary<String, WebsiteUser> users;

        static WebsiteUserStorage()
        {
            users = new Dictionary<String, WebsiteUser>();

            users.Add("user1",
                new WebsiteUser("user1", "user1", "AAAAAAAAAAAAAAAA"));

            users.Add("user2",
               new WebsiteUser("user2", "user2", "BBBBBBBBBBBBBBBB"));

            users.Add("user3",
               new WebsiteUser("user3", "user3", null));
        }

        public static WebsiteUser GetUser(string login)
        {
            if (users.ContainsKey(login))
                return users[login];
            return null;
        }

    }
}