using System;
using System.Collections.Generic;

namespace AppointmentManager.Models
{
    public class GroupMeeting : Appointment
    {
        public List<User> participants { get; set; }

        public GroupMeeting(string name, DateTime startTime, DateTime endTime, string location)
            : base(name, startTime, endTime, location)
        {
            participants = new List<User>();
        }

        public void addParticipant(User user)
        {
            participants.Add(user);
        }

        public bool isSameNameAndDuration(Appointment other)
        {
            return this.name.Equals(other.name, StringComparison.OrdinalIgnoreCase) &&
                   this.getDuration() == other.getDuration();
        }
    }
}
