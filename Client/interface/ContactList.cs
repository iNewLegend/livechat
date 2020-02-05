using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Threading;
  

namespace LiveChatClient
{
    public partial class ContactList : Form
    {
        public static int MaxContacts = 1000;
        private Point Mouse_Loction;
        public static int AddedCount = 0;
        public static bool ContactList_State = false;
        public static bool ContactList_IsMini = false;
        public static string SelectNick = "";
        public static ListViewHitTestInfo ListItemSelected;
        public FontDialog dFont = new FontDialog();
        private static Tray TrayApp = new Tray();
        public static bool FristTimeOpen = false;
        public static messages MSG = new messages();
        delegate void AddContactCallBack(int Local_Indexer,string contact,bool state);
        delegate void ItemChangeStateCallBack(int id, bool state);
        TaskbarNotifier taskbarNotifier;
        ChangeAwayForm change = new ChangeAwayForm();
        public ContactList()
        {
            InitializeComponent();
        }
        public void ShowWelcome()
        {
            taskbarNotifier.Show("Welcome LiveChat", string.Format("{0} your connected to LiveChat server and you got {1} contacts", Protocol.OBJ.Nick, ContactlistView.Items.Count), 2500, 2500, 2500);
        }
        public void Popupinit()
        {
            taskbarNotifier = new TaskbarNotifier();
            taskbarNotifier.SetBackgroundBitmap(Properties.Resources.popup_skin,Color.FromArgb(255, 0, 255));
            taskbarNotifier.SetCloseBitmap(Properties.Resources.popup_close,Color.FromArgb(255, 0, 255), new Point(127, 8));
            taskbarNotifier.TitleRectangle = new Rectangle(40, 9, 70, 25);
            taskbarNotifier.ContentRectangle = new Rectangle(8, 41, 133, 68);
        }

        public void AddItem(int Local_Index, string Contact, bool State)
        {
            if (ContactlistView.InvokeRequired)
            {
                AddContactCallBack ContactThread = new AddContactCallBack(AddItem);
                Invoke(ContactThread, new object[] { Local_Index, Contact, State });
            }
            else
            {
                if (State)
                {
                    ContactObjects.ContactStruct[Local_Index].item = ContactlistView.Items.Insert(Local_Index, Contact, 0);
                    ContactObjects.ContactStruct[Local_Index].item.Group = ContactObjects.GroupsStruct.OnlineGroup;
                    ContactObjects.ContactStruct[Local_Index].item.ForeColor = Color.Green;
                }
                else
                {
                    ContactObjects.ContactStruct[Local_Index].item = ContactlistView.Items.Insert(Local_Index, Contact, 1);
                    ContactObjects.ContactStruct[Local_Index].item.Group = ContactObjects.GroupsStruct.OfflineGroup;
                    ContactObjects.ContactStruct[Local_Index].item.ForeColor = Color.Red;
                }
                foreach (ColumnHeader ch in ContactlistView.Columns)
                {
                    ch.Width = -Contact.Length;
                }
                ShowWelcome();
            }
        }
        public void AddContact(string ContactName, bool State, int id)
        {
            if (ContactList_State)
            {
                ContactObjects.ContactStruct[AddedCount].IsConneced = State;
                ContactObjects.ContactStruct[AddedCount].Nick = ContactName;
                ContactObjects.ContactStruct[AddedCount].id = id;
                ContactObjects.ContactStruct[AddedCount].index = AddedCount;
                AddItem(AddedCount, ContactName, State);
                AddedCount++;
            }
        }
        public void ItemChangeState(int local_index, bool State)
        {
            if (InvokeRequired)
            {
                ItemChangeStateCallBack Call = new ItemChangeStateCallBack(ItemChangeState);
                Invoke(Call, new object[] { local_index, State });
            }
            else
            {
                string Nick = ContactObjects.ContactStruct[local_index].Nick;
                ContactlistView.Items.RemoveAt(local_index);
                taskbarNotifier.Show(Nick, string.Format("{0} now is {1}", Nick, ContactObjects.GetConnected(local_index)), 2000, 2000, 2000);
                if (State)
                {
                    ContactObjects.ContactStruct[local_index].item = ContactlistView.Items.Insert(local_index, ContactObjects.ContactStruct[local_index].Nick, 0);
                    ContactObjects.ContactStruct[local_index].item.Group = ContactObjects.GroupsStruct.OnlineGroup;
                    ContactObjects.ContactStruct[local_index].item.ForeColor = Color.Green;
                }
                else
                {
                    ContactObjects.ContactStruct[local_index].item = ContactlistView.Items.Insert(local_index, ContactObjects.ContactStruct[local_index].Nick, 1);
                    ContactObjects.ContactStruct[local_index].item.Group = ContactObjects.GroupsStruct.OfflineGroup;
                    ContactObjects.ContactStruct[local_index].item.ForeColor = Color.Red;
                }
                MSG.ChangeState(local_index, State);
            }
        }
        public void ChangeContactState(int id, bool state)
        {
            if (ContactList_State)
            {
                int index = ContactObjects.GetIndexById(id);
                ContactObjects.ContactStruct[index].IsConneced = state;
                ItemChangeState(index, state);
            }
        }

