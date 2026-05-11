using System;
using System.Windows.Forms;

namespace AppointmentManager.Views
{
    public class WarningMessage
    {
        private string _messageText;
        public string MessageText { get { return _messageText; } set { _messageText = value; } }
        
        private string _type;
        public string Type { get { return _type; } set { _type = value; } }

        public WarningMessage(string messageText, string type)
        {
            this.MessageText = messageText;
            this.Type = type;
        }

        public void display()
        {
            if (Type == "Error")
            {
                MessageBox.Show(MessageText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Type == "Success")
            {
                MessageBox.Show(MessageText, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(MessageText, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public string getUserChoice()
        {
            if (Type == "Conflict")
            {
                var res = MessageBox.Show(MessageText, "Conflict Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes) return "Replace";
                if (res == DialogResult.No) return "NewTime";
                return "Cancel";
            }
            else if (Type == "Group")
            {
                // Mở hộp thoại cảnh báo phát hiện trùng Group Meeting
                var res = MessageBox.Show(MessageText, "Group Meeting Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return res == DialogResult.Yes ? "Join" : "Ignore";
            }
            return "Cancel";
        }
    }
}
