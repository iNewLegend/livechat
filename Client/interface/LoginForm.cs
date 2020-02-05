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
    public partial class LoginForm : Form
    {
        private Point Mouse_Offset;
        public bool OnLogged = false;
        public static ContactList CL = new ContactList();
        public static bool IconCreate = false;
        delegate void MinimizeCallBack();
        delegate void LoginChangeSateCallBack(bool state);
        public LoginForm()
        {
            InitializeComponent();
        }
        public void Pause(bool State)
        {
            if (!OnLogged)
                try
                {
                    username_box.Enabled = State;
                    password_box.Enabled = State;
                    remember_me.Enabled = State;
                    sing_in.Enabled = State;
                    settings.Enabled = State;
                    Enabled = State;
                }
                catch
                {
                    Thread.Sleep(1);
                }
        }
        public static void LoginStateChange(bool State)
        {
            if (LoginForm.username_box.InvokeRequired)
            {
                LoginChangeSateCallBack d = new LoginChangeSateCallBack(LoginStateChange);
                LoginForm.username_box.Invoke(d, new object[] { State });
            }
            else
            {
                username_box.Enabled = State;
                password_box.Enabled = State;
                sing_in.Enabled = State;
                settings.Enabled = State;
                remember_me.Enabled = State;
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
            username_box.Text = "";
            username_box.MaxLength = 16;
            password_box.Text = "";
            password_box.MaxLength = 16;
            password_box.PasswordChar = '*';
            if (!IconCreate)
            {
                IconCreate = true;
                Tray.CreateIcon();
            }
            Thread t = new Thread(new ThreadStart(cThread));
            t.Start();
            username_box.Text = Program.ObjConfig.Username;
            password_box.Text = Program.ObjConfig.Password;
            remember_me.Checked = Convert.ToBoolean(Program.ObjConfig.IsRem);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                Point mPostion = Control.MousePosition;
                mPostion.Offset(Mouse_Offset);
                this.Location = mPostion;
            }
            
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Mouse_Offset = new Point(-e.X, -e.Y);
           
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void sing_in_Click(object sender, EventArgs e)
        {
            if (username_box.Text.Length < 1)
            {
                Pause(false);
                MessageBox.Show("Error: username required.");
                Pause(true);
                return;
            }
            if (password_box.Text.Length < 1)
            {
                MessageBox.Show("Error: password required.");
                return;
            }
            Protocol.OBJ.AccountName = username_box.Text;
            Protocol.OBJ.AccountPassword = password_box.Text;
            if (remember_me.Checked)
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

        private void Close_Click(object sender, EventArgs e)
        {
            Program.ProgramState = false;
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            Tray.FromState = false;
            Program.IsMinimize = true;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Mouse_Offset = new Point(-e.X, -e.Y);
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mPostion = Control.MousePosition;
                mPostion.Offset(Mouse_Offset);
                this.Location = mPostion;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void Maximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void minimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }
        // ********************************************************************************************* //
        // ** Close Button
        // ********************************************************************************************* //
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            Close.Image = Properties.Resources.BT_close_over;
        }
        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Close.Image = Properties.Resources.BT_close;
        }

        private void Close_MouseDown(object sender, MouseEventArgs e)
        {
            Close.Image = Properties.Resources.BT_close_click;
        }

        private void Close_MouseUp(object sender, MouseEventArgs e)
        {
            Close.Image = Properties.Resources.BT_close;
        }
        // ********************************************************************************************* //
        // ** Maximize Button
        // ********************************************************************************************* //
        private void Maximize_MouseDown(object sender, MouseEventArgs e)
        {
            Maximize.Image = Properties.Resources.BT_maximize_click;
        }

        private void Maximize_MouseEnter(object sender, EventArgs e)
        {
            Maximize.Image = Properties.Resources.BT_maximize_over;
        }

        private void Maximize_MouseLeave(object sender, EventArgs e)
        {
            Maximize.Image = Properties.Resources.BT_maximize;
        }

        private void Maximize_MouseUp(object sender, MouseEventArgs e)
        {
            Maximize.Image = Properties.Resources.BT_maximize;
        }
        // ********************************************************************************************* //
        // ** Minimize Button
        // ********************************************************************************************* //
        private void minimize_MouseDown(object sender, MouseEventArgs e)
        {
            minimize.Image = Properties.Resources.BT_minmize_click;
        }

        private void minimize_MouseEnter(object sender, EventArgs e)
        {
            minimize.Image = Properties.Resources.BT_minmize_over;
        }

        private void minimize_MouseLeave(object sender, EventArgs e)
        {
            minimize.Image = Properties.Resources.BT_minimize;
        }

        private void minimize_MouseUp(object sender, MouseEventArgs e)
        {
            minimize.Image = Properties.Resources.BT_minimize;
        }
        
    }
}
