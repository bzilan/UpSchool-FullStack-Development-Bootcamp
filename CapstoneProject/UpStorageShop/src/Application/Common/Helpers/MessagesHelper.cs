using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class MessagesHelper
    {
        public static class Email
        {
            public static class Confirmation
            {
                public static string Subject => "Thank you for signing up to CrawlerBot!";
                public static string ActivationMessage => "In order to activate your account please hit the activation button given below.";
                public static string ButtonText => "Activate My Account";
                public static string ButtonLink(string email, string emailToken) => $"https://crawler.app/account/account-activation?email={email}&token={emailToken}";
                public static string Name(string firstName) => $"Hi {firstName}";
            }
        }
    }
}
