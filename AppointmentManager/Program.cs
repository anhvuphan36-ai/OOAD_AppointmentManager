using System;
using System.Windows.Forms;
using AppointmentManager.Views;
using AppointmentManager.Models;
using AppointmentManager.Controllers;

namespace AppointmentManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            User initUser = new User("U1", "Admin", "admin@test.com");
            CalendarController controller = new CalendarController();
            controller.Users.Add(initUser);
            controller.CurrentUser = initUser;
            Application.Run(new MainScreen(controller));
        }
    }
}