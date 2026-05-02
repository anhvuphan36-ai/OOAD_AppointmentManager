using System;
using System.Windows.Forms;
using System.Drawing;
using AppointmentManager.Models;
using AppointmentManager.Controllers;

namespace AppointmentManager.Views
{
    public class AddAppointmentUI : Form
    {
        private DateTime currentDate;
        private DateTime currentTime;
        
        private CalendarController calendarController;
        
        private TextBox txtName; 
        private DateTimePicker dtpStart; 
        private DateTimePicker dtpEnd; 
        private TextBox txtLocation; 
        private CheckBox chkReminder;
        private TextBox txtReminderMessage;
        private Button btnSave; 
        private Button btnCancel; 

        public AddAppointmentUI(CalendarController calendarController)
        {
            this.calendarController = calendarController;
            showAddAppointmentWindow(DateTime.Today, DateTime.Now);
        }

        public void showAddAppointmentWindow(DateTime date, DateTime time)
        {
            this.currentDate = date;
            this.currentTime = time;

            this.Text = "Add Appointment";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblTitle = new Label { Text = "Schedule Event", Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), Location = new Point(25, 15), AutoSize = true, ForeColor = Color.FromArgb(0, 120, 212) };

            Label lblTitleTxt = new Label { Text = "Name:", Location = new Point(30, 65), AutoSize = true };
            txtName = new TextBox { Location = new Point(140, 62), Width = 210 };

            Label lblStart = new Label { Text = "Start Time:", Location = new Point(30, 105), AutoSize = true };
            dtpStart = new DateTimePicker { Location = new Point(140, 102), Width = 210, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", MinDate = DateTime.Today };

            Label lblEnd = new Label { Text = "End Time:", Location = new Point(30, 145), AutoSize = true };
            dtpEnd = new DateTimePicker { Location = new Point(140, 142), Width = 210, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", MinDate = DateTime.Today };

            Label lblLoc = new Label { Text = "Location:", Location = new Point(30, 185), AutoSize = true };
            txtLocation = new TextBox { Location = new Point(140, 182), Width = 210 };

            chkReminder = new CheckBox { Text = "Add Reminder", Location = new Point(30, 225), AutoSize = true };
            chkReminder.CheckedChanged += (s, e) => txtReminderMessage.Enabled = chkReminder.Checked;
            txtReminderMessage = new TextBox { Location = new Point(140, 255), Width = 210, Enabled = false, PlaceholderText = "Reminder Message" };

            btnSave = new Button { Text = "Save", Location = new Point(140, 310), Width = 100, Height = 40, BackColor = Color.FromArgb(0, 120, 212), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSave.Click += (s, e) => {
                var appt = getUserInput();
                if (validateInput(appt))
                {
                    Appointment conflict = calendarController.findConflictingAppointment(appt.startTime, appt.endTime);
                    if (conflict != null)
                    {
                        string choice = showWarning($"Conflict with '{conflict.name}'.\n[Yes] to Replace old.\n[No] to enter New Time.", "Conflict");
                        if (choice == "Replace")
                        {
                            calendarController.removeAppointment(conflict);
                            calendarController.addAppointment(appt);
                            showConfirmation("Appointment replaced successfully.");
                            closeWindow();
                        }
                        return;
                    }

                    GroupMeeting gm = calendarController.findGroupMeetingByNameAndDuration(appt.name, appt.getDuration());
                    if (gm != null)
                    {
                        string choice = showWarning($"Group meeting '{appt.name}' found. Join it?", "Group");
                        if (choice == "Join")
                        {
                            gm.addParticipant(calendarController.user);
                            showConfirmation("Joined group meeting successfully.");
                            closeWindow();
                            return;
                        }
                    }

                    calendarController.addAppointment(appt);
                    if (chkReminder.Checked)
                    {
                        appt.addReminder(txtReminderMessage.Text);
                    }
                    showConfirmation("Appointment created successfully.");
                    closeWindow();
                }
            };

            btnCancel = new Button { Text = "Cancel", Location = new Point(250, 310), Width = 100, Height = 40, FlatStyle = FlatStyle.Flat };
            btnCancel.Click += (s, e) => closeWindow();

            this.Controls.AddRange(new Control[] { lblTitle, lblTitleTxt, txtName, lblStart, dtpStart, lblEnd, dtpEnd, lblLoc, txtLocation, chkReminder, txtReminderMessage, btnSave, btnCancel });
        }

        public Appointment getUserInput()
        {
            return new Appointment(txtName.Text, dtpStart.Value, dtpEnd.Value, txtLocation.Text);
        }

        public bool validateInput(Appointment appointmentData)
        {
            if (string.IsNullOrWhiteSpace(appointmentData.name))
            {
                MessageBox.Show("Name is required.", "Error");
                return false;
            }
            if (!appointmentData.isValid())
            {
                MessageBox.Show("Invalid time range or time is in the past.", "Error");
                return false;
            }
            return true;
        }

        public string showWarning(string message, string type)
        {
            WarningMessage warning = new WarningMessage(message, type);
            return warning.getUserChoice();
        }

        public bool showConfirmation(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        public void closeWindow()
        {
            this.Close();
        }
    }
}
