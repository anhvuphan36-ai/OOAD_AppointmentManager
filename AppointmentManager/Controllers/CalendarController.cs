using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentManager.Models;

namespace AppointmentManager.Controllers
{
    public class CalendarController
    {
        private List<Appointment> _appointments;
        public List<Appointment> Appointments { get { return _appointments; } set { _appointments = value; } }

        private List<Reminder> _reminders;
        public List<Reminder> Reminders { get { return _reminders; } set { _reminders = value; } }

        private List<User> _users;
        public List<User> Users { get { return _users; } set { _users = value; } }

        private User _currentUser;
        public User CurrentUser { get { return _currentUser; } set { _currentUser = value; } }

        public CalendarController()
        {
            Appointments = new List<Appointment>();
            Reminders = new List<Reminder>();
            Users = new List<User>();
        }

        public bool ValidateAppointment(Appointment appointment)
        {
            if (string.IsNullOrWhiteSpace(appointment.Name))
            {
                return false;
            }
            if (appointment.StartTime < DateTime.Now || appointment.EndTime <= appointment.StartTime)
            {
                return false;
            }
            return true;
        }

        public int GetDuration(Appointment appointment)
        {
            return (int)(appointment.EndTime - appointment.StartTime).TotalMinutes;
        }

        public bool CheckConflict(Appointment a1, Appointment a2)
        {
            return (a1.StartTime < a2.EndTime && a1.EndTime > a2.StartTime);
        }

        public bool AddAppointment(Appointment appointment)
        {
            Appointments.Add(appointment);
            if (appointment.Reminder != null)
            {
                Reminders.Add(appointment.Reminder);
                appointment.Reminder.scheduleReminder(appointment.StartTime);
            }
            return true;
        }

        public void RemoveAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
            if (appointment.Reminder != null)
            {
                Reminders.Remove(appointment.Reminder);
            }
        }

        public void ReplaceAppointment(Appointment oldApp, Appointment newApp)
        {
            RemoveAppointment(oldApp);
            AddAppointment(newApp);
        }

        public Appointment FindConflictingAppointment(DateTime start, DateTime end)
        {
            Appointment temp = new Appointment("", start, end, "");
            return Appointments.FirstOrDefault(a => CheckConflict(a, temp));
        }

        public GroupMeeting FindGroupMeetingByNameAndDuration(string name, int duration)
        {
            return Appointments.OfType<GroupMeeting>().FirstOrDefault(g => 
                g.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                GetDuration(g) == duration);
        }

        public void AddUserToGroupMeeting(User user, GroupMeeting groupMeeting)
        {
            groupMeeting.addParticipant(user);
        }
    }
}
