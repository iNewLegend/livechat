using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveChatClient
{
    public partial class FindUsersForm : Form
    {
        public static ListViewHitTestInfo ListItemSelected;
        public static string SelectNick = "";
        public delegate void ChangeResultsDataSourceCallback(string ID, string Username, string Usernick, string email);
        public FindUsersForm()
        {
            InitializeComponent();
        }
        public Packets.USER_DEFINE GetOptionValue()
        {
            if (byID.Checked)
                return Packets.USER_DEFINE.USER_ID;
            if (byNickName.Checked)
                return Packets.USER_DEFINE.USER_NICK;
            if (byEmail.Checked)
                return Packets.USER_DEFINE.USER_EMAIL;
            return Packets.USER_DEFINE.USER_ID;
        }
        private void searchUser_Click(object sender, EventArgs e)
        {
            UsersList.Items.Clear();
            Protocol.SendFindUser(searchString.Text, GetOptionValue());
        }

        private void searchString_TextChanged(object sender, EventArgs e)
        {
            if (searchString.Text != "")
                searchUser.Enabled = true;
            else
                searchUser.Enabled = false;         
        }

        public void AddUser(string ID,string Username,string Usernick,string email)
        {
            if (UsersList.InvokeRequired)
            {
                ChangeResultsDataSourceCallback CallBack = new ChangeResultsDataSourceCallback(AddUser);
                UsersList.Invoke(CallBack, new object[] { ID,Username,Usernick,email });
            }
            else
            {
                ListViewItem CurItem = UsersList.Items.Add(ID);
                CurItem.SubItems.Add(Username);
                CurItem.SubItems.Add(Usernick);
                CurItem.SubItems.Add(email);
            }
        }

        private void FindUsersForm_Load(object sender, EventArgs e)
        {

        }

        private void UsersList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    ListItemSelected = UsersList.HitTest(e.X, e.Y);
                }
                catch
                {
                    return;
                }
                Protocol.SendContactRequest(int.Parse(ListItemSelected.Item.Text));
            }
        }

    }
}
