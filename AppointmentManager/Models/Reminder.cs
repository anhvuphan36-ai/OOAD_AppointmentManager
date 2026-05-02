using System;

namespace AppointmentManager.Models
{
    public class Reminder
    {
        private int _timeBefore;
        public int TimeBefore { get { return _timeBefore; } set { _timeBefore = value; } }

        private string _message;
        public string Message { get { return _message; } set { _message = value; } }

        private bool _isSent;
        public bool IsSent { get { return _isSent; } set { _isSent = value; } }

        public Reminder(int timeBefore, string message)
        {
            this.TimeBefore = timeBefore;
            this.Message = message;
            this.IsSent = false;
        }

        public void scheduleReminder(DateTime appointmentTime)
        {
            // Dummy implementation
            Console.WriteLine($"Reminder scheduled for {appointmentTime.AddMinutes(-TimeBefore)}");
        }

        public void sendReminder()
        {
            this.IsSent = true;
            Console.WriteLine("Reminder sent: " + Message);
        }
    }
}
