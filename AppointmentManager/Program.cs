using System;
using System.Windows.Forms;
using AppointmentManager.Views;
using AppointmentManager.Models;

namespace AppointmentManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            User initUser = new User("U1", "Admin", "admin@test.com");
            Application.Run(new MainScreen(initUser));
        }
    }
}