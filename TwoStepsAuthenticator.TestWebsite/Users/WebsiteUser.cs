using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoStepsAuthenticator.TestWebsite.Users
{
    public class WebsiteUser
    {
        public WebsiteUser(string login, string password, string key)
        {
            Login = login;
            Password = password;
            DoubleAuthKey = key;
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string DoubleAuthKey { get; set; }

        public bool DoubleAuthActivated
        {
            get { return !String.IsNullOrEmpty(DoubleAuthKey); }
        }

    }

}