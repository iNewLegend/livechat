using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace LiveChatClient.tools
{
    class etc
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern void ExitProcess(int uExitCode);
        public static void Exit()
        {
            ExitProcess(0);
        }
    }
    class use
    {
        public static int GetUnicodeCharsCount(string NoramlText)
        {
            NoramlText = NoramlText + new string((char)0x03, 1);
            UnicodeEncoding Unicode = new UnicodeEncoding();
            byte[] bytes = Unicode.GetBytes(NoramlText);
            return bytes.Length;
        }
        public static int GetIndexOfZeroArray(int StartIndex, byte[] array)
        {
            int count = 0;
            for (int i = 0; array[i + StartIndex] != 0; i++)
                count = i + 1;
            return count;
        }
        public static int GetIndexOfZeroString(int StartIndex, string Get)
        {
            int count = 0;
            for (int i = 0; Get[i + StartIndex] != 0; i++)
                count = i + 1;
            return count;
        }
        public static string GetStringFromIndexToEnd(int Index, byte[] array)
        {
            int size = GetIndexOfZeroArray(Index, array);
            char[] temp = new char[size];
            for (int i = Index; i - Index != size; i++)
                temp[i - Index] = (char)array[i];
            return new string(temp);
        }
        public static string GetStringFromIndexToEndChar(int Index, string StringToClear)
        {
            int size = GetIndexOfZeroString(Index, StringToClear);
            char[] temp = new char[size];
            for (int i = Index; i - Index != size; i++)
                temp[i - Index] = (char)StringToClear[i];
            return new string(temp);
        }
    }
}