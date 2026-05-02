using System;
using System.Collections.Generic;

namespace AppointmentManager.Models
{
    public class GroupMeeting : Appointment
    {
        private List<User> _participants;
        public List<User> Participants { get { return _participants; } set { _participants = value; } }

        public GroupMeeting(string name, DateTime startTime, DateTime endTime, string location)
            : base(name, startTime, endTime, location)
        {
            Participants = new List<User>();
        }

        public void addParticipant(User user)
        {
            Participants.Add(user);
        }
    }
}
