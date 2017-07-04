using System;

namespace AnomalyDetection.Core.Helpers
{
    public delegate void SimpleNotificationEventHandler<T>(object sender, SimpleNotificationEventArgs<T> e);

    public class SimpleNotificationEventArgs<T> : SimpleNotificationEventArgs
    {
        public T Value { get; set; }

        public SimpleNotificationEventArgs(T value, string message, DateTime timeOccured)
        {
            this.Value = value;
            this.Message = message;
            this.TimeOccured = timeOccured;
        }

        public SimpleNotificationEventArgs(T value, string message) : this(value, message, DateTime.Now)
        {
        }

        public SimpleNotificationEventArgs(T value) : this(value, string.Empty)
        {
        }
    }


    /// <summary>
    /// An event handler for providing a simple notification with a user message and a timestamp
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SimpleNotificationEventHandler(object sender, SimpleNotificationEventArgs e);

    public class SimpleNotificationEventArgs : EventArgs
    {
        public string Message { get; set; }
        public DateTime TimeOccured { get; set; }

        public SimpleNotificationEventArgs(string message, DateTime timeOccured)
        {
            this.Message = message;
            this.TimeOccured = timeOccured;
        }

        public SimpleNotificationEventArgs(string message)
            : this(message, DateTime.Now)
        {
        }

        public SimpleNotificationEventArgs()
            : this(string.Empty)
        {
        }
    }

}