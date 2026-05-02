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

        public MainScreen(User currentUser)
        {
            calendarController = new CalendarController(currentUser);
            initComponents();
            loadAppointmentList();
        }

        private void initComponents()
        {
            this.Text = "Appointment Management";
            this.Size = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);

            appointmentTable = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(590, 300),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            appointmentTable.CellClick += appointmentTable_CellClick;
            this.Controls.Add(appointmentTable);

            btnAdd = new Button
            {
                Text = "Add Appointment",
                Location = new Point(20, 340),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += (s, e) => onAddButtonClicked();
            this.Controls.Add(btnAdd);
        }

        private void loadAppointmentList()
        {
            appointmentTable.DataSource = null;
            if(calendarController.appointments.Any()) {
                appointmentTable.DataSource = calendarController.appointments.Select(a => new {
                    Id = a.id,
                    Title = a.name,
                    Start = a.startTime.ToString("dd/MM/yyyy HH:mm"),
                    End = a.endTime.ToString("dd/MM/yyyy HH:mm"),
                    Location = a.location,
                    Reminder = a.reminder != null ? "Yes" : "No"
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
                        var appt = calendarController.appointments.FirstOrDefault(a => a.id == id);
                        if (appt != null && appt.reminder != null)
                        {
                            MessageBox.Show(appt.reminder.message, "Reminder Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
