namespace LiveChatClient
{
    partial class ChangeAwayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeAwayForm));
            this.ChangeAwayLabel = new System.Windows.Forms.Label();
            this.awayStringTextbox = new System.Windows.Forms.TextBox();
            this.changeAway = new System.Windows.Forms.Button();
            this.closeForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChangeAwayLabel
            // 
            resources.ApplyResources(this.ChangeAwayLabel, "ChangeAwayLabel");
            this.ChangeAwayLabel.Name = "ChangeAwayLabel";
            this.ChangeAwayLabel.Click += new System.EventHandler(this.ChangeAwayLabel_Click);
            // 
            // awayStringTextbox
            // 
            resources.ApplyResources(this.awayStringTextbox, "awayStringTextbox");
            this.awayStringTextbox.Name = "awayStringTextbox";
            this.awayStringTextbox.TextChanged += new System.EventHandler(this.awayStringTextbox_TextChanged);
            // 
            // changeAway
            // 
            this.changeAway.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.changeAway.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.changeAway, "changeAway");
            this.changeAway.Name = "changeAway";
            this.changeAway.UseVisualStyleBackColor = false;
            this.changeAway.Click += new System.EventHandler(this.changeAway_Click);
            // 
            // closeForm
            // 
            this.closeForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(203)))), ((int)(((byte)(242)))));
            this.closeForm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeForm.FlatAppearance.BorderColor = System.Drawing.Color.White;
            resources.ApplyResources(this.closeForm, "closeForm");
            this.closeForm.Name = "closeForm";
            this.closeForm.UseVisualStyleBackColor = false;
            // 
            // ChangeAwayForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.closeForm);
            this.Controls.Add(this.changeAway);
            this.Controls.Add(this.awayStringTextbox);
            this.Controls.Add(this.ChangeAwayLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeAwayForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ChangeAwayLabel;
        public System.Windows.Forms.TextBox awayStringTextbox;
        public System.Windows.Forms.Button changeAway;
        public System.Windows.Forms.Button closeForm;
    }
}