// ##########################################
// ## Createed By : [CzF]Leo123 (leonid vinikov)
// ## Date : 
// ## FileName : Tray.cs
// ## Info : using to minimize appliction to tray
// ## And show Tray menu
// ##########################################

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using LiveChatClient.Properties;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Resources;

namespace LiveChatClient
{
    public class IconMenuItem : MenuItem
    {
        Icon m_Icon;
        Font m_Font;
        public IconMenuItem() : this("", null, null, Shortcut.None) { }
        public IconMenuItem(string text, Icon icon, EventHandler onClick, Shortcut shortcut) :
            base(text, onClick, shortcut)
        {
            OwnerDraw = true;
            m_Font = new Font("Comic Sans MS", 8);
            m_Icon = icon;
        }
        protected override void Dispose(bool disposing)
        {
            try
            {
                m_Font.Dispose();
                m_Font = null;
                m_Icon.Dispose();
                m_Icon = null;
                base.Dispose();
            }
            catch
            {
                if (!Program.ProgramState)
                    Application.ExitThread();
            }
        }

        public Icon Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            StringFormat sf = new StringFormat();

            sf.HotkeyPrefix = HotkeyPrefix.Show;
            sf.SetTabStops(60, new float[] { 0 });

            base.OnMeasureItem(e);

            e.ItemHeight = 22;
            e.ItemWidth = (int)e.Graphics.MeasureString(GetRealText(), m_Font, 10000, sf).Width + 10;
            sf.Dispose();
            sf = null;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            Brush br;
            bool fDisposeBrush = false;


            if (m_Icon != null)
                e.Graphics.DrawIcon(m_Icon, e.Bounds.Left + 2, e.Bounds.Top + 2);

            Rectangle rcBk = e.Bounds;
            rcBk.X += 24;

            if ((e.State & DrawItemState.Selected) != 0)
            {
                br = new LinearGradientBrush(rcBk, SystemColors.Highlight, SystemColors.Control, 0f);
                fDisposeBrush = true;
            }
            else
                br = SystemBrushes.Control;

            e.Graphics.FillRectangle(br, rcBk);
            // Only Dispose the brush if we created it, not if it was retrieved from SystemBrushes
            if (fDisposeBrush)
                br.Dispose();

            br = null;

            StringFormat sf = new StringFormat();
            sf.HotkeyPrefix = HotkeyPrefix.Show;
            sf.SetTabStops(60, new float[] { 0 });
            br = new SolidBrush(e.ForeColor);
            e.Graphics.DrawString(GetRealText(), m_Font, br, e.Bounds.Left + 25, e.Bounds.Top + 2, sf);
            br.Dispose();
            br = null;
            sf.Dispose();
            sf = null;

        }

        private string GetRealText()
        {
            string s = Text;

            // Append shortcut if one is set and it should be visible
            if (ShowShortcut && (Shortcut != Shortcut.None))
            {
                // To get a string representation of a Shortcut value, cast
                // it into a Keys value and use the KeysConverter class (via TypeDescriptor).
                Keys k = (Keys)Shortcut;
                s += "\t" + TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(k);
            }

            return s;
        }

    }
    class Tray
    {
        private static NotifyIcon nIcon = new NotifyIcon();
        public static bool FromState = true;
        private static LoginForm lFrom = new LoginForm();
        public static ContextMenu my_menu = new ContextMenu();
        public static void CreateIcon()
        {
            nIcon.Icon = Resources.browser;
            nIcon.Text = "LiveChat";
            nIcon.Visible = true;
            my_menu.MenuItems.Add(new IconMenuItem("Minimize LiveChat", Resources.Mini, Minimize, Shortcut.None));
            my_menu.MenuItems.Add(new IconMenuItem("Open LiveChat", Resources.Add, Open, Shortcut.None));
            my_menu.MenuItems.Add(new IconMenuItem("Close LiveChat", Resources.Close, Exit, Shortcut.None));
            my_menu.MenuItems.Add(new MenuItem("-"));
            my_menu.MenuItems.Add(new IconMenuItem("About LiveChat", Resources.About, About, Shortcut.None));
            nIcon.ContextMenu = my_menu;
            nIcon.MouseDoubleClick += new MouseEventHandler(nIcon_MouseDoubleClick);
        }
        public static void CloseIcon()
        {
            nIcon.Visible = false;
        }
        public static void nIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ContactList.ContactList_State)
            {
                if (ContactList.ContactList_IsMini)
                {
                    ContactList.ContactList_IsMini = false;
                    Protocol.cList.Show();
                }
                return;
            }
            Program.IsMinimize = false;
            if (!FromState)
            {
                FromState = true;
                lFrom.Show();
            }
        }
        public static void Open(object sender, EventArgs e)
        {
            if (ContactList.ContactList_State)
            {
                if (ContactList.ContactList_IsMini)
                {
                    ContactList.ContactList_IsMini = false;
                    Protocol.cList.Show();
                }
                return;
            }
            Program.IsMinimize = false;
            if (!FromState)
            {
                FromState = true;
                lFrom.Show();
            }
        }
        public static void Exit(object sender, EventArgs e)
        {
            Program.ProgramState = false;
        }
        public static void About(object sender, EventArgs e)
        {
            MessageBox.Show("[CzF]Leo123 & eXeCuTeR, Copyright", "About");
        }
        public static void Minimize(object sender, EventArgs e)
        {
            if (ContactList.ContactList_State)
            {
                if (!ContactList.ContactList_IsMini)
                {
                    Protocol.cList.Hide();
                    ContactList.ContactList_IsMini = true;
                }
                return;
            }
            Tray.FromState = false;
            Program.IsMinimize = true;
        }
    }
}
