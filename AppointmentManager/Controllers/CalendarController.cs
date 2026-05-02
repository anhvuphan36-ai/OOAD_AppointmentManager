using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentManager.Models;

namespace AppointmentManager.Controllers
{
    public class CalendarController
    {
        public List<Appointment> appointments { get; set; }
        public List<Reminder> reminders { get; set; }
        public User user { get; set; }

        public CalendarController(User user)
        {
            this.user = user;
            appointments = new List<Appointment>();
            reminders = new List<Reminder>();
        }

        public bool addAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
            if (appointment.reminder != null)
            {
                reminders.Add(appointment.reminder);
                appointment.reminder.scheduleReminder(appointment.startTime);
            }
            return true;
        }

        public void removeAppointment(Appointment appointment)
        {
            appointments.Remove(appointment);
            if (appointment.reminder != null)
            {
                reminders.Remove(appointment.reminder);
            }
        }

        public Appointment findConflictingAppointment(DateTime start, DateTime end)
        {
            Appointment temp = new Appointment("", start, end, "");
            return appointments.FirstOrDefault(a => a.conflictsWith(temp));
        }

        public GroupMeeting findGroupMeetingByNameAndDuration(string name, int duration)
        {
            return appointments.OfType<GroupMeeting>().FirstOrDefault(g => 
                g.name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                g.getDuration() == duration);
        }

        public void addUserToGroupMeeting(User user, GroupMeeting groupMeeting)
        {
            groupMeeting.addParticipant(user);
        }
    }
}
