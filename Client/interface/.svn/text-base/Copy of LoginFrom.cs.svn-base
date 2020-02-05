// ##########################################
// ## Createed By : [CzF]Leo123 (leonid vinikov)
// ## Date : 
// ## FileName : LoginFrom.cs
// ## Info : using to login 
// ##########################################

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using LiveChatClient.tools;

namespace LiveChatClient
{
    public partial class Form1 : Form
    {
        private Point mouse_offset;
        public bool OnLogged = false;
        public static ContactList CL = new ContactList();
        public static bool IconCreate = false;
        delegate void MinimizeCallBack();
        delegate void LoginChangeSateCallBack(bool state);
        public Form1()
        {
            InitializeComponent();
        }
        public void Pause(bool State)
        {
            if(!OnLogged)
            try
            {
                UsernametextBox.Enabled = State;
                PasswordtextBox.Enabled = State;
                checkBox1.Enabled = State;
                LoginButton.Enabled = State;
                SettingsButton.Enabled = State;
                Enabled = State;
            }
            catch
            {
                Thread.Sleep(1);
            }
        }
        public static void LoginStateChange(bool State)
        {
            if (Form1.UsernametextBox.InvokeRequired)
            {
                LoginChangeSateCallBack d = new LoginChangeSateCallBack(LoginStateChange);
                Form1.UsernametextBox.Invoke(d, new object[] { State });
            }
            else
            {
                UsernametextBox.Enabled = State;
                PasswordtextBox.Enabled = State;
                LoginButton.Enabled = State;
                SettingsButton.Enabled = State;
                checkBox1.Enabled = State;
            }

 
        }
        private void Minimize()
        {
            if (InvokeRequired)
            {
                MinimizeCallBack CallBacker = new MinimizeCallBack(Minimize);
                Invoke(CallBacker);
            }
            else
                Hide();
        }
        private void cThread()
        {
            while (true)
            {
                if (!Program.ProgramState)
                    break;
                if (Program.IsMinimize)
                {
                    Minimize();
                    Program.IsMinimize = false;
                }
                Thread.Sleep(1);
            }
            Tray.CloseIcon();
            etc.Exit();

        }
        private void Init()
        {
            UsernametextBox.Text = "";
            UsernametextBox.MaxLength = 16;
            PasswordtextBox.Text = "";
            PasswordtextBox.MaxLength = 16;
            PasswordtextBox.PasswordChar = '*';
            if (!IconCreate)
            {
                IconCreate = true;
                Tray.CreateIcon();
            }
            Thread t = new Thread(new ThreadStart(cThread));
            t.Start();
            UsernametextBox.Text = Program.ObjConfig.Username;
            PasswordtextBox.Text = Program.ObjConfig.Password;
            checkBox1.Checked = Convert.ToBoolean(Program.ObjConfig.IsRem);
            // Create the ToolTip and associate with the Form container.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(LoginButton, "Login to server.");
            toolTip1.SetToolTip(SettingsButton, "Open settings window.");
            toolTip1.SetToolTip(UsernametextBox, "Type your LiveChat username in here.");
            toolTip1.SetToolTip(PasswordtextBox, "Type your LiveChat password in here.");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (UsernametextBox.Text.Length < 1)
            {
                Pause(false);
                MessageBox.Show("Error: username required.");
                Pause(true);
                return;
            }
            if (PasswordtextBox.Text.Length < 1)
            {
                MessageBox.Show("Error: password required.");
                return;
            }
            Protocol.OBJ.AccountName = UsernametextBox.Text;
            Protocol.OBJ.AccountPassword = PasswordtextBox.Text;
            if (checkBox1.Checked)
            {
                Program.ObjConfig.Username = Protocol.OBJ.AccountName;
                Program.ObjConfig.Password = Protocol.OBJ.AccountPassword;
                Program.ObjConfig.IsRem = 1;
                Program.WriteFiles();
            }
            else
            {
                Program.ObjConfig.Username = "";
                Program.ObjConfig.Password = "";
                Program.ObjConfig.IsRem = 0;
                Program.WriteFiles();
            }
            OnLogged = true;
            LoginStateChange(false);
            Protocol.SendLogin();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mPostion = Control.MousePosition;
                mPostion.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mPostion;
            }
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Program.ProgramState = false;
        }

        private void MiniButton_Click(object sender, EventArgs e)
        {
            Tray.FromState = false;
            Program.IsMinimize = true;
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TopPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
