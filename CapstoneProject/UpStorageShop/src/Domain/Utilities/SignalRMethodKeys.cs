using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public class SignalRMethodKeys
    {
        public static class Order
        {
            public static string Added => nameof(Added);
            public static string Removed => nameof(Removed);
            public static string Updated => nameof(Updated);
        }

        public static class Log
        {
            public static string SendLogNotificationAsync => nameof(SendLogNotificationAsync);
            public static string NewLogAdded => nameof(NewLogAdded);
            public static string SendToken => nameof(SendToken);
        }

        public static class Notification
        {
            public static string NotificationAdded => nameof(NotificationAdded);
        }
    }
}
