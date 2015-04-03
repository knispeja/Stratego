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
        public readonly int[] defaults = new int[13] { 0, 1, 1, 2, 3, 4, 4, 4, 5, 8, 1, 6, 1 };

        int piecePlacing = 0;
        int ticks = 0;
        int panelWidth;
        int panelHeight;
        int[,] boardState;
        int[] placements;
        bool gameStarted;

        public StrategoWin()
        {
            InitializeComponent();
            SoundPlayer sound = new SoundPlayer(Properties.Resources.BattleDramatic);
            sound.PlayLooping();
            this.StartButton.FlatStyle = FlatStyle.Flat;
            this.StartButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, Color.Red);
            this.StartButton.FlatAppearance.BorderSize = 0;
            this.StartButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, Color.Red);
            Timer t = new Timer();
            this.panelWidth = this.backPanel.Width;
            this.panelHeight = this.backPanel.Height;
            t.Start();
            boardState = new int[10, 10];
            for (int x = 0; x < boardState.GetLength(0); x++ )
            {
                for (int y = 0; y < 6; y++)
                    boardState[x, y] = 42;
            }
        }

        public StrategoWin(int windowWidth, int windowHeight, int[,] boardState)
        {
            this.panelWidth = windowWidth;
            this.panelHeight = windowHeight;
            this.boardState = boardState;
            this.placements = (int[]) this.defaults.Clone();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            //if (ticks < 40)
            //    return;
            SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
            sound.Play();
            this.FireBox.Dispose();
            this.placements = (int[])this.defaults.Clone();
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
                this.panelWidth = this.backPanel.Width;
                this.panelHeight = this.backPanel.Height;

                Pen pen = new Pen(Color.White, 1);
                Graphics g = e.Graphics;

                int num_cols = 10;
                int num_rows = 10;
                int col_inc = panelWidth / num_cols;
                int row_inc = panelHeight / num_rows;

                for (int i = 0; i < num_cols + 1; i++)
                {
                    g.DrawLine(pen, col_inc*i, 0, col_inc*i, panelHeight);
                }
                for (int j = 0; j < num_rows + 1; j++)
                {
                    g.DrawLine(pen, 0, row_inc*j, panelWidth, row_inc*j);
                }

                int diameter = Math.Min(col_inc,row_inc);
                int paddingX = (col_inc - diameter) / 2;
                int paddingY = (row_inc - diameter) / 2;
                for (int x = 0; x < this.boardState.GetLength(0); x++){
                    for(int y = 0; y < this.boardState.GetLength(1); y++){
                        int piece = this.boardState[x, y];
                        if (piece != 0)
                        {
                            Brush b;
                            if (piece != 42)
                            {
                                if (piece > 0)
                                {
                                    b = new SolidBrush(Color.FromArgb(25, 25, 15 * Math.Abs(piece)));
                                    pen.Color = Color.FromArgb(200, 200, 255);
                                }
                                else
                                {
                                    b = new SolidBrush(Color.FromArgb(15 * Math.Abs(piece), 25, 25));
                                    pen.Color = Color.FromArgb(255, 200, 200);
                                }

                                int cornerX = x * col_inc + paddingX;
                                int cornerY = y * row_inc + paddingY;

                                g.FillEllipse(b, cornerX, cornerY, diameter, diameter);
                                g.DrawEllipse(pen, cornerX, cornerY, diameter, diameter);

                                string drawString = Piece.toString(piece);
                                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                                g.DrawString(drawString, drawFont, drawBrush, cornerX + diameter / 8, cornerY + diameter / 4);
                                drawFont.Dispose();
                                drawBrush.Dispose();
                            }
                        }
                    }
                }

                pen.Dispose();
                g.Dispose();
            }
        }

        public int getPiece(int x, int y) 
        {
            return this.boardState[x,y];
        }

        public int getPiecesLeft(int piece)
        {
            return this.placements[piece];
        }

        public bool? placePiece(int piece, int x, int y)
        {
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY= this.panelHeight / this.boardState.GetLength(1);
            if (this.boardState[x / scaleX, y / scaleY] == 0 && this.placements[Math.Abs(piece)] > 0)
            {
                this.boardState[x / scaleX, y / scaleY] = piece;
                this.placements[Math.Abs(piece)] -= 1;
                return true;
            }
            return false;
        }

        private void TitlePictureBox_Click(object sender, EventArgs e)
        {

        }

        private void backPanel_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void backPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameStarted)
            {
                placePiece(this.piecePlacing, e.X, e.Y);
                backPanel.Focus();
            }
            backPanel.Invalidate();
        }

        private void backPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (gameStarted)
            {
                KeysConverter kc = new KeysConverter();
                string keyChar = kc.ConvertToString(e.KeyCode);

                double num;
                if (double.TryParse(keyChar, out num))
                {
                    this.piecePlacing = (int) num;
                }
                else
                {
                    if (keyChar == "S")
                        this.piecePlacing = 10;
                    else if (keyChar == "B")
                        this.piecePlacing = 11;
                    else if (keyChar == "F")
                        this.piecePlacing = 12;
                }
            }
        }
    }
}
