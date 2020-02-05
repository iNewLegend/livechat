using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LiveChatClient.tools
{
    public static class BufferClass
    {

        public struct Buffer
        {
            private byte[] BufferData;
            public int Index;
            public int Length;

            public Buffer(int Size)
            {
                BufferData = new byte[Size];
                Index = 0;
                Length = BufferData.GetLength(0);
            }
            public Buffer(byte[] ArrayOfBytes)
            {
                BufferData = new byte[ArrayOfBytes.Length];
                ArrayOfBytes.CopyTo(BufferData, 0);
                Index = 0;
                Length = BufferData.Length;
            }

            public Buffer(int Size, byte[] ArrayOfBytes)
            {
                BufferData = new byte[Size];
                ArrayOfBytes.CopyTo(BufferData, 0);
                Index = 0;
                Length = BufferData.Length;
            }

            public string ReadStringToSeperator(byte sperator)
            {
                int spec = FindByte(sperator);
                string Temp = ReadString(spec);

                return Temp;
            }
            private void Resize(int Deltasize)
            {
                byte[] newbuffer = new byte[Length + Deltasize];
                BufferData.CopyTo(newbuffer, 0);
                BufferData = newbuffer;
                Length = BufferData.Length;
            }


            public byte[] GetBuffer()
            {
                return BufferData;
            }

            public byte[] GetWrittenBuffer()
            {
                try
                {
                    if (Index > Length)
                    {
                        byte[] result = new byte[Index - 1];
                        System.Buffer.BlockCopy(BufferData, 0, result, 0, Index - 1);
                        return result;
                    }
                    else
                    {
                        byte[] result = new byte[Index];
                        System.Buffer.BlockCopy(BufferData, 0, result, 0, Index);
                        return result;
                    }
                }
                catch
                {
                    MessageBox.Show(String.Format("Index is out of bound. Index = {0} Length = {1} ", Index, Length));
                    return new byte[1];
                }
            }

            public bool setIndex(int nIndex)
            {
                if (Index > Length) return false;
                Index = nIndex;
                return true;
            }

            public bool addIndex(int nIndex)
            {
                return setIndex(Index + nIndex);
            }

            public void resetIndex()
            {
                setIndex(0);
            }

            //Write methods

            public void WriteByte(byte Byte)
            {
                if (Index >= Length) Resize((Index - Length) + 1);
                BufferData[Index] = Byte;
                ++Index;
            }

            public void WriteShort(ushort ShortInteger)
            {
                if (Index + 2 >= Length) Resize((Index - Length) + 2);
                BufferData[Index] = (byte)(ShortInteger);
                ++Index;
                BufferData[Index] = (byte)(ShortInteger / 256);
                ++Index;
            }

            public void WriteInt(uint Integer)
            {
                /*
                 * This stuff is suppose to happen too, but C# doesnt have power operator >_>
                for (int i = 0; i < 4; i++)
                {
                    BufferData[Index] = (byte)((Integer/(256^i)))
                }
                Index += 4;
                 */

                if (Index + 4 >= Length) Resize((Index - Length) + 4);
                BufferData[Index] = (byte)(Integer);
                ++Index;
                BufferData[Index] = (byte)(Integer / 256);
                ++Index;
                BufferData[Index] = (byte)(Integer / 65536);
                ++Index;
                BufferData[Index] = (byte)(Integer / 16777216);
                ++Index;
            }

            public void BytePad(byte Pad, int nLength)
            {
                if ((Index + nLength) >= Length) Resize(nLength);
                for (int i = 0; i < nLength; i++)
                {
                    BufferData[Index + i] = Pad;
                }
                Index += nLength;
            }

            public void WriteString(string String)
            {
                if ((Index + String.Length) >= Length) Resize(String.Length);
                for (int i = 0; i < String.Length; i++)
                {
                    BufferData[Index + i] = (byte)String[i];
                }
                Index += String.Length;
            }
            public void WriteStringShort(string String)
            {
                for(int i = 0 ; i != String.Length ; i++)
                    WriteShort(String[i]);
            }

            public void WriteStringLen(string String)
            {
                if ((Index + String.Length + 2) >= Length) Resize(String.Length + 2);
                WriteShort((ushort)String.Length);
                WriteString(String);
            }
            public void WriteStringLenA(string String, int Len)
            {
                for (int i = 0; i != Len; i++)
                    if (i >= String.Length)
                        WriteByte(0x00);
                    else
                        WriteByte((byte)String[i]);

            }
            public void WriteStringPaddedLeft(string String, char Pad, int PadLength)
            {
                String.PadLeft(PadLength, Pad);
                if ((Index + String.Length) >= Length) Resize(String.Length);
                WriteString(String);
            }

            public void WriteStringPaddedRight(string String, char Pad, int PadLength)
            {
                String.PadRight(PadLength, Pad);
                if ((Index + String.Length) >= Length) Resize(String.Length);
                WriteString(String);
            }


            public void WriteArray(byte[] Array)
            {
                if ((Index + Array.Length) >= Length) Resize(Array.Length);
                for (int i = 0; i < Array.Length; i++)
                {
                    BufferData[Index + i] = Array[i];
                }
                Index += Array.Length;
            }

            public void WriteHexString(string HexString)
            {
                int _out;
                byte[] _byte = HexEncoding.GetBytes(HexString, out _out);
                WriteArray(_byte);
            }
            public void WriteUnicodeString(string UniString)
            {
                UniString = UniString + new string((char)0x03, 1);
                UnicodeEncoding Unicode = new UnicodeEncoding();
                byte[] bytes = Unicode.GetBytes(UniString);
                WriteArray(bytes);
            }
            //Read methods

            public byte ReadByte()
            {
                try
                {
                    byte result = BufferData[Index];
                    ++Index;
                    return result;
                }
                catch
                {
                    MessageBox.Show(String.Format("Trying to read out of data boundaries. Size = {0} ReadIndex = {1} ReadSize = 2", Length, Index));
                    return 0;
                }
            }


            public ushort ReadShort()
            {
                try
                {
                    ushort result = (ushort)((BufferData[Index]) + (BufferData[Index + 1] * 256));
                    Index += 2;
                    return result;
                }
                catch
                {
                    MessageBox.Show(String.Format("Trying to read out of data boundaries. Size = {0} ReadIndex = {1} ReadSize = 2", Length, Index));
                    return 0;
                }
            }

            public ushort ReadShort(ushort Pos)
            {
                int TempIndex = Index;
                Index = Pos;
                ushort result = ReadShort();
                Index = TempIndex;
                return result;
            }


            public int ReadInt()
            {
                try
                {
                    int result = (BufferData[Index]) + (BufferData[Index + 1] * 256) + (BufferData[Index + 2] * 65536) + (BufferData[Index + 3] * 16777216);
                    Index += 4;
                    return result;
                }
                catch
                {
                    MessageBox.Show(String.Format("Trying to read out of data boundaries. Size = {0} ReadIndex = {1} ReadSize = 4", Length, Index));
                    return 0;
                }
            }

            public int ReadInt(int Pos)
            {
                int TempIndex = Index;
                Index = Pos;
                int result = ReadInt();
                Index = TempIndex;
                return result;
            }
            public string ReadStringShort(int _length)
            {
                ushort[] newbuf = new ushort[_length];
                for (int i = 0; i != _length; i++)
                    newbuf[i] = ReadShort();
                Byte[] bytes = new Byte[newbuf.Length];  // Assumption - only need one byte per ushort
                int ii = 0;
                foreach (ushort x in newbuf)
                {
                    byte[] tmp = System.BitConverter.GetBytes(x);
                    bytes[ii++] = tmp[0];
                    // Note: not using tmp[1] as all characters in 0 < x < 127 use one byte.
                }
                return Encoding.ASCII.GetString(bytes);

            }
            public string ReadUnicodeString(int cIndex, int Count)
            {
                UnicodeEncoding d = new UnicodeEncoding();
                int charCount = d.GetCharCount(BufferData, cIndex, Count);
                char[] chars = new Char[charCount];
                d.GetChars(BufferData, cIndex, Count, chars, 0);
                return new string(chars);

            }
            public string ReadString(int _length)
            {
                char[] newbuf = new char[_length];
                try
                {
                    for (int i = 0; i < _length; i++)
                    {
                        newbuf[i] = (char)BufferData[Index + i];
                    }
                    Index += _length;
                    return new string(newbuf);
                }
                catch
                {
                    MessageBox.Show(String.Format("Trying to read out of data boundaries. Size = {0} ReadIndex = {1} ReadSize = {2}", Length, Index, _length));
                    return "";
                }
            }


            public string ReadStringFromLength()
            {
                int _length = ReadShort();
                return ReadString(_length);
            }
            public int FindByte(byte ToFind)
            {
                int Len = 0;
                int IndexSave = Index;
                while (Len <= Length && ReadByte() != ToFind)
                    Len++;
                Index = IndexSave;
                return Len;
            }

            public string ReadStringFromLength(int Pos)
            {
                int TempIndex = Index;
                Index = Pos;
                string result = ReadStringFromLength();
                Index = TempIndex;
                return result;
            }

            public string ReadStringSpecify(int Pos, int nLength)
            {
                int TempIndex = Index;
                Index = Pos;
                string result = ReadString(nLength);
                Index = TempIndex;
                return result;
            }

            public void ShowBuffer()
            {
                MessageBox.Show(String.Format("Buffer : "));
                for (int i = 0; i < Length; i++)
                {
                    if (i == Index)
                        Utils.SetConsoleColour(12);
                    Console.Write("{0:X} ", BufferData[i]);
                    if (i == Index)
                        Utils.SetConsoleColour(7);
                }
                MessageBox.Show(String.Format(""));
            }

            public void ShowBufferPos(int Pos)
            {
                MessageBox.Show(String.Format("Buffer : "));
                for (int i = 0; i < Length; i++)
                {
                    if (i == Pos)
                        Utils.SetConsoleColour(12);
                    Console.Write("{0:X} ", BufferData[i]);
                    if (i == Pos)
                        Utils.SetConsoleColour(7);
                }
                //MessageBox.Show("");
            }


        }
        class HexEncoding
        {
            /* External code from http://www.codeproject.com/KB/recipes/hexencoding.aspx */
            /* Author = neilck http://www.codeproject.com/script/Membership/Profiles.aspx?mid=375133 */
            public static bool IsHexDigit(Char c)
            {
                int numChar;
                int numA = Convert.ToInt32('A');
                int num1 = Convert.ToInt32('0');
                c = Char.ToUpper(c);
                numChar = Convert.ToInt32(c);
                if (numChar >= numA && numChar < (numA + 6))
                    return true;
                if (numChar >= num1 && numChar < (num1 + 10))
                    return true;
                return false;
            }

            private static byte HexToByte(string hex)
            {
                if (hex.Length > 2 || hex.Length <= 0)
                    throw new ArgumentException("hex must be 1 or 2 characters in length");
                byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                return newByte;
            }

            public static byte[] GetBytes(string hexString, out int discarded)
            {
                discarded = 0;
                string newString = "";
                char c;
                // remove all none A-F, 0-9, characters
                for (int i = 0; i < hexString.Length; i++)
                {
                    c = hexString[i];
                    if (IsHexDigit(c))
                        newString += c;
                    else
                        discarded++;
                }
                // if odd number of characters, discard last character
                if (newString.Length % 2 != 0)
                {
                    discarded++;
                    newString = newString.Substring(0, newString.Length - 1);
                }

                int byteLength = newString.Length / 2;
                byte[] bytes = new byte[byteLength];
                string hex;
                int j = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    hex = new String(new Char[] { newString[j], newString[j + 1] });
                    bytes[i] = HexToByte(hex);
                    j = j + 2;
                }
                return bytes;
            }
            /* End external code */
        }
    }
    class Utils
    {
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(uint nStdHandle);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section,
                 string key, int ndefault, string filePath);

        public static int ReadINIInt(string section,
                 string key, int ndefault, string filePath)
        {
            return GetPrivateProfileInt(section, key, ndefault, filePath);
        }

        public static string ReadINIString(string section,
                         string key, string def, int size, string filePath)
        {
            StringBuilder Output = new StringBuilder(size);
            GetPrivateProfileString(section, key, def, Output, size, filePath);
            return Output.ToString();
        }

        public static bool SetConsoleColour(int Colour)
        {
            if (Colour == 0) return false;
            return SetConsoleTextAttribute(GetStdHandle(0xfffffff5), Colour);
        }

        public static void MoveBytes(byte[] Bytes, int Index)
        {
            for (int i = 0; i < Bytes.Length - (Index); i++)
            {
                Bytes[i] = Bytes[i + Index];
            }
        }

        public static byte[] ReplaceBytes(byte[] Bytes, int Index)
        {
            if ((Bytes.Length - Index) == 0) return new byte[1] { 00 };
            byte[] result = new byte[Bytes.Length - Index];
            System.Buffer.BlockCopy(Bytes, Index, result, 0, Bytes.Length - Index);
            return result;
        }

        public static void CopyBytes(byte[] Destination, byte[] Source)
        {
            System.Buffer.BlockCopy(Source, 0, Destination, 0, Source.Length);
        }

        public static void CopyBytes(byte[] Destination, byte[] Source, int Count)
        {
            System.Buffer.BlockCopy(Source, 0, Destination, 0, Count);
        }

        public static void CopyBytes(byte[] Destination, int DestinationIndex, byte[] Source, int SourceIndex, int Count)
        {
            System.Buffer.BlockCopy(Source, SourceIndex, Destination, DestinationIndex, Count);
        }
    }
}