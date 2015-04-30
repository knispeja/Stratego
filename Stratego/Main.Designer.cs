namespace Stratego
{
    partial class StrategoWin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrategoWin));
            this.startTimer = new System.Windows.Forms.Timer(this.components);
            this.SidePanelOpenButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.backPanel = new Stratego.BuffPanel();
            this.PauseMenuExitButton = new System.Windows.Forms.Button();
            this.SidePanel = new System.Windows.Forms.Panel();
            this.donePlacingButton = new System.Windows.Forms.Button();
            this.removeCheckBox = new System.Windows.Forms.CheckBox();
            this.place12Button = new System.Windows.Forms.Button();
            this.place11Button = new System.Windows.Forms.Button();
            this.place10Button = new System.Windows.Forms.Button();
            this.place9Button = new System.Windows.Forms.Button();
            this.place8Button = new System.Windows.Forms.Button();
            this.place7Button = new System.Windows.Forms.Button();
            this.place6Button = new System.Windows.Forms.Button();
            this.place5Button = new System.Windows.Forms.Button();
            this.place4Button = new System.Windows.Forms.Button();
            this.place3Button = new System.Windows.Forms.Button();
            this.place2Button = new System.Windows.Forms.Button();
            this.place1Button = new System.Windows.Forms.Button();
            this.TitlePictureBox = new System.Windows.Forms.PictureBox();
            this.FireBox = new System.Windows.Forms.PictureBox();
            this.backPanel.SuspendLayout();
            this.SidePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FireBox)).BeginInit();
            this.SuspendLayout();
            // 
            // startTimer
            // 
            this.startTimer.Enabled = true;
            this.startTimer.Tick += new System.EventHandler(this.startTimer_Tick);
            // 
            // SidePanelOpenButton
            // 
            this.SidePanelOpenButton.Location = new System.Drawing.Point(0, 0);
            this.SidePanelOpenButton.Name = "SidePanelOpenButton";
            this.SidePanelOpenButton.Size = new System.Drawing.Size(96, 23);
            this.SidePanelOpenButton.TabIndex = 1;
            this.SidePanelOpenButton.Text = "Open Side Panel";
            this.SidePanelOpenButton.UseVisualStyleBackColor = true;
            this.SidePanelOpenButton.Visible = false;
            this.SidePanelOpenButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SidePanelOpenButton_MouseClick);
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartButton.BackColor = System.Drawing.Color.Transparent;
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Image = global::Stratego.Properties.Resources.StartButton;
            this.StartButton.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.StartButton.Location = new System.Drawing.Point(782, 419);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(216, 91);
            this.StartButton.TabIndex = 2;
            this.StartButton.TabStop = false;
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Visible = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            this.StartButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartButton_MouseDown);
            this.StartButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StartButton_MouseUp);
            // 
            // backPanel
            // 
            this.backPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backPanel.BackColor = System.Drawing.Color.Transparent;
            this.backPanel.Controls.Add(this.PauseMenuExitButton);
            this.backPanel.Controls.Add(this.SidePanel);
            this.backPanel.Controls.Add(this.StartButton);
            this.backPanel.Controls.Add(this.TitlePictureBox);
            this.backPanel.Controls.Add(this.FireBox);
            this.backPanel.Location = new System.Drawing.Point(0, 0);
            this.backPanel.Margin = new System.Windows.Forms.Padding(0);
            this.backPanel.Name = "backPanel";
            this.backPanel.Size = new System.Drawing.Size(1120, 630);
            this.backPanel.TabIndex = 0;
            this.backPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.backPanel_Paint);
            this.backPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.backPanel_MouseClick);
            this.backPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.backPanel_PreviewKeyDown);
            // 
            // PauseMenuExitButton
            // 
            this.PauseMenuExitButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PauseMenuExitButton.Location = new System.Drawing.Point(503, 284);
            this.PauseMenuExitButton.Name = "PauseMenuExitButton";
            this.PauseMenuExitButton.Size = new System.Drawing.Size(136, 59);
            this.PauseMenuExitButton.TabIndex = 5;
            this.PauseMenuExitButton.Text = "Exit";
            this.PauseMenuExitButton.UseVisualStyleBackColor = true;
            this.PauseMenuExitButton.Visible = false;
            this.PauseMenuExitButton.VisibleChanged += new System.EventHandler(this.PauseMenuExitButton_VisibleChanged);
            this.PauseMenuExitButton.Click += new System.EventHandler(this.PauseMenuExitButton_Click);
            // 
            // SidePanel
            // 
            this.SidePanel.BackColor = System.Drawing.Color.DimGray;
            this.SidePanel.Controls.Add(this.donePlacingButton);
            this.SidePanel.Controls.Add(this.removeCheckBox);
            this.SidePanel.Controls.Add(this.place12Button);
            this.SidePanel.Controls.Add(this.place11Button);
            this.SidePanel.Controls.Add(this.place10Button);
            this.SidePanel.Controls.Add(this.place9Button);
            this.SidePanel.Controls.Add(this.place8Button);
            this.SidePanel.Controls.Add(this.place7Button);
            this.SidePanel.Controls.Add(this.place6Button);
            this.SidePanel.Controls.Add(this.place5Button);
            this.SidePanel.Controls.Add(this.place4Button);
            this.SidePanel.Controls.Add(this.place3Button);
            this.SidePanel.Controls.Add(this.place2Button);
            this.SidePanel.Controls.Add(this.place1Button);
            this.SidePanel.Location = new System.Drawing.Point(0, 29);
            this.SidePanel.Name = "SidePanel";
            this.SidePanel.Size = new System.Drawing.Size(107, 598);
            this.SidePanel.TabIndex = 4;
            this.SidePanel.Visible = false;
            // 
            // donePlacingButton
            // 
            this.donePlacingButton.Enabled = false;
            this.donePlacingButton.Location = new System.Drawing.Point(5, 3);
            this.donePlacingButton.Name = "donePlacingButton";
            this.donePlacingButton.Size = new System.Drawing.Size(96, 46);
            this.donePlacingButton.TabIndex = 13;
            this.donePlacingButton.Text = "Done Placing!";
            this.donePlacingButton.UseVisualStyleBackColor = true;
            this.donePlacingButton.Click += new System.EventHandler(this.donePlacingButton_click);
            // 
            // removeCheckBox
            // 
            this.removeCheckBox.AutoSize = true;
            this.removeCheckBox.Location = new System.Drawing.Point(12, 565);
            this.removeCheckBox.Name = "removeCheckBox";
            this.removeCheckBox.Size = new System.Drawing.Size(66, 17);
            this.removeCheckBox.TabIndex = 12;
            this.removeCheckBox.Text = "Remove";
            this.removeCheckBox.UseVisualStyleBackColor = true;
            this.removeCheckBox.CheckedChanged += new System.EventHandler(this.removeCheckBox_CheckedChanged);
            // 
            // place12Button
            // 
            this.place12Button.Location = new System.Drawing.Point(5, 517);
            this.place12Button.Name = "place12Button";
            this.place12Button.Size = new System.Drawing.Size(96, 36);
            this.place12Button.TabIndex = 11;
            this.place12Button.Tag = "12";
            this.place12Button.Text = "Flag";
            this.place12Button.UseVisualStyleBackColor = true;
            // 
            // place11Button
            // 
            this.place11Button.Location = new System.Drawing.Point(5, 475);
            this.place11Button.Name = "place11Button";
            this.place11Button.Size = new System.Drawing.Size(96, 36);
            this.place11Button.TabIndex = 10;
            this.place11Button.Tag = "11";
            this.place11Button.Text = "Bomb";
            this.place11Button.UseVisualStyleBackColor = true;
            // 
            // place10Button
            // 
            this.place10Button.Location = new System.Drawing.Point(5, 433);
            this.place10Button.Name = "place10Button";
            this.place10Button.Size = new System.Drawing.Size(96, 36);
            this.place10Button.TabIndex = 9;
            this.place10Button.Tag = "10";
            this.place10Button.Text = "Spy";
            this.place10Button.UseVisualStyleBackColor = true;
            // 
            // place9Button
            // 
            this.place9Button.Location = new System.Drawing.Point(5, 391);
            this.place9Button.Name = "place9Button";
            this.place9Button.Size = new System.Drawing.Size(96, 36);
            this.place9Button.TabIndex = 8;
            this.place9Button.Tag = "9";
            this.place9Button.Text = "9";
            this.place9Button.UseVisualStyleBackColor = true;
            // 
            // place8Button
            // 
            this.place8Button.Location = new System.Drawing.Point(5, 349);
            this.place8Button.Name = "place8Button";
            this.place8Button.Size = new System.Drawing.Size(96, 36);
            this.place8Button.TabIndex = 7;
            this.place8Button.Tag = "8";
            this.place8Button.Text = "8";
            this.place8Button.UseVisualStyleBackColor = true;
            // 
            // place7Button
            // 
            this.place7Button.Location = new System.Drawing.Point(5, 307);
            this.place7Button.Name = "place7Button";
            this.place7Button.Size = new System.Drawing.Size(96, 36);
            this.place7Button.TabIndex = 6;
            this.place7Button.Tag = "7";
            this.place7Button.Text = "7";
            this.place7Button.UseVisualStyleBackColor = true;
            // 
            // place6Button
            // 
            this.place6Button.Location = new System.Drawing.Point(5, 265);
            this.place6Button.Name = "place6Button";
            this.place6Button.Size = new System.Drawing.Size(96, 36);
            this.place6Button.TabIndex = 5;
            this.place6Button.Tag = "6";
            this.place6Button.Text = "6";
            this.place6Button.UseVisualStyleBackColor = true;
            // 
            // place5Button
            // 
            this.place5Button.Location = new System.Drawing.Point(5, 223);
            this.place5Button.Name = "place5Button";
            this.place5Button.Size = new System.Drawing.Size(96, 36);
            this.place5Button.TabIndex = 4;
            this.place5Button.Tag = "5";
            this.place5Button.Text = "5";
            this.place5Button.UseVisualStyleBackColor = true;
            // 
            // place4Button
            // 
            this.place4Button.Location = new System.Drawing.Point(5, 181);
            this.place4Button.Name = "place4Button";
            this.place4Button.Size = new System.Drawing.Size(96, 36);
            this.place4Button.TabIndex = 3;
            this.place4Button.Tag = "4";
            this.place4Button.Text = "4";
            this.place4Button.UseVisualStyleBackColor = true;
            // 
            // place3Button
            // 
            this.place3Button.Location = new System.Drawing.Point(5, 139);
            this.place3Button.Name = "place3Button";
            this.place3Button.Size = new System.Drawing.Size(96, 36);
            this.place3Button.TabIndex = 2;
            this.place3Button.Tag = "3";
            this.place3Button.Text = "3";
            this.place3Button.UseVisualStyleBackColor = true;
            // 
            // place2Button
            // 
            this.place2Button.Location = new System.Drawing.Point(5, 97);
            this.place2Button.Name = "place2Button";
            this.place2Button.Size = new System.Drawing.Size(96, 36);
            this.place2Button.TabIndex = 1;
            this.place2Button.Tag = "2";
            this.place2Button.Text = "2";
            this.place2Button.UseVisualStyleBackColor = true;
            // 
            // place1Button
            // 
            this.place1Button.BackColor = System.Drawing.Color.DimGray;
            this.place1Button.Location = new System.Drawing.Point(5, 55);
            this.place1Button.Name = "place1Button";
            this.place1Button.Size = new System.Drawing.Size(96, 36);
            this.place1Button.TabIndex = 0;
            this.place1Button.Tag = "1";
            this.place1Button.Text = "1";
            this.place1Button.UseVisualStyleBackColor = true;
            // 
            // TitlePictureBox
            // 
            this.TitlePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TitlePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.TitlePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.TitlePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("TitlePictureBox.Image")));
            this.TitlePictureBox.Location = new System.Drawing.Point(677, 114);
            this.TitlePictureBox.Name = "TitlePictureBox";
            this.TitlePictureBox.Size = new System.Drawing.Size(321, 95);
            this.TitlePictureBox.TabIndex = 1;
            this.TitlePictureBox.TabStop = false;
            this.TitlePictureBox.Visible = false;
            this.TitlePictureBox.WaitOnLoad = true;
            // 
            // FireBox
            // 
            this.FireBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FireBox.BackColor = System.Drawing.Color.Transparent;
            this.FireBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FireBox.Image = global::Stratego.Properties.Resources.AniFire;
            this.FireBox.Location = new System.Drawing.Point(-4, -3);
            this.FireBox.Margin = new System.Windows.Forms.Padding(0);
            this.FireBox.Name = "FireBox";
            this.FireBox.Size = new System.Drawing.Size(1130, 630);
            this.FireBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FireBox.TabIndex = 3;
            this.FireBox.TabStop = false;
            this.FireBox.Visible = false;
            // 
            // StrategoWin
            // 
            this.AcceptButton = this.StartButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1120, 630);
            this.Controls.Add(this.SidePanelOpenButton);
            this.Controls.Add(this.backPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "StrategoWin";
            this.Text = "Stratego";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StrategoWin_Load);
            this.backPanel.ResumeLayout(false);
            this.SidePanel.ResumeLayout(false);
            this.SidePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FireBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer startTimer;
        private System.Windows.Forms.PictureBox FireBox;
        private System.Windows.Forms.Button StartButton;
        private BuffPanel backPanel;
        public System.Windows.Forms.PictureBox TitlePictureBox;
        private System.Windows.Forms.Panel SidePanel;
        private System.Windows.Forms.Button place1Button;
        private System.Windows.Forms.Button SidePanelOpenButton;
        private System.Windows.Forms.Button place12Button;
        private System.Windows.Forms.Button place11Button;
        private System.Windows.Forms.Button place10Button;
        private System.Windows.Forms.Button place9Button;
        private System.Windows.Forms.Button place8Button;
        private System.Windows.Forms.Button place7Button;
        private System.Windows.Forms.Button place6Button;
        private System.Windows.Forms.Button place5Button;
        private System.Windows.Forms.Button place4Button;
        private System.Windows.Forms.Button place3Button;
        private System.Windows.Forms.Button place2Button;
        private System.Windows.Forms.Button PauseMenuExitButton;
        private System.Windows.Forms.CheckBox removeCheckBox;
        private System.Windows.Forms.Button donePlacingButton;
    }
}

