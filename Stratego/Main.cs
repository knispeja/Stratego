using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Media;

namespace Stratego
{
    public partial class StrategoWin : Form
    {
        int ticks = 0;
        int w;
        int h;
        int[,] boardState;
        int[] placements = new int[13];
        bool gameStarted;

        public StrategoWin(bool noGUI)
        {
            if(!noGUI){
                InitializeComponent();
                SoundPlayer sound = new SoundPlayer(Properties.Resources.BattleDramatic);
                sound.PlayLooping();
                this.StartButton.FlatStyle = FlatStyle.Flat;
                this.StartButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, Color.Red);
                this.StartButton.FlatAppearance.BorderSize = 0;
                this.StartButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, Color.Red);
                Timer t = new Timer();
                this.w = this.backPanel.Width;
                this.h = this.backPanel.Height;
                t.Start();
            }

            boardState = new int[10, 10];
        }

        public StrategoWin(int windowWidth, int windowHeight, int[,] boardState)
        {
            this.w = windowWidth;
            this.h = windowHeight;
            this.boardState = boardState;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //if (ticks < 40)
            //    return;
            SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
            sound.Play();
            this.FireBox.Dispose();
            this.gameStarted = true;
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            ticks++;
            if (ticks == 25)
            {
                //this.StartButton.Visible = true;
                this.TitlePictureBox.Visible = true;
            }
            else if (ticks == 40)
            {
                this.TitlePictureBox.Parent = this.FireBox;
                this.StartButton.Parent = this.FireBox;
                //this.FormBorderStyle = FormBorderStyle.FixedSingle;
                //this.FormBorderStyle = FormBorderStyle.Sizable;
                this.StartButton.Visible = true;
                this.FireBox.Visible = true;
                this.startTimer.Dispose();
            }
        }

        private void StartButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.StartButton.Image = Properties.Resources.StartButtonClick;
        }

        private void StartButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.StartButton.Image = Properties.Resources.StartButton;
        }

        private void StrategoWin_Load(object sender, EventArgs e)
        {
        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {
            if (this.gameStarted)
            {
                Pen pen = new Pen(Color.White, 1);
                Graphics g = e.Graphics;

                int num_cols = 10;
                int num_rows = 10;
                int height = backPanel.Height;
                int width = backPanel.Width;
                int col_inc = width / num_cols;
                int row_inc = height / num_rows;

                for (int i = 0; i < num_cols + 1; i++)
                {
                    g.DrawLine(pen, col_inc*i, 0, col_inc*i, height);
                }
                for (int j = 0; j < num_rows + 1; j++)
                {
                    g.DrawLine(pen, 0, row_inc*j, width, row_inc*j);
                }

                int radius = Math.Min(col_inc,row_inc);
                int paddingx = (col_inc - radius) / 2;
                int paddingy = (row_inc - radius) / 2;

                for (int x = 0; x < this.boardState.GetLength(0); x++){
                    for(int y = 0; y < this.boardState.GetLength(1); y++){
                        if(this.boardState[x,y] != 0)
                            g.DrawEllipse(pen, x*col_inc + paddingx, y*row_inc + paddingy, radius, radius);
                    }
                }

                g.Dispose();
            }
        }

        public int getPiece(int x, int y) 
        {
            return this.boardState[x,y];
        }

        public bool? placePiece(int piece, int x, int y)
        {
            int scale = this.h / this.boardState.GetLength(0);
            if (this.boardState[x / scale, y / scale] == 0)
            {
                this.boardState[x / scale, y / scale] = piece;
                return true;
            }
            return false;
        }

        private void TitlePictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
