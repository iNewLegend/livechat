namespace LiveChatClient
{
    partial class ContactList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactList));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("1", System.Windows.Forms.HorizontalAlignment.Center);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("2", System.Windows.Forms.HorizontalAlignment.Center);
            this.MyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProfileImageList = new System.Windows.Forms.ImageList(this.components);
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.MyPictureBox = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.PersonalMessage = new System.Windows.Forms.Label();
            this.ContactlistView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.NickLabel = new System.Windows.Forms.Label();
            this.MainButton = new System.Windows.Forms.Button();
            this.StatusButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.MinToTray = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.MyMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MyPictureBox)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // MyMenu
            // 
            this.MyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.startToolStripMenuItem});
            this.MyMenu.Name = "MyMenu";
            this.MyMenu.Size = new System.Drawing.Size(161, 51);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toolStripTextBox1.Enabled = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.ReadOnly = true;
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageToolStripMenuItem});
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // messageToolStripMenuItem
            // 
            this.messageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("messageToolStripMenuItem.Image")));
            this.messageToolStripMenuItem.Name = "messageToolStripMenuItem";
            this.messageToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.messageToolStripMenuItem.Text = "Message";
            this.messageToolStripMenuItem.Click += new System.EventHandler(this.messageToolStripMenuItem_Click);
            // 
            // ProfileImageList
            // 
            this.ProfileImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ProfileImageList.ImageStream")));
            this.ProfileImageList.TransparentColor = System.Drawing.Color.Black;
            this.ProfileImageList.Images.SetKeyName(0, "online.png");
            this.ProfileImageList.Images.SetKeyName(1, "offline.png");
            // 
            // SearchButton
            // 
            this.SearchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.SearchButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.SearchButton.Location = new System.Drawing.Point(116, 59);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(38, 20);
            this.SearchButton.TabIndex = 18;
            this.SearchButton.Text = "&GO";
            this.SearchButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SearchButton.UseVisualStyleBackColor = false;
            // 
            // SearchBox
            // 
            this.SearchBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SearchBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.SearchBox.ContextMenuStrip = this.MyMenu;
            this.SearchBox.Location = new System.Drawing.Point(8, 59);
            this.SearchBox.MaxLength = 50;
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(107, 20);
            this.SearchBox.TabIndex = 9;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            this.SearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyDown);
            // 
            // MyPictureBox
            // 
            this.MyPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MyPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MyPictureBox.Location = new System.Drawing.Point(6, 7);
            this.MyPictureBox.Name = "MyPictureBox";
            this.MyPictureBox.Size = new System.Drawing.Size(45, 45);
            this.MyPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MyPictureBox.TabIndex = 16;
            this.MyPictureBox.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.PersonalMessage);
            this.panel5.Controls.Add(this.ContactlistView);
            this.panel5.Controls.Add(this.SearchButton);
            this.panel5.Controls.Add(this.SearchBox);
            this.panel5.Controls.Add(this.MyPictureBox);
            this.panel5.Controls.Add(this.NickLabel);
            this.panel5.Controls.Add(this.MainButton);
            this.panel5.Controls.Add(this.StatusButton);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Controls.Add(this.button2);
            this.panel5.Location = new System.Drawing.Point(-2, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(162, 429);
            this.panel5.TabIndex = 12;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
            // 
            // PersonalMessage
            // 
            this.PersonalMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.PersonalMessage.Location = new System.Drawing.Point(58, 27);
            this.PersonalMessage.Name = "PersonalMessage";
            this.PersonalMessage.Size = new System.Drawing.Size(91, 12);
            this.PersonalMessage.TabIndex = 20;
            this.PersonalMessage.Text = "Personal Message";
            this.PersonalMessage.DoubleClick += new System.EventHandler(this.AwayString_DoubleClick);
            this.PersonalMessage.Click += new System.EventHandler(this.PersonalMessage_Click);
            // 
            // ContactlistView
            // 
            this.ContactlistView.BackColor = System.Drawing.Color.White;
            this.ContactlistView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContactlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.ContactlistView.ContextMenuStrip = this.MyMenu;
            listViewGroup1.Header = "1";
            listViewGroup1.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "2";
            listViewGroup2.HeaderAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            listViewGroup2.Name = "listViewGroup2";
            this.ContactlistView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.ContactlistView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ContactlistView.LargeImageList = this.ProfileImageList;
            this.ContactlistView.Location = new System.Drawing.Point(4, 85);
            this.ContactlistView.Name = "ContactlistView";
            this.ContactlistView.Size = new System.Drawing.Size(151, 284);
            this.ContactlistView.SmallImageList = this.ProfileImageList;
            this.ContactlistView.TabIndex = 19;
            this.ContactlistView.TileSize = new System.Drawing.Size(1, 1);
            this.ContactlistView.UseCompatibleStateImageBehavior = false;
            this.ContactlistView.View = System.Windows.Forms.View.Details;
            this.ContactlistView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ContactlistView_MouseDoubleClick);
            this.ContactlistView.SelectedIndexChanged += new System.EventHandler(this.ContactlistView_SelectedIndexChanged);
            this.ContactlistView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContactlistView_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 1;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 1;
            // 
            // NickLabel
            // 
            this.NickLabel.AutoEllipsis = true;
            this.NickLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NickLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.NickLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.NickLabel.Location = new System.Drawing.Point(57, 9);
            this.NickLabel.Name = "NickLabel";
            this.NickLabel.Size = new System.Drawing.Size(97, 18);
            this.NickLabel.TabIndex = 15;
            this.NickLabel.Click += new System.EventHandler(this.NickLabel_Click);
            // 
            // MainButton
            // 
            this.MainButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.MainButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MainButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.MainButton.Location = new System.Drawing.Point(80, 400);
            this.MainButton.Name = "MainButton";
            this.MainButton.Size = new System.Drawing.Size(74, 23);
            this.MainButton.TabIndex = 14;
            this.MainButton.Text = "Main";
            this.MainButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MainButton.UseVisualStyleBackColor = false;
            // 
            // StatusButton
            // 
            this.StatusButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.StatusButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.StatusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.StatusButton.Location = new System.Drawing.Point(80, 375);
            this.StatusButton.Name = "StatusButton";
            this.StatusButton.Size = new System.Drawing.Size(74, 23);
            this.StatusButton.TabIndex = 12;
            this.StatusButton.Text = "&Online";
            this.StatusButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.StatusButton.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.button4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.button4.Location = new System.Drawing.Point(3, 400);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(74, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "My Details";
            this.button4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.button2.Location = new System.Drawing.Point(3, 375);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Find Users";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(135)))), ((int)(((byte)(160)))));
            this.panel3.Controls.Add(this.MinToTray);
            this.panel3.Controls.Add(this.Title);
            this.panel3.Controls.Add(this.CloseButton);
            this.panel3.Location = new System.Drawing.Point(-2, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(165, 23);
            this.panel3.TabIndex = 11;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseMove);
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseDown);
            // 
            // MinToTray
            // 
            this.MinToTray.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.MinToTray.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MinToTray.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinToTray.Font = new System.Drawing.Font("Gautami", 6.25F, System.Drawing.FontStyle.Bold);
            this.MinToTray.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.MinToTray.Location = new System.Drawing.Point(116, 3);
            this.MinToTray.Name = "MinToTray";
            this.MinToTray.Size = new System.Drawing.Size(17, 17);
            this.MinToTray.TabIndex = 9;
            this.MinToTray.Text = "-";
            this.MinToTray.UseVisualStyleBackColor = false;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold);
            this.Title.ForeColor = System.Drawing.Color.Black;
            this.Title.Location = new System.Drawing.Point(14, 3);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(62, 15);
            this.Title.TabIndex = 8;
            this.Title.Text = "LiveChat";
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.CloseButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Font = new System.Drawing.Font("Gautami", 5.25F, System.Drawing.FontStyle.Bold);
            this.CloseButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(40)))), ((int)(((byte)(81)))));
            this.CloseButton.Location = new System.Drawing.Point(139, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(17, 17);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.Text = "X";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ContactList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(216)))), ((int)(((byte)(228)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(158, 452);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ContactList";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LiveChat - Contact List";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.Load += new System.EventHandler(this.ContactList_Load);
            this.MyMenu.ResumeLayout(false);
            this.MyMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MyPictureBox)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip MyMenu;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem messageToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ImageList ProfileImageList;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox SearchBox;
        public System.Windows.Forms.PictureBox MyPictureBox;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.Label NickLabel;
        private System.Windows.Forms.Button MainButton;
        private System.Windows.Forms.Button StatusButton;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button CloseButton;
        public System.Windows.Forms.ListView ContactlistView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label PersonalMessage;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button MinToTray;
    }
}