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
            this.FireBox = new System.Windows.Forms.PictureBox();
            this.TitlePictureBox = new System.Windows.Forms.PictureBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.backPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.FireBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).BeginInit();
            this.backPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // startTimer
            // 
            this.startTimer.Enabled = true;
            this.startTimer.Tick += new System.EventHandler(this.startTimer_Tick);
            // 
            // FireBox
            // 
            this.FireBox.BackColor = System.Drawing.Color.Transparent;
            this.FireBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FireBox.Image = global::Stratego.Properties.Resources.AniFire;
            this.FireBox.Location = new System.Drawing.Point(0, 8);
            this.FireBox.Name = "FireBox";
            this.FireBox.Size = new System.Drawing.Size(492, 445);
            this.FireBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FireBox.TabIndex = 3;
            this.FireBox.TabStop = false;
            this.FireBox.Visible = false;
            // 
            // TitlePictureBox
            // 
            this.TitlePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.TitlePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("TitlePictureBox.Image")));
            this.TitlePictureBox.Location = new System.Drawing.Point(89, 100);
            this.TitlePictureBox.Name = "TitlePictureBox";
            this.TitlePictureBox.Size = new System.Drawing.Size(318, 86);
            this.TitlePictureBox.TabIndex = 1;
            this.TitlePictureBox.TabStop = false;
            this.TitlePictureBox.Visible = false;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.Transparent;
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Image = global::Stratego.Properties.Resources.StartButton;
            this.StartButton.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.StartButton.Location = new System.Drawing.Point(149, 294);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(202, 91);
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
            this.backPanel.AutoSize = true;
            this.backPanel.BackColor = System.Drawing.Color.Transparent;
            this.backPanel.Controls.Add(this.StartButton);
            this.backPanel.Controls.Add(this.TitlePictureBox);
            this.backPanel.Controls.Add(this.FireBox);
            this.backPanel.Location = new System.Drawing.Point(-2, -8);
            this.backPanel.Name = "backPanel";
            this.backPanel.Size = new System.Drawing.Size(496, 458);
            this.backPanel.TabIndex = 0;
            this.backPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.backPanel_Paint);
            // 
            // StrategoWin
            // 
            this.AcceptButton = this.StartButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(489, 445);
            this.Controls.Add(this.backPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "StrategoWin";
            this.Text = "Stratego";
            this.Load += new System.EventHandler(this.StrategoWin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FireBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).EndInit();
            this.backPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer startTimer;
        private System.Windows.Forms.PictureBox FireBox;
        private System.Windows.Forms.PictureBox TitlePictureBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Panel backPanel;
    }
}

