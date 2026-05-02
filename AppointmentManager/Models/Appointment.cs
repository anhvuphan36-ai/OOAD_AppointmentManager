using System;

namespace AppointmentManager.Models
{
    public class Appointment
    {
        public string id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public Reminder reminder { get; set; }

        public Appointment(string name, DateTime startTime, DateTime endTime, string location)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.startTime = startTime;
            this.endTime = endTime;
            this.location = location;
        }

        public bool isValid()
        {
            return startTime >= DateTime.Now && endTime > startTime;
        }

        public int getDuration()
        {
            return (int)(endTime - startTime).TotalMinutes;
        }

        public bool conflictsWith(Appointment other)
        {
            return (this.startTime < other.endTime && this.endTime > other.startTime);
        }

        public void addReminder(string message)
        {
            this.reminder = new Reminder(15, message);
        }
    }
}
