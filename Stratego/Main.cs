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
        int activeSidePanelButton = 0; //Placeholder for which button on the placement side panel is being used
        int ticks = 0;
        int panelWidth;
        int panelHeight;
        int[,] boardState;
        int[] placements;
        bool gameStarted;

        public StrategoWin()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
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

            this.SidePanelOpenButton.Visible = true;
            foreach (var button in this.SidePanel.Controls.OfType<Button>())
                button.Click += SidePanelButtonClick;
        }

        /// <summary>
        /// Event that handles button presses in the side panel, where the user selects the kind of piece to place.
        /// </summary>
        /// <param name="sender">Button that was pressed</param>
        /// <param name="e"></param>
        private void SidePanelButtonClick(object sender, EventArgs e)
        {
           // this.piecePlacing = Convert.ToInt32(((Button)sender).Text); No longer used, as I use the Tag text of the buttons instead.
            if (!this.removeCheckBox.Checked)
            {
                foreach (var button in this.SidePanel.Controls.OfType<Button>())
                    button.UseVisualStyleBackColor = true;
                this.piecePlacing = Convert.ToInt32(((Button)sender).Tag);
                ((Button)sender).UseVisualStyleBackColor = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                                if (piece == 9)
                                {
                                    int scaleX = this.panelWidth / this.boardState.GetLength(0);
                                    int scaleY = this.panelHeight / this.boardState.GetLength(1);
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX / 10), y * scaleY + (scaleY / 10), scaleX - 2*(scaleX / 10), scaleY - 2*(scaleY / 10));
                                    Image imag = Properties.Resources.BlueScout;
                                    e.Graphics.DrawImage(imag, r);
                                }
                                else
                                {
                                    g.FillEllipse(b, cornerX, cornerY, diameter, diameter);
                                    g.DrawEllipse(pen, cornerX, cornerY, diameter, diameter);

                                    string drawString = Piece.toString(piece);
                                    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                                    g.DrawString(drawString, drawFont, drawBrush, cornerX + diameter / 8, cornerY + diameter / 4);
                                    drawFont.Dispose();
                                    drawBrush.Dispose();
                                }
                                b.Dispose();
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
            bool? piecePlaced = false;
            if (gameStarted)
            {
                piecePlaced = placePiece(this.piecePlacing, e.X, e.Y);
                backPanel.Focus();
            }
            if (piecePlaced.Value)
            {
                int scaleX = this.panelWidth / this.boardState.GetLength(0);
                int scaleY = this.panelHeight / this.boardState.GetLength(1);
                //This makes it so iy only repaints the rectangle where the piece is placed
                Rectangle r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY); 
                backPanel.Invalidate(r);
            }
            //backPanel.Invalidate();
        }

        private void backPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (gameStarted)
            {
                KeysConverter kc = new KeysConverter();
                string keyChar = kc.ConvertToString(e.KeyCode);
                if(e.KeyCode==Keys.Escape)
                {
                    this.PauseMenuExitButton.Visible = !this.PauseMenuExitButton.Visible;
                    //Make the escape/pause/whatever panel visible
                }
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

        private void SidePanelOpenButton_MouseClick(object sender, MouseEventArgs e)
        {
            //Makes the side panel open when the button is clicked
            if (this.SidePanel.Visible)
                this.backPanel.Focus();
            this.SidePanel.Visible = !this.SidePanel.Visible;
        }

        private void PauseMenuExitButton_Click(object sender, EventArgs e)
        {
            //Makes the program close if the exit button is pressed
            if (Application.MessageLoop)
                Application.Exit();
        }

        private void PauseMenuExitButton_VisibleChanged(object sender, EventArgs e)
        {
            //Sets the exit button to be in the center of the panel whenever it is made visible/not
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            this.Location = new Point(this.panelWidth / 2 - ((Button)sender).Width / 2, this.panelHeight / 2 - ((Button)sender).Height / 2);
        }

        private void removeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.removeCheckBox.Checked)
            {
                this.activeSidePanelButton = this.piecePlacing;
                this.piecePlacing = 0;
            } 
            else
                this.piecePlacing = this.activeSidePanelButton;
        }
    }
}