        public void DialogShower()
        {
            ShowDialog();
        }
        public void ShowContactList()
        {
            NickLabel.Text = Protocol.OBJ.Nick;
            Thread dialog = new Thread(DialogShower);
            dialog.Start();
            Thread.Sleep(30);
            Protocol.SendContactListRequest();
        }
        public void SetProfileImage(string path)
        {
            MyPictureBox.Image = Image.FromFile(path);
            Protocol.OBJ.MyImage = MyPictureBox.Image;
        }
        private void ContactList_Load(object sender, EventArgs e)
        {
            PersonalMessage.Text = Program.ObjConfig.PersonalMessage;
            if(PersonalMessage.Text.Length > 0)
                Protocol.SendChangeAway(PersonalMessage.Text);
            ContactObjects.GroupsStruct.OnlineGroup = ContactlistView.Groups.Add("0", "Connected");
            ContactObjects.GroupsStruct.OfflineGroup = ContactlistView.Groups.Add("1","Disconnected");
            ContactObjects.GroupsStruct.OfflineGroup.HeaderAlignment = HorizontalAlignment.Center;
            ContactObjects.GroupsStruct.OnlineGroup.HeaderAlignment = HorizontalAlignment.Center;
            if (File.Exists(string.Format("data\\{0}\\profile.jpg", Protocol.OBJ.id)))
            {
                Protocol.OBJ.MyImage = Image.FromFile(string.Format("data\\{0}\\profile.jpg", Protocol.OBJ.id)); // we load  image to picture box.
                MyPictureBox.Image = Protocol.OBJ.MyImage;
            }
            else
                //Protocol.SendRequestProfileImage();
            Popupinit();
        }
        private void messageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = ContactObjects.GetIndexByNick(SelectNick);
            MSG.OpenDialog(index);
        }
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Loction = new Point(-e.X, -e.Y); 
        }
        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mPostion = Control.MousePosition;
                mPostion.Offset(Mouse_Loction.X, Mouse_Loction.Y);
                Location = mPostion;
            }
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            ContactList.ContactList_IsMini = true;
            Hide();
        }

        private void ContactlistView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    ListItemSelected = ContactlistView.HitTest(e.X, e.Y);
                }
                catch
                {
                    return;
                }
                SelectNick = ListItemSelected.Item.Text;
                MSG.OpenDialog(ContactObjects.GetIndexByNick(SelectNick));
            }
        }

        private void ContactlistView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                char[] Nick = new char[30];
                try
                {
                    ContactlistView.ContextMenuStrip.Show(ContactlistView, new Point(e.X, e.Y));
                    ListItemSelected = ContactlistView.HitTest(e.X, e.Y);
                    SelectNick = ListItemSelected.Item.Text;
                    for (int i = 0; i != SelectNick.Length; i++)
                        if (ListItemSelected.Item.Text[i] == ':')
                            break;
                        else
                            Nick[i] = ListItemSelected.Item.Text[i];
                    toolStripTextBox1.Text = new string(Nick);
                }
                catch
                {
                    ContactlistView.ContextMenuStrip.Hide();
                }


            }
        }

        public static int CallCount = 0;
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
  
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void AwayString_DoubleClick(object sender, EventArgs e)
        {
            change.changeAway.Click += new EventHandler(changeAway_Click);
            change.closeForm.Click += new EventHandler(closeForm_Click);
            change.ShowDialog();
        }

        private void changeAway_Click(object sender, EventArgs e)
        {
            PersonalMessage.Text = change.awayStringTextbox.Text;
            change.Close();
            if (PersonalMessage.Text.Length > 0)
                Protocol.SendChangeAway(PersonalMessage.Text);
        }

        private void closeForm_Click(object sender, EventArgs e)
        {
            change.Close();
        }
        public void ShowFindUsers()
        {
            Protocol.searchUsersForm.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {

            new Thread(new ThreadStart(ShowFindUsers)).Start();
        }

        private void NickLabel_Click(object sender, EventArgs e)
        {

        }

        private void ContactlistView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PersonalMessage_Click(object sender, EventArgs e)
        {

        }
    }
    class ContactObjects
    {
        public static ContactObj[] ContactStruct = new ContactObj[ContactList.MaxContacts];
        public static GroupsObj GroupsStruct = new GroupsObj();
        public static string GetConnected(int local_index)
        {
            if (ContactStruct[local_index].IsConneced)
                return "Online";
            return "Offline";
        }
        public static int GetIndexByNick(string Nick)
        {
            for (int i = 0; i != ContactList.MaxContacts; i++)
                if (ContactStruct[i].Nick == Nick)
                    return i ;
            return -1;
        }
        public static int GetIndexById(int id)
        {
            for (int i = 0; i != ContactList.MaxContacts; i++)
                if (ContactStruct[i].id == id)
                    return i;
            return -1;
        }
        public static int GetIdByNick(string Nick)
        {
            for (int i = 0; i != ContactList.MaxContacts; i++)
                if (ContactStruct[i].Nick == Nick)
                    return i;
            return -1;
        }
        public static bool IsOpendMessage(int index)
        {
            return ContactStruct[index].IsMessagsOpend;
        }
        public static string RemoveCharacters(string s, string removeChars)
        {
            int i = 0, j = 0;

            int lengthC = removeChars.Length;
            int lengthS = s.Length;
            int[] intCollection = new int[256];
            char[] s2 = new char[lengthS];

            for (i = 0; i < lengthC; i++)
            {
                intCollection[removeChars[i]] = 1;
            }

            i = j = 0;
            for (i = 0; i < lengthS; i++)
            {
                if (intCollection[s[i]] != 1)
                {
                    s2[j] = s[i];
                    j++;
                }
            }

            return new string(s2);

        }
        public static string GetNickById(int id)
        {
            for (int i = 0; i != ContactList.MaxContacts; i++)
                if (ContactStruct[i].id == id)
                    return ContactStruct[i].Nick;
            return "";
        }
        public struct ContactObj
        {
            public int index;                               // local index
            public int id;                                  // id in server
            public bool IsConneced;                         // is user online?
            public bool IsMessagsOpend;                     // Is you open a message window to tallk with a contact
            public string Nick;                             // Nick
            public ListViewItem item;                       // Item
            public messages MessageDialog;                  // His message dialog.
        }
        public struct GroupsObj
        {
            public ListViewGroup OnlineGroup;
            public ListViewGroup OfflineGroup;
        }
    }
}
