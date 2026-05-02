using System;
using System.Windows.Forms;

namespace AppointmentManager.Views
{
    public class WarningMessage
    {
        public string messageText { get; set; }
        public string type { get; set; } 

        public WarningMessage(string messageText, string type)
        {
            this.messageText = messageText;
            this.type = type;
        }

        public void display()
        {
            MessageBox.Show(messageText, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string getUserChoice()
        {
            if (type == "Conflict")
            {
                var res = MessageBox.Show(messageText, "Conflict Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes) return "Replace";
                if (res == DialogResult.No) return "NewTime";
                return "Cancel";
            }
            else if (type == "Group")
            {
                var res = MessageBox.Show(messageText, "Group Meeting Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return res == DialogResult.Yes ? "Join" : "Ignore";
            }
            return "Cancel";
        }
    }
}
