using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Settings
{
    public class NotificationSettings
    {
        public Guid Id { get; set; } 

        public string? UserId { get; set; }

        public bool PushNotification { get; set; }

        public bool EmailNotification { get; set; }

        public string? EmailAddress { get; set; }
    }
}
