/* 
 * ###################################################################################################################
 * ## Developer : [CzF]Leo123 (leonid vinikov)
 * ## Sub Developer : Ventox
 * ## MSN : loniao@walla.co.il 
 * ## email : leo1234u@gmail.com
 * ## Create Date : 07:04 ‎09/‎07/‎2008
 * ## Last Edit : 07:04 ‎09/‎07/‎2008
 * ## FileName : Messages.cs
 * ## Info : to speak with contact lists
 * ## FileVersion : 0.5
 * ###################################################################################################################
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LiveChatClient
{
    public partial class messages : Form
    {
        delegate void MsgWriteCallBack(int senderid,string message);
        delegate void ChangeTypingStateCallBack(int TyperId, bool state);
        public static int OpendCount = 0;
        public static int CurrOpen = 0;
        public int index;
        public messages()
        {
            InitializeComponent();
        }
        private void WriteRTB(RichTextBox rtb,string s, Color colr, Font f)
        {
            rtb.SelectionStart = rtb.Text.Length;
            rtb.SelectionLength = 0;
            rtb.SelectionColor = colr;
            rtb.SelectionFont = f;
            rtb.SelectedText = s;
        }
        public void OpenDialog(int index)
        {
            if (!ContactObjects.ContactStruct[index].IsMessagsOpend)
            {
                OpendCount++;
                ContactObjects.ContactStruct[index].IsMessagsOpend = true;
                ContactObjects.ContactStruct[index].MessageDialog = new messages();
                ContactObjects.ContactStruct[index].MessageDialog.index = index;
                Thread dialog = new Thread(DialogCore);
                CurrOpen = index;
                dialog.Start();
            }
            else
                ContactObjects.ContactStruct[index].MessageDialog.Activate();
        }
        public void DialogCore()
        {
            ContactObjects.ContactStruct[CurrOpen].MessageDialog.ShowDialog();
        }
        public void AddTime(int index)
        {
            ContactObjects.ContactStruct[index].MessageDialog.ServerText.Text += string.Format("\n({0}:{1})", DateTime.Now.Hour, DateTime.Now.Minute);
        }
        public void SendMsg(string message, int id)
        {
            if (this.MeText.Text.Length == 0)
            {
                MessageBox.Show("You can't send empty message");
                return;
            }
            if (this.MeText.Text.Length < 500 && CheckLinesIn(message) < 10)
            {
                this.AddTime(this.index);
                this.ServerText.Text += String.Format(" {0} Says: {1}", Protocol.OBJ.Nick, message);
                this.MeText.Clear();
                Protocol.SendMessage(id, message);
            }
            else
                MessageBox.Show("The message is too big.");
        }
        public void WriteAway(int ID, string message)
        {
            int index = ContactObjects.GetIndexById(ID);
            if (ContactObjects.IsOpendMessage(index))
            {
                AddTime(index);
                string Nick = ContactObjects.ContactStruct[index].Nick;

                WriteRTB(ContactObjects.ContactStruct[index].MessageDialog.ServerText, String.Format("{0} personal message is : \n{1}", Nick, message), Color.Red, ServerText.Font);
            }
        }
        public void WriteMessage(int senderid,string message)
        {
            
            int index = ContactObjects.GetIndexById(senderid);
            if (ContactObjects.IsOpendMessage(index))
            {
                ContactObjects.ContactStruct[index].MessageDialog.Activate();
                AddTime(index);
                string format = ContactObjects.ContactStruct[index].Nick + " Says: " + message;
                WriteRTB(ContactObjects.ContactStruct[index].MessageDialog.ServerText, message, Color.Red, ServerText.Font);
            }
            else
            {
                OpenDialog(index);
                Thread.Sleep(100);
                WriteMessage(senderid, message);  
            }
        }
        public void ChangeState(int index, bool state)
        {
            if (ContactObjects.ContactStruct[index].IsMessagsOpend)
            {
                ContactObjects.ContactStruct[index].MessageDialog.Text = string.Format("{0} ({1}) - LiveChat Message Session", ContactObjects.ContactStruct[index].Nick, ContactObjects.GetConnected(index));
                AddTime(index);
                ContactObjects.ContactStruct[index].MessageDialog.ServerText.Text += String.Format("{0} Now is {1}", ContactObjects.ContactStruct[index].Nick, ContactObjects.GetConnected(index));
            }
        }
        public void ChangeTypingState(int TyperId, bool State)
        {
            int TyperIndex = ContactObjects.GetIndexById(TyperId);
            if (ContactObjects.ContactStruct[TyperIndex].IsMessagsOpend)
            {
                if (ContactObjects.ContactStruct[TyperIndex].MessageDialog.StatusBox.InvokeRequired)
                {
                    ChangeTypingStateCallBack thisthread = new ChangeTypingStateCallBack(ChangeTypingState);
                    ContactObjects.ContactStruct[TyperIndex].MessageDialog.Invoke(thisthread, new object[] { TyperId, State });
                }
                else
                {
                    ContactObjects.ContactStruct[TyperIndex].MessageDialog.StatusBox.Visible = State;
                    ContactObjects.ContactStruct[TyperIndex].MessageDialog.StatusBox.Text = string.Format("{0} is typeing ...", ContactObjects.ContactStruct[TyperIndex].Nick);
                }
            }

        }
        public int CheckLinesIn(string message)
        {
            int NewLinesCount = 0;
            for (int i = 0; i != message.Length; i++)
                if (message[i] == '\n')
                    NewLinesCount++;
            return NewLinesCount;
        }
        private void messages_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} ({1}) - LiveChat Message Session", ContactObjects.ContactStruct[this.index].Nick, ContactObjects.GetConnected(this.index));
            this.ServerText.Text = string.Format("LiveChat {0} ({1}) ...", ContactObjects.ContactStruct[this.index].Nick, ContactObjects.GetConnected(this.index));
            this.MePictureBox.Image = Protocol.OBJ.MyImage;
            Protocol.SendRequestAway(ContactObjects.ContactStruct[this.index].id);
            // TO DO : HERE I NEED REUST THE profile IMAGE OF MAN THAT I SPEAK WITH IM
        }

        private void MeText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) // If ENTER PRESSD
            {
                char[] ClearMsg = new char[this.MeText.Text.Length];
                for (int i = 0; i != this.MeText.Text.Length - 1; i++)
                    ClearMsg[i] = this.MeText.Text[i];
                SendMsg(new string(ClearMsg), ContactObjects.ContactStruct[this.index].id);
            }
        }

        private void MeText_TextChanged(object sender, EventArgs e)
        {
            if (this.MeText.Text.Length == 1)
                Protocol.SendTyping(ContactObjects.ContactStruct[this.index].id,true);
            if (this.MeText.Text.Length == 0)
                Protocol.SendTyping(ContactObjects.ContactStruct[this.index].id,false);
            this.Charstextbox.Text = string.Format("{0}/250", this.MeText.Text.Length);
            if (this.MeText.Text.Length >= 250)
                this.Charstextbox.ForeColor = Color.Red;
            else
                this.Charstextbox.ForeColor = Color.FromArgb(0, 35, 70);
        }

        private void messages_FormClosed(object sender, FormClosedEventArgs e)
        {
            int index = this.index;
            ContactObjects.ContactStruct[index].IsMessagsOpend = false;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            
        }

        private void ServerText_TextChanged(object sender, EventArgs e)
        {
            //move to last line
            ServerText.SelectionStart = ServerText.Text.Length;
            ServerText.Focus();
            ServerText.ScrollToCaret();

            //focus passed to input box
            MeText.Focus();
            
        }

        private void SendButton_Click_1(object sender, EventArgs e)
        {
            SendMsg(this.MeText.Text, ContactObjects.ContactStruct[this.index].id);
        }
    }
}
