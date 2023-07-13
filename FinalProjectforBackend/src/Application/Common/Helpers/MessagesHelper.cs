using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public class MessagesHelper
    {
        public static class Email
        {
            public static class Confirmation
            {
                public static string Subject => "Crawling operation.";
                public static string ActivationMessage => "You can access the results from the database.";

            }
        }
    }
}
