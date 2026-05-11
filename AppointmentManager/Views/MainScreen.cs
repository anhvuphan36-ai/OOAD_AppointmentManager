using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using AppointmentManager.Models;
using AppointmentManager.Controllers;

namespace AppointmentManager.Views
{
    public class MainScreen : Form
    {
        private CalendarController calendarController;
        private DataGridView appointmentTable;
        private Button btnAdd;
        private System.Windows.Forms.Timer reminderTimer;
        private MonthCalendar monthCalendar;
        private Label lblHeader;
        private Button btnShowAll;
        private DateTime? selectedDate = null;

        public MainScreen(CalendarController controller)
        {
            this.calendarController = controller;
            initComponents();
            loadAppointmentList();
        }

        private void initComponents()
        {
            this.Text = "Appointment Management";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(240, 244, 248);
            this.Font = new Font("Segoe UI", 10F);

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 280,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblAppTitle = new Label
            {
                Text = "My Calendar",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 212),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft
            };

            monthCalendar = new MonthCalendar
            {
                Dock = DockStyle.Top,
                MaxSelectionCount = 1
            };
            monthCalendar.DateSelected += (s, e) => {
                selectedDate = monthCalendar.SelectionStart;
                lblHeader.Text = $"Appointments for {selectedDate.Value.ToString("dd/MM/yyyy")}";
                loadAppointmentList();
            };

            Panel spacer1 = new Panel { Dock = DockStyle.Top, Height = 20 };

            btnAdd = new Button
            {
                Text = "Add Appointment",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => onAddButtonClicked();

            Panel spacer2 = new Panel { Dock = DockStyle.Top, Height = 10 };

            btnShowAll = new Button
            {
                Text = "Show All Appointments",
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(0, 120, 212),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnShowAll.Click += (s, e) => {
                selectedDate = null;
                lblHeader.Text = "All Appointments";
                loadAppointmentList();
            };

            leftPanel.Controls.Add(btnShowAll);
            leftPanel.Controls.Add(spacer2);
            leftPanel.Controls.Add(btnAdd);
            leftPanel.Controls.Add(spacer1);
            leftPanel.Controls.Add(monthCalendar);
            leftPanel.Controls.Add(lblAppTitle);

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            lblHeader = new Label
            {
                Text = "All Appointments",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.BottomLeft
            };

            Panel spacer3 = new Panel { Dock = DockStyle.Top, Height = 15 };

            appointmentTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
            };
            appointmentTable.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 235, 250);
            appointmentTable.DefaultCellStyle.SelectionForeColor = Color.Black;
            appointmentTable.DefaultCellStyle.Padding = new Padding(5);
            appointmentTable.RowTemplate.Height = 40;
            appointmentTable.EnableHeadersVisualStyles = false;
            appointmentTable.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            appointmentTable.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 100, 100);
            appointmentTable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            appointmentTable.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            appointmentTable.ColumnHeadersHeight = 40;
            appointmentTable.CellClick += appointmentTable_CellClick;

            rightPanel.Controls.Add(appointmentTable);
            rightPanel.Controls.Add(spacer3);
            rightPanel.Controls.Add(lblHeader);

            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);

            reminderTimer = new System.Windows.Forms.Timer();
            reminderTimer.Interval = 10000;
            reminderTimer.Tick += checkReminders;
            reminderTimer.Start();
        }

        private void loadAppointmentList()
        {
            appointmentTable.DataSource = null;
            var apps = calendarController.Appointments.AsEnumerable();
            if (selectedDate.HasValue)
            {
                apps = apps.Where(a => a.StartTime.Date == selectedDate.Value.Date);
            }

            if(apps.Any()) {
                appointmentTable.DataSource = apps.Select(a => new {
                    Id = a.Id,
                    Title = a.Name,
                    Start = a.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    End = a.EndTime.ToString("dd/MM/yyyy HH:mm"),
                    Location = a.Location,
                    Type = (a is GroupMeeting) ? "Group" : "Personal", // Phân biệt loại cuộc hẹn
                    Participants = (a is GroupMeeting gm) ? gm.Participants.Count.ToString() : "-", // Đếm số lượng người tham gia
                    Reminder = a.Reminder != null ? "Yes" : "No"
                }).ToList();
                
                if (appointmentTable.Columns.Contains("Id"))
                    appointmentTable.Columns["Id"].Visible = false;
            }
        }

        private void appointmentTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (appointmentTable.Columns[e.ColumnIndex].Name == "Reminder")
                {
                    string hasReminder = appointmentTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (hasReminder == "Yes")
                    {
                        string id = appointmentTable.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                        var appt = calendarController.Appointments.FirstOrDefault(a => a.Id == id);
                        if (appt != null && appt.Reminder != null)
                        {
                            new WarningMessage(appt.Reminder.Message, "Information").display();
                        }
                    }
                }
                else if (appointmentTable.Columns[e.ColumnIndex].Name == "Participants")
                {
                    string id = appointmentTable.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    var appt = calendarController.Appointments.FirstOrDefault(a => a.Id == id);
                    if (appt is GroupMeeting gm)
                    {
                        if (gm.Participants.Count > 0)
                        {
                            // Nối tên tất cả người tham gia bằng dấu xuống dòng và in ra
                            var names = string.Join("\n", gm.Participants.Select(p => p.Name));
                            new WarningMessage($"Participants:\n{names}", "Information").display();
                        }
                        else
                        {
                            new WarningMessage("No participants joined yet.", "Information").display();
                        }
                    }
                }
            }
        }

        private void onAddButtonClicked()
        {
            var ui = new AddAppointmentUI(calendarController);
            ui.ShowDialog();
            loadAppointmentList();
        }

        private void checkReminders(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            foreach (var appt in calendarController.Appointments.ToList())
            {
                if (appt.Reminder != null && !appt.Reminder.IsSent)
                {
                    var reminderTime = appt.StartTime.AddMinutes(-appt.Reminder.TimeBefore);
                    if (now >= reminderTime)
                    {
                        appt.Reminder.sendReminder();
                        new WarningMessage($"Reminder for: {appt.Name}\n{appt.Reminder.Message}", "Information").display();
                    }
                }
            }
        }
    }
}
