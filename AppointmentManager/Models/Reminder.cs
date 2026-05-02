using System;

namespace AppointmentManager.Models
{
    public class Reminder
    {
        public int timeBefore { get; set; }
        public string message { get; set; }
        public bool isSent { get; set; }

        public Reminder(int timeBefore, string message)
        {
            this.timeBefore = timeBefore;
            this.message = message;
            this.isSent = false;
        }

        public void scheduleReminder(DateTime appointmentTime)
        {
            // Dummy implementation
            Console.WriteLine($"Reminder scheduled for {appointmentTime.AddMinutes(-timeBefore)}");
        }

        public void sendReminder()
        {
            this.isSent = true;
            Console.WriteLine("Reminder sent: " + message);
        }
    }
}
