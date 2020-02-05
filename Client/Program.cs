// ##########################################
// ## Devloper : [CzF]Leo123 (leonid vinikov)
// ## Date : 
// ## FileName : Program.cs
// ## Info : using to load&read files and run app , connect to server
// ## Program : LiveChat Main Client.
// ##########################################

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using LiveChatClient.tools;
namespace LiveChatClient
{
    static class Program
    {
        public static LoginForm connect;
        public static bool ProgramState = true;
        public static bool IsMinimize = false;
        public static Config ObjConfig = new Config();
        public static InIClass ConfigFile = new InIClass("./Config.ini");
        //public static TextIni.TextIni LangReservoir = new TextIni.TextIni(@"Lang\default.txt");
        delegate void ShutDownCallback();
        static void Main()
        {                                               
            ReadFiles();
            //LangReservoir.LoadFile();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!Protocol.SendConnect())
            {
                //MessageBox.Show(LangReservoir.GetValue(0));
                MessageBox.Show("Can't connect to the server , please try later");
                Application.Exit();
                return;
            }
            Application.Run(( connect = new LoginForm()) );
        }

        public static void ShutDown()
        {
            if (connect.InvokeRequired)
            {
                ShutDownCallback d = new ShutDownCallback(ShutDown);
                connect.Invoke(d, null);
            }
            else
                connect.Close();
        }

        public static void ReadFiles()
        {
            ObjConfig.ServerAddress = ConfigFile.IniReadValueString("Server", "ServerAddress");
            ObjConfig.ServerPort = ConfigFile.IniReadValueInt("Server", "ServerPort");
            ObjConfig.Username = ConfigFile.IniReadValueString("Login", "Username");
            ObjConfig.Password = ConfigFile.IniReadValueString("Login", "Password");
            ObjConfig.IsRem = ConfigFile.IniReadValueInt("Login", "IsRem");
            ObjConfig.PersonalMessage = ConfigFile.IniReadValueString("ETC", "PersonalMessage");
        }
        public static void WriteFiles()
        {
            ConfigFile.IniWriteValueString("Server", "ServerAddress", ObjConfig.ServerAddress);
            ConfigFile.IniWriteValueString("Server", "ServerPort", ObjConfig.ServerPort.ToString());
            ConfigFile.IniWriteValueString("Login", "Username", ObjConfig.Username);
            ConfigFile.IniWriteValueString("Login", "Password", ObjConfig.Password);
            ConfigFile.IniWriteValueString("Login", "IsRem", ObjConfig.IsRem.ToString());
            ConfigFile.IniWriteValueString("ETC", "PersonalMessage", ObjConfig.PersonalMessage);
        }
        public struct Config
        {
            public string ServerAddress;
            public int ServerPort;
            public string Username;
            public string Password;
            public int IsRem;
            public string PersonalMessage;
        }
    }
}
