using System;

namespace AppointmentManager.Models
{
    public class Appointment
    {
        private string _id;
        public string Id { get { return _id; } set { _id = value; } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private string _location;
        public string Location { get { return _location; } set { _location = value; } }

        private DateTime _startTime;
        public DateTime StartTime { get { return _startTime; } set { _startTime = value; } }

        private DateTime _endTime;
        public DateTime EndTime { get { return _endTime; } set { _endTime = value; } }

        private Reminder _reminder;
        public Reminder Reminder { get { return _reminder; } set { _reminder = value; } }

        public Appointment(string name, DateTime startTime, DateTime endTime, string location)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Location = location;
        }

        public void addReminder(string message)
        {
            this.Reminder = new Reminder(15, message);
        }
    }
}
