using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LiveChatClient.tools
{
    public class InIClass
    {
            public string path;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

            public InIClass(string INIPath)
            {
                path = INIPath;
            }

            public void IniWriteValueString(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }
            public string IniReadValueString(string Section, string Key)
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
                return temp.ToString();
            }
            public int IniReadValueInt(string Section, string Key)
            {
                return (GetPrivateProfileInt(Section, Key, 0, this.path));
            }
    }
}
