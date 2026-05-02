using System;

namespace AppointmentManager.Models
{
    public class User
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        public User(string userId, string name, string email)
        {
            this.userId = userId;
            this.name = name;
            this.email = email;
        }

        public void joinGroupMeeting(GroupMeeting groupMeeting)
        {
            groupMeeting.addParticipant(this);
        }

        public void replaceAppointment(Appointment oldApp, Appointment newApp)
        {
            // Managed by Calendar, added here to match class diagram strictly
        }
    }
}
