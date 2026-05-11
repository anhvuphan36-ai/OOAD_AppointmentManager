using System;
using System.Collections.Generic;

namespace AppointmentManager.Models
{
    public class GroupMeeting : Appointment // Kế thừa thuộc tính của Appointment (Tên, Thời gian, Địa điểm)
    {
        private List<User> _participants;
        // Danh sách chứa những người tham gia vào cuộc họp nhóm này
        public List<User> Participants { get { return _participants; } set { _participants = value; } }

        public GroupMeeting(string name, DateTime startTime, DateTime endTime, string location)
            : base(name, startTime, endTime, location)
        {
            Participants = new List<User>(); // Khởi tạo danh sách rỗng ban đầu
        }

        // Hàm này dùng để nhét thêm 1 người vào cuộc họp
        public void addParticipant(User user)
        {
            Participants.Add(user);
        }
    }
}
