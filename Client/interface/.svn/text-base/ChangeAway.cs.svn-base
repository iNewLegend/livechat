using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveChatClient
{
    public partial class ChangeAwayForm : Form
    {
        public ChangeAwayForm()
        {
            InitializeComponent();
        }

        private void awayStringTextbox_TextChanged(object sender, EventArgs e)
        {
            if (awayStringTextbox.Text != "")
                changeAway.Enabled = true;
            else
                changeAway.Enabled = false;
        }

        private void ChangeAwayLabel_Click(object sender, EventArgs e)
        {

        }

        private void changeAway_Click(object sender, EventArgs e)
        {
            Program.ObjConfig.PersonalMessage = awayStringTextbox.Text;
            Program.WriteFiles();
        }
    }
}
