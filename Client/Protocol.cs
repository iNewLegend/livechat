/* 
 * ###################################################################################################################
 * ## Developer : [CzF]Leo123 (leonid vinikov)
 * ## MSN : loniao@walla.co.il 
 * ## email : leo1234u@gmail.com
 * ## Create Date : 00:52 ‎01/‎07/‎2008
 * ## Last Edit : 1:32 PM 1/17/2009
 * ## FileName : Protocol.cs
 * ## Info : using to speak with server.
 * ## FileVersion : 0.6
 * ###################################################################################################################
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using LiveChatClient.tools;

namespace LiveChatClient
{
    class Protocol
    {
        public static User OBJ = new User();
        public static ContactList cList = new ContactList();
        public static LoginForm lFm = new LoginForm();
        public static Thread ServerThread = new Thread(delegate() { ServerThreadr(); });
        public static FindUsersForm searchUsersForm = new FindUsersForm();

        static void ServerThreadr()
        {
            byte[] Packet;
            int Temp = 0;
            TcpClient Connection = OBJ.Connection;
            try
            {
                Temp = Connection.Client.Available;
            }
            catch
            {
                MessageBox.Show("Conncetion Closed by LiveChat Server.");
                Program.ProgramState = false;
                Application.Exit();
            }
            while (Connection.Client.Connected && Program.ProgramState)
            {
                Packet = new byte[Connection.Client.Available];
                try
                {
                    Connection.Client.Receive(Packet);
                }
                catch
                {
                    MessageBox.Show("Conncetion Closed by LiveChat Server.");
                    Program.ProgramState = false;
                    Application.Exit();
                }
                if (Packet.Length > 0)
                {
                    byte PacketType = Packet[0];
                    int PacketSize = Packet[1];
                    byte ProtoNum = Packet[2];
                    Protocol.ProtocolCore(PacketSize, (Packets.PacketTypes)PacketType, (Packets.PROTO_ENUM)ProtoNum, Packet);
                    Thread.Sleep(1);
                }
            }
        }

        public static bool ProtocolCore(int aLen, Packets.PacketTypes ProtocolType, Packets.PROTO_ENUM ProtoNum, byte[] RecvLong)
        {
            byte SubProto = RecvLong[3];
            byte[] Recv = new byte[aLen];
            try
            {
                for (int i = 0; i != aLen; i++)
                    Recv[i] = RecvLong[i];
            }
            catch(Exception x) 
            {
                OBJ.LastError = string.Format("{0}\n{1}\n{2}",x.Message,x.Data,x.Source);
                return false;
            }
            switch (ProtocolType)
            {
                #region C1
                case Packets.PacketTypes.C1_SIZE_BYTE:
                    switch (ProtoNum)
                    {
                        #region login
                        case Packets.PROTO_ENUM.PROTO_LOGIN: // Login
                             {
                                switch (Recv[4])
                                {
                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_FINE:
                                            SendRequestNickID();
                                            break;
                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_INVALIDACCOUNT:
                                            MessageBox.Show("Your account not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_INVALIDPASSWORD:
                                            MessageBox.Show("Your password is invaild.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_CONNECTIONERROR:
                                            MessageBox.Show("An error occurred while trying to connect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;
                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_INVALIDVERSION:
                                            MessageBox.Show("Your LiveChat version is obsolete, please upgrade.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_NOCHARGEINFO:
                                            MessageBox.Show("111");
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_SERVERISFULL:
                                            MessageBox.Show("The server is already at capacity, please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_TOMANYATTEMPTS:
                                            MessageBox.Show("Too many connection attempts, please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_AGERESTRICTS:
                                            MessageBox.Show("ererre");
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_ACCOUNTINUSE:
                                            MessageBox.Show("Your account is already connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LoginForm.LoginStateChange(true);
                                            break;

                                    case (byte)Packets.ACCOUNTJOIN_RESULT.JR_ACCOUNTBLOCKED:
                                            MessageBox.Show("This account is currently blocked.");
                                            LoginForm.LoginStateChange(true);
                                            break;
                                     default:
                                            MessageBox.Show("Unrecognized error detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            break;
                                }
                                break;
                             }
                        #endregion login

                        #region ContactList
                        case Packets.PROTO_ENUM.PROTO_CONTACTLIST:  // Contact List
                            {
                                switch (RecvLong[3])
                                {
                                    case (byte)Packets.R_SUBPROTO_CONTACTLIST.CONTACTLIST_AddContact: // Add Contact
                                        {
                                            BufferClass.Buffer RecvPacket = new BufferClass.Buffer(aLen);
                                            for (int i = 0; i < aLen; i++)
                                                RecvPacket.WriteByte(RecvLong[i]);
                                            RecvPacket.setIndex(4);
                                            bool State = Convert.ToBoolean(RecvPacket.ReadByte());
                                            int id = RecvPacket.ReadInt();
                                            string Nick = RecvPacket.ReadString(20);
                                            cList.AddContact(Nick, State, id);
                                            break;
                                        }
                                    case (byte)Packets.R_SUBPROTO_CONTACTLIST.CONTACTLIST_ChangeContactState: // offline // online
                                        {
                                            BufferClass.Buffer RecvPacket = new BufferClass.Buffer(aLen);
                                            for (int i = 0; i < aLen; i++)
                                                RecvPacket.WriteByte(RecvLong[i]);
                                            RecvPacket.setIndex(4);
                                            int id = RecvPacket.ReadInt();
                                            bool State = Convert.ToBoolean(RecvPacket.ReadByte());
                                            cList.ChangeContactState(id, State);
                                            break;
                                        }
                                    case (byte)Packets.R_SUBPROTO_CONTACTLIST.CONTACTLIST_AddContacts: // geting all contact list as one packet
                                        {
                                            BufferClass.Buffer RecvPacket = new BufferClass.Buffer(aLen);
                                            for (int i = 0; i < aLen; i++)
                                                RecvPacket.WriteByte(RecvLong[i]);
                                            RecvPacket.setIndex(4);
                                            string ContactNick;
                                            int id;
                                            bool State;
                                            while (RecvPacket.Index < RecvPacket.Length)
                                            {
                                                id = RecvPacket.ReadInt();
                                                State = Convert.ToBoolean(RecvPacket.ReadByte());
                                                ContactNick = RecvPacket.ReadString(RecvPacket.FindByte(0xFF)).TrimEnd('\0'); // Tirmend used to delete 0x00 from nick(string)
                                                RecvPacket.ReadByte();
                                                cList.AddContact(ContactNick, State, id);
                                            }
                                            break;
                                        }
                                    case (byte)Packets.R_SUBPROTO_CONTACTLIST.CONTACTLIST_ShowContactList: // Open Contact List From
                                        {
                                            Tray.FromState = false;
                                            Program.IsMinimize = true;
                                            ContactList.ContactList_State = true;
                                            cList.ShowContactList();
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion ContactList

                        #region Message
                        case Packets.PROTO_ENUM.PROTO_MESSAGE: // Message
                            {
                                switch (RecvLong[3])
                                {
                                    case (byte)Packets.R_SUBPROTO_MESSAGE.MESSAGE_SendMessage: // recv a new msg from server (Other contact)
                                        {
                                            BufferClass.Buffer RecvBuff = new BufferClass.Buffer(RecvLong);
                                            RecvBuff.WriteArray(Recv);
                                            RecvBuff.setIndex(4);
                                            int SenderId = RecvBuff.ReadInt();
                                            int Size = RecvBuff.ReadInt();
                                            string message = RecvBuff.ReadUnicodeString(RecvBuff.Index, Size);
                                            ContactList.MSG.WriteMessage(SenderId, message);
                                            break;
                                        }
                                    case (byte)Packets.R_SUBPROTO_MESSAGE.MESSAGE_TypingUser: // change is typign state 
                                        {
                                            BufferClass.Buffer RecvBuff = new BufferClass.Buffer(aLen);
                                            for (int i = 0; i != aLen; i++)
                                                RecvBuff.WriteByte(RecvLong[i]);
                                            RecvBuff.setIndex(0);
                                            RecvBuff.ReadByte();
                                            RecvBuff.ReadByte();
                                            RecvBuff.ReadByte();
                                            RecvBuff.ReadByte();
                                            bool State = Convert.ToBoolean(RecvBuff.ReadByte());
                                            int TyperId = RecvBuff.ReadInt();
                                            ContactList.MSG.ChangeTypingState(TyperId, State);
                                            break;
                                        }
                                    case (byte)Packets.R_SUBPROTO_MESSAGE.MESSAGE_SendMSGBOX:
                                        {
                                            BufferClass.Buffer RecvBuff = new BufferClass.Buffer(RecvLong);
                                            RecvBuff.WriteArray(Recv);
                                            RecvBuff.setIndex(4);
                                            MessageBoxButtons mBoxType = (MessageBoxButtons)RecvBuff.ReadInt();
                                            string message = RecvBuff.ReadStringToSeperator(0x00);
                                            MessageBox.Show(message, "LiveChat Server!", mBoxType);
                                            break;
                                        }
                                    case (byte)Packets.R_SUBPROTO_MESSAGE.MESSAGE_AWAY:
                                        {
                                            BufferClass.Buffer RecvBuff = new BufferClass.Buffer(RecvLong);
                                            RecvBuff.WriteArray(Recv);
                                            RecvBuff.setIndex(4);
                                            int RequestID = RecvBuff.ReadInt();
                                            int Size = RecvBuff.ReadInt();
                                            string message = RecvBuff.ReadUnicodeString(RecvBuff.Index, Size-2);
                                            ContactList.MSG.WriteAway(RequestID, message);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion Message

                        #region Profile
                        case Packets.PROTO_ENUM.PROTO_PROFIL: // profile
                            {
                                switch (RecvLong[3]) // sub proto num 
                                {
                                    case 0x00: // Geting my profile image
                                        {
                                            string profile_image_path = String.Format("data\\{0}\\profile.jpg", OBJ.id);
                                            if (File.Exists(profile_image_path))
                                                File.Delete(profile_image_path);
                                            BinaryWriter FileImage = new BinaryWriter(File.Open(profile_image_path, FileMode.CreateNew));
                                            byte[] ImageBytes = new byte[RecvLong.Length-4];
                                            for (int i = 0; i != ImageBytes.Length; i++)
                                                ImageBytes[i] = RecvLong[i + 4];
                                            FileImage.Write(ImageBytes);
                                            FileImage.Close();
                                            Thread.Sleep(100);
                                            cList.SetProfileImage(profile_image_path);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion Profile

                        #region Other
                        case Packets.PROTO_ENUM.PROTO_OTHER: // Other things
                            {
                                switch ((Packets.SUBPROTO_OTHER)SubProto)
                                {
                                    case Packets.SUBPROTO_OTHER.OTHER_RequestNickID:  // Geting nick and id
                                        {
                                            BufferClass.Buffer RecvBuff = new BufferClass.Buffer(RecvLong);
                                            RecvBuff.WriteArray(Recv);
                                            RecvBuff.setIndex(4);
                                            OBJ.id = RecvBuff.ReadInt();
                                            OBJ.Nick = RecvBuff.ReadStringToSeperator(0);
                                            break;
                                        }
                                    case Packets.SUBPROTO_OTHER.OTHER_FindUser:
                                        {
                                            BufferClass.Buffer RecvBuff = new BufferClass.Buffer(RecvLong);
                                            RecvBuff.WriteArray(Recv);
                                            RecvBuff.setIndex(6);
                                            string ID = String.Format("{0}", RecvBuff.ReadByte());
                                            string NickName = RecvBuff.ReadString(20);
                                            string UserName = RecvBuff.ReadString(10);
                                            string Email = RecvBuff.ReadString(25);
                                            searchUsersForm.AddUser(ID, UserName, NickName, Email);
                                            break;
                                        }
                                }
                                break;
                            }
                        #endregion Other
                    }
                    break;
                #endregion C1
            }
            return true;
        }

        #region SendFunctions
        public static bool SendConnect()
        {
            TcpClient Connection = new TcpClient();
            try
            {
                Connection.Connect(IPAddress.Parse(Program.ObjConfig.ServerAddress), Program.ObjConfig.ServerPort);
            }
            catch // if connecting to normal ip fail
            {
                try // try to get ip from DNS & connect to geted ip.
                {
                    IPAddress[] ip = Dns.GetHostAddresses(Program.ObjConfig.ServerAddress);
                    Connection.Connect(ip, Program.ObjConfig.ServerPort);
                }
                catch   // if this fail too return false program will not start
                {
                    return false;
                }
            }
            ServerThread.Start();
            OBJ.Connection = Connection;
            return true;
        }
        public static void SendLogin()
        {
            BufferClass.Buffer DataSend = new BufferClass.Buffer(22);
            DataSend.setIndex(0);
            DataSend.WriteStringLenA(OBJ.AccountName, 10);
            DataSend.WriteStringLenA(OBJ.AccountPassword, 10);
            SendData(OBJ.Connection, (byte)Packets.PROTO_ENUM.PROTO_LOGIN, DataSend.GetWrittenBuffer());
        }
        public static void SendContactListRequest()
        {
            byte[] DataSend = new byte[1];
            DataSend[0] = (byte)Packets.S_SUBPROTO_CONTACTLIST.CONTACTLIST_ReuqestContactList;
            SendData(OBJ.Connection,  Packets.PROTO_ENUM.PROTO_CONTACTLIST, DataSend);
        }
        public static void SendContactRequest(int ID)
        {
            BufferClass.Buffer SendBuff = new BufferClass.Buffer(6);
            SendBuff.WriteByte((byte)Packets.S_SUBPROTO_CONTACTLIST.CONTACTLIST_RequestContact);
            SendBuff.WriteInt((uint)ID);
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_CONTACTLIST, SendBuff.GetWrittenBuffer());
        }
        public static void SendRequestProfileImage()
        {
            /*
            byte[] DataSend = new byte[2];
            DataSend[0] = 0x0;
            DataSend[1] = 0x0;
            SendData(OBJ.Connection, 0x3, DataSend);*/
        }
        public static void SendData(TcpClient Connection, Packets.PROTO_ENUM PacketType, byte[] Buff)
        {
            byte[] SendData = new byte[Buff.Length + 3];
            SendData[0] = 0xC1;
            SendData[1] = (byte)(Buff.Length + 3);
            SendData[2] = (byte)PacketType;
            int index = 3;
            for (int i = 0; i != Buff.Length; i++)
                SendData[index++] = Buff[i];
            Connection.Client.Send(SendData);
        }
        public static bool SendMessage(int id, string message)
        {
            int Unicode_Message_Len = use.GetUnicodeCharsCount(message);
            BufferClass.Buffer SendBuff = new BufferClass.Buffer(Unicode_Message_Len+9);
            SendBuff.WriteByte((byte)Packets.S_SUBPROTO_MESSAGE.MESSAGE_SendMessage);
            SendBuff.WriteInt((uint)id);
            SendBuff.WriteInt((uint)Unicode_Message_Len);
            SendBuff.WriteUnicodeString(message);
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_MESSAGE, SendBuff.GetWrittenBuffer());
            return true;
        }
        public static void SendTyping(int id,bool State)
        {
            BufferClass.Buffer SendBuff = new BufferClass.Buffer(5);
            SendBuff.WriteByte((byte)Packets.S_SUBPROTO_MESSAGE.MESSAGE_TypingUser);
            SendBuff.WriteByte(Convert.ToByte(State));
            SendBuff.WriteInt((uint)id);
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_MESSAGE, SendBuff.GetWrittenBuffer());
        }
        public static void SendChangeAway(string message)
        {
            int Unicode_Message_Len = use.GetUnicodeCharsCount(message);
            BufferClass.Buffer SendBuff = new BufferClass.Buffer(Unicode_Message_Len + 5);
            SendBuff.WriteByte((byte)Packets.S_SUBPROTO_MESSAGE.MESSAGE_ChangeAway);
            SendBuff.WriteInt((uint)Unicode_Message_Len);
            SendBuff.WriteUnicodeString(message);
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_MESSAGE, SendBuff.GetWrittenBuffer());
        }
        public static void SendRequestAway(int ID)
        {
            BufferClass.Buffer SendBuff = new BufferClass.Buffer(5);
            SendBuff.WriteByte((byte)Packets.S_SUBPROTO_MESSAGE.MESSAGE_RequestAway);
            SendBuff.WriteInt((uint)ID);
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_MESSAGE, SendBuff.GetWrittenBuffer());
        }
        public static void SendRequestNickID()
        {
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_OTHER , new byte[1] { (byte)Packets.SUBPROTO_OTHER.OTHER_RequestNickID });
        }
        public static void SendFindUser(string toSearch,Packets.USER_DEFINE Option)
        {
            BufferClass.Buffer SendBuff = new BufferClass.Buffer(24);
            SendBuff.WriteByte((byte)Packets.SUBPROTO_OTHER.OTHER_FindUser);
            SendBuff.WriteByte((byte)Option);
            SendBuff.WriteStringLenA(toSearch, 20);
            SendData(OBJ.Connection, Packets.PROTO_ENUM.PROTO_OTHER, SendBuff.GetWrittenBuffer());
        }
        #endregion SendFunctions
    }
    public struct User
    {
        public int aIndex;
        public TcpClient Connection;
        public int id;
        public string AccountName;
        public string AccountPassword;
        public string Nick;
        public string LastError;
        public int LastId;
        public System.Drawing.Image MyImage;
    }
}