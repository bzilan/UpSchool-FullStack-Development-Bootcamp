using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Email
{
   public class SendEmailDto
   {
        public string EmailAddress { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }

        public SendEmailDto(string emailAddress, string content, string subject)
        {
            EmailAddress = emailAddress;

            Content = content;

            Subject = subject;
        }
   }
}
