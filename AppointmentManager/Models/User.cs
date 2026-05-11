using System;

namespace AppointmentManager.Models
{
    public class User
    {
        private string _userId;
        public string UserId { get { return _userId; } set { _userId = value; } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private string _email;
        public string Email { get { return _email; } set { _email = value; } }

        public User(string userId, string name, string email)
        {
            this.UserId = userId;
            this.Name = name;
            this.Email = email;
        }

        // Định nghĩa hành động một người dùng tự xin tham gia vào nhóm
        public void joinGroupMeeting(GroupMeeting groupMeeting)
        {
            groupMeeting.addParticipant(this); // Đẩy chính bản thân (this) vào nhóm
        }

        public void replaceAppointment(Appointment oldApp, Appointment newApp)
        {
            // Managed by Calendar, added here to match class diagram strictly
        }
    }
}
