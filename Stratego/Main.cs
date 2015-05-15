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
using System.IO;

namespace Stratego
{
    public partial class StrategoWin : Form
    {
        public readonly int[] defaults = new int[13] { 0, 1, 1, 2, 3, 4, 4, 4, 5, 8, 1, 6, 1 };
        //public readonly int[] defaults = new int[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        //public readonly int[] defaults = new int[13] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };

        bool testing = false;
        int piecePlacing = 0;                     // The piece currently being placed by the user
        int activeSidePanelButton = 0;            // Placeholder for which button on the placement side panel is being used
        int ticks = 0;                            // Used by the timer to keep track of the title screen's music & sounds
        public int panelWidth { get; set; }       // Width of the enclosing panel   
        public int panelHeight { get; set; }      // Height of the enclosing panel
        public int[,] boardState { get; set; }    // The 2DArray full of all pieces on the board
        public int[] placements;                         // The array which holds information on how many pieces of each type can still be placed
        public bool preGameActive { get; set; }   // Whether or not the pre game has begun
        public int turn { get; set; }             // -1 for player2 and 1 for player 1. 0 when game isn't started. 
                                                  // 2 for transition from player1 to player2; -2 for transition from player2 to player1
        public Point pieceSelectedCoords { get; set; }       // Coordinates of the piece that is currently selected in the array
        public Boolean pieceIsSelected { get; set; }        //Just a boolean indicating if a piece is currently selected or not
        public Boolean isSinglePlayer { get; set; }         //Whether player 2 is an AI or not
        public Point lastFought { get; set; }             //Coordinates of the last piece to win a battle

        public AI ai;

        /// <summary>
        /// Initializer for normal play (initializes GUI).
        /// Not to be used for testing!
        /// </summary>
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
            this.turn = 0;
            this.preGameActive = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.LoadButton.Click +=LoadButton_Click;  // Why do we have these two lines instead of just setting the
            this.SaveButton.Click +=SaveButton_Click;  // property using the GUI??
            t.Start();

            // Initialize the board state with invalid spaces in the enemy player's side
            // of the board and empty spaces everywhere else. To be changed later!
            boardState = new int[10, 10];
            for (int row = 0; row < 6; row++) fillRow(42, row);

            this.ai = new AI(this, -1);
        }

        /// <summary>
        /// Initializer for the testing framework.
        /// Excludes all GUI elements from initialization.
        /// </summary>
        /// <param name="windowWidth">Used for a simulated GUI window width</param>
        /// <param name="windowHeight">Used for a simulated GUI window height</param>
        /// <param name="boardState">Modified initial board state for ease of testing</param>
        public StrategoWin(int windowWidth, int windowHeight, int[,] boardState)
        {
            this.testing = true;
            this.panelWidth = windowWidth;
            this.panelHeight = windowHeight;
            this.boardState = boardState;
            this.placements = (int[]) this.defaults.Clone();
            this.preGameActive = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);

            this.ai = new AI(this, -1);
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (path.EndsWith("\\bin\\Debug") || path.EndsWith("\\bin\\Release"))
            {
                for (int i = 0; i < path.Length - 3; i++)
                {
                    if ((path[i] == '\\') && (path[i + 1] == 'b') && (path[i + 2] == 'i') && (path[i + 3] == 'n'))
                    {
                        path = path.Substring(0, i);
                        break;
                    }
                }
            }
            dialog.InitialDirectory = System.IO.Path.Combine(path, @"Resources\SaveGames");
            dialog.RestoreDirectory = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(dialog.FileName);
                saveGame(writer);
                writer.Close();
            }
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (path.EndsWith("\\bin\\Debug") || path.EndsWith("\\bin\\Release"))
            {
                for (int i = 0; i < path.Length - 3; i++)
                {
                    if ((path[i] == '\\') && (path[i + 1] == 'b') && (path[i + 2] == 'i') && (path[i + 3] == 'n'))
                    {
                        path = path.Substring(0, i);
                        break;
                    }
                }
            }
            dialog.InitialDirectory = System.IO.Path.Combine(path, @"Resources\SaveGames");
            dialog.RestoreDirectory = true;
            if(dialog.ShowDialog()== DialogResult.OK)
            {
                Stream file = null;
                try
                {
                    if((file = dialog.OpenFile())!= null){
                        loadGame(new StreamReader(file));
                    }
                    SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
                    sound.Play();
                    this.FireBox.Dispose();

                    this.LoadButton.Visible = false;
                    this.SinglePlayerButton.Visible = false;
                    this.SidePanelOpenButton.Visible = true;
                    this.NextTurnButton.Visible = true;
                    this.backPanel.Focus();
                    this.lastFought = new Point(-1, -1);

                    foreach (var button in this.SidePanel.Controls.OfType<Button>())
                        if (button.Name != donePlacingButton.Name) button.Click += SidePanelButtonClick;
  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Called by the start button on the main menu.
        /// </summary>
        /// <param name="sender">Button that was pressed</param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            //if (ticks < 40)
            //    return;
            SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
            sound.Play();
            this.FireBox.Dispose();
            this.placements = (int[])this.defaults.Clone();
            // Start the game!
            nextTurn();
            this.LoadButton.Visible = false;
            this.SinglePlayerButton.Visible = false;
            this.SidePanelOpenButton.Visible = true;
            foreach (var button in this.SidePanel.Controls.OfType<Button>())
                if(button.Name != donePlacingButton.Name) button.Click += SidePanelButtonClick;
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
                this.piecePlacing = this.turn * Convert.ToInt32(((Button)sender).Tag);
                ((Button)sender).UseVisualStyleBackColor = false;
            }
        }

        /// <summary>
        /// Called by each individual tick of the timer
        /// </summary>
        /// <param name="sender">The timer that ticked</param>
        /// <param name="e"></param>
        private void startTimer_Tick(object sender, EventArgs e)
        {
            this.ticks++;
            if (this.ticks == 25)
            {
                //this.StartButton.Visible = true;
                this.TitlePictureBox.Visible = true;
            }
            else if (this.ticks == 40)
            {
                this.TitlePictureBox.Parent = this.FireBox;
                this.StartButton.Parent = this.FireBox;
                //this.FormBorderStyle = FormBorderStyle.FixedSingle;
                //this.FormBorderStyle = FormBorderStyle.Sizable;
                this.StartButton.Visible = true;
                this.FireBox.Visible = true;
                this.LoadButton.Visible = true;
                this.SinglePlayerButton.Visible = true;
                this.startTimer.Dispose();
            }
        }

        /// <summary>
        /// Called by the start button on the main menu on mousedown
        /// Used for changing the image of the start button when pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.StartButton.Image = Properties.Resources.StartButtonClick;
        }

        /// <summary>
        /// Called by the start button on the main menu on mouseup
        /// Changes the image of the start button back to normal on release
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.StartButton.Image = Properties.Resources.StartButton;
        }

        private void StrategoWin_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Primary paint function.
        /// Draws all necessary things onto the back panel!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backPanel_Paint(object sender, PaintEventArgs e)
        {
            if (this.turn != 0)
            {
                this.panelWidth = this.backPanel.Width;
                this.panelHeight = this.backPanel.Height;

                Pen pen = new Pen(Color.White, 1);
                Graphics g = e.Graphics;

                int num_cols = this.boardState.GetLength(0);
                int num_rows = this.boardState.GetLength(1);
                int col_inc = panelWidth / num_cols;
                int row_inc = panelHeight / num_rows;

                // Draw vertical gridlines
                for (int i = 0; i < num_cols + 1; i++)
                {
                    g.DrawLine(pen, col_inc*i, 0, col_inc*i, panelHeight);
                }

                // Draw horizontal gridlines
                for (int j = 0; j < num_rows + 1; j++)
                {
                    g.DrawLine(pen, 0, row_inc*j, panelWidth, row_inc*j);
                }

                int[,] pieceMoves = new int[num_rows, num_cols];
                if (pieceIsSelected)
                    pieceMoves = this.GetPieceMoves(this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y);

                // Large loop which draws the necessary circles/images that represent pieces
                int diameter = Math.Min(col_inc,row_inc);
                int paddingX = (col_inc - diameter) / 2;
                int paddingY = (row_inc - diameter) / 2;
                for (int x = 0; x < this.boardState.GetLength(0); x++)
                {
                    for(int y = 0; y < this.boardState.GetLength(1); y++)
                    {
                        int scaleX = this.panelWidth / this.boardState.GetLength(0);
                        int scaleY = this.panelHeight / this.boardState.GetLength(1);
                        int piece = this.boardState[x, y];
                        if (piece != 0)
                        {
                            Brush b;
                            if (piece != 42)
                            {
                                if (piece > 0)
                                {
                                    // Piece is on the blue team, so we change the brush color to blue
                                    //b = new SolidBrush(Color.FromArgb(25, 25, 15 * Math.Abs(piece)));
                                    b = new SolidBrush(Color.FromArgb(25, 25, 175));
                                    pen.Color = Color.FromArgb(200, 200, 255);
                                }
                                else
                                {
                                    // Piece is on the red team, so we change the brush to reflect that
                                    //b = new SolidBrush(Color.FromArgb(15 * Math.Abs(piece), 25, 25));
                                    b = new SolidBrush(Color.FromArgb(175, 25, 25));
                                    pen.Color = Color.FromArgb(255, 200, 200);
                                }

                                int cornerX = x * col_inc + paddingX;
                                int cornerY = y * row_inc + paddingY;

                                if (piece == 9)
                                {
                                    // Piece is a blue scout (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55))/2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn == 1 ||this.lastFought.Equals(new Point (x, y)))
                                    {
                                        Image imag = Properties.Resources.BlueScoutN;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.DrawRectangle(pen, r);
                                        g.FillRectangle(b, r);
                                    }
                                }
                                else if (piece == -9)
                                {
                                    // Piece is a red scout (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==-1 || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.RedScout;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.FillRectangle(b, r);
                                        g.DrawRectangle(pen, r);
                                    }
                                }
                                else if (piece == 11)
                                {
                                    // Piece is a blue bomb (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==1 || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.BlueBomb;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.DrawRectangle(pen, r);
                                        g.FillRectangle(b, r);
                                    }
                                }
                                else if (piece == -11)
                                {
                                    // Piece is a red bomb (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==-1 || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.RedBomb;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.FillRectangle(b, r);
                                        g.DrawRectangle(pen, r);
                                    }
                                }
                                else if (piece == 10)
                                {
                                    // Piece is a blue spy (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==1 || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.BlueSpy;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.DrawRectangle(pen, r);
                                        g.FillRectangle(b, r);
                                    }
                                }
                                else if (piece == -10)
                                {
                                    // Piece is a red spy (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==-1 || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.RedSpy;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.FillRectangle(b, r);
                                        g.DrawRectangle(pen, r);
                                    }
                                }
                                else if (piece == 5)
                                {
                                    // Piece is a blue captain (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==1|| this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.BlueCaptain;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.FillRectangle(b, r);
                                        g.DrawRectangle(pen, r);
                                    }
                                }
                                else if (piece == -5)
                                {
                                    // Piece is a red captain (displaying as image)
                                    Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                                    if (turn ==-1 || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        Image imag = Properties.Resources.RedCaptain;
                                        e.Graphics.DrawImage(imag, r);
                                        if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                            pen.Color = Color.FromArgb(10, 255, 10);
                                        g.DrawRectangle(pen, r);
                                    }
                                    else
                                    {
                                        g.FillRectangle(b, r);
                                        g.DrawRectangle(pen, r);
                                    }
                                }
                                else
                                {
                                    // Piece is something else, display as circle (to be changed later)
                                    g.FillEllipse(b, cornerX, cornerY, diameter, diameter);
                                    if (this.pieceIsSelected && this.pieceSelectedCoords.X == x && this.pieceSelectedCoords.Y == y)
                                        pen.Color = Color.FromArgb(10, 255, 10);
                                    g.DrawEllipse(pen, cornerX, cornerY, diameter, diameter);

                                    // Draw the text (the name of the piece) onto the circle
                                    if ((turn == -1 && piece < 0) || (turn == 1 && piece > 0) || this.lastFought.Equals(new Point(x, y)))
                                    {
                                        string drawString = Piece.toString(piece);
                                        System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
                                        System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                                        g.DrawString(drawString, drawFont, drawBrush, cornerX + diameter / 8, cornerY + diameter / 4);
                                        drawFont.Dispose();
                                        drawBrush.Dispose();
                                    }
                                }
                                // Dispose of the brush
                                b.Dispose();
                            }
                        }
                        if (pieceMoves[x, y] == 1)
                        {
                            Rectangle r = new Rectangle(x * scaleX + 1, y * scaleY + 1, scaleX - 2, scaleY - 2);
                            //b.Color = Color.FromArgb(90, 90, 255);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 130, 130, 130)), r);
                        }
                    }
                }

                // Dispose of our resources
                pen.Dispose();
                //g.Dispose();
            }
        }

        /// <summary>
        /// Gets the piece at a given board cell
        /// </summary>
        /// <param name="x">x-coordinate of the cell we want</param>
        /// <param name="y">y-coordinate of the cell we want</param>
        /// <returns>The number of the piece located at (x,y)</returns>
        public int getPiece(int x, int y) 
        {
            return this.boardState[x,y];
        }

        public void fillRow(int value, int row)
        {
            for (int x = 0; x < this.boardState.GetLength(0); x++) this.boardState[x, row] = value;
        }

        /// <summary>
        /// Retrieves the number of pieces still available for
        /// placement of a given type
        /// </summary>
        /// <param name="piece">Type of the piece you want to check</param>
        /// <returns>Number of pieces available for placement</returns>
        public int getPiecesLeft(int piece)
        {
            return this.placements[piece];
        }

        /// <summary>
        /// Places a piece at the given coordinates
        /// </summary>
        /// <param name="piece">Number of the piece you want to place</param>
        /// <param name="x">x-coordinate you want to place it at</param>
        /// <param name="y">y-coordinate you want to place it at</param>
        /// <returns>Whether or not the placement was successful</returns>
        public bool? placePiece(int piece, int x, int y)
        {
            if (turn == 0 || Math.Abs(turn) == 2) return false;
            if (Math.Abs(piece) > 12 || x<0 || y<0 || x>this.panelWidth || y>this.panelHeight) throw new ArgumentException();
            if ((Math.Sign(piece) != Math.Sign(this.turn)) &&(piece!=0)) return false;
            Boolean retVal = true;
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY= this.panelHeight / this.boardState.GetLength(1);
            int pieceAtPos = this.boardState[x / scaleX, y / scaleY];

            if (piece == 0 && pieceAtPos != 42)
            {
                // We are trying to remove
                if (Math.Sign(pieceAtPos) != Math.Sign(this.turn)) return false;
                if (pieceAtPos == 0) retVal = false;
                this.placements[Math.Abs(pieceAtPos)]++;
            }
            else if (pieceAtPos == 0 && this.placements[Math.Abs(piece)] > 0)
            {
                // We are trying to add
                this.placements[Math.Abs(piece)] -= 1;
            }
            else retVal = false;

            if (retVal) this.boardState[x / scaleX, y / scaleY] = piece;
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        public void nextTurn() 
        {
            if(!testing)
                this.backPanel.Invalidate();

            //this.backPanel.Update();
            if(this.turn == 0)
            {
                preGameActive = true;
                this.turn = 1;
            }
            else if (this.turn == 1)
            {
                if (this.preGameActive)
                {
                    this.turn = -1;
                    this.placements = this.defaults;
                }
                else
                {
                    if (!testing)
                    {
                        if (!isSinglePlayer)
                            NextTurnButton.Text = "Player 2's Turn";
                        else
                            NextTurnButton.Text = "AI's Turn";
                        NextTurnButton.Visible = true;
                        this.backPanel.Focus();
                    }
                    this.turn = 2;
                }
            }
            else if(this.turn == -1)
            {
                if (this.preGameActive)
                {
                    for (int i = 4; i < 6; i++)
                    {
                        for (int x = 0; x < 2; x++)
                            this.boardState[x, i] = 0;
                        for (int x = 4; x < 6; x++)
                            this.boardState[x, i] = 0;
                        for (int x = 8; x < 10; x++)
                            this.boardState[x, i] = 0;
                    }
                    this.preGameActive = false;
                }
                if (!testing && !this.isSinglePlayer)
                {
                    NextTurnButton.Text = "Player 1's Turn";
                    NextTurnButton.Visible = true;
                    this.backPanel.Focus();
                }
                if (!this.isSinglePlayer) this.turn = -2;
                else this.turn = 1;
            }
            else if(this.turn == -2)
            {
                turn = 1;
            }
            else 
            {
                turn = -1;
            }

            if (this.isSinglePlayer && this.turn == this.ai.team && !this.testing)
            {
                if (this.preGameActive)
                    this.ai.placePieces();
                else
                    this.ai.takeTurn();
            }
        }

        /// <summary>
        /// Selects a piece if no piece is selected.
        /// </summary>
        /// <param name="x">x coords of the click in pixels</param>
        /// <param name="y">y coord of the click in pixels</param>
        /// <returns></returns>
        public bool? SelectPiece(int x, int y)
        {
            if ((Math.Abs(turn) == 2)||(turn ==-1 &&isSinglePlayer)) return false;
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            if ((this.pieceSelectedCoords == new Point(x / scaleX, y / scaleY))&&this.pieceIsSelected)
            {
                this.pieceIsSelected = false;
                return false;
            }
            if ((Math.Abs(this.boardState[x / scaleX, y / scaleY]) == 11 || Math.Abs(this.boardState[x / scaleX, y / scaleY]) == 12) ||
                Math.Sign(this.boardState[x / scaleX, y / scaleY]) != Math.Sign(this.turn))
            {
                return false;
            }
            this.pieceSelectedCoords = new Point(x/scaleX, y/scaleY);
            this.pieceIsSelected = true;
            return true;
        }

        /// <summary>
        /// Moves the selected piece(if there is one) to the tile tile which corresponds to the x,y coords (if valid)
        /// </summary>
        /// <param name="x">x coordinate of the mouse click of where to move (pixels)</param>
        /// <param name="y">y coordinate of the mouse click of where to move (pixels)</param>
        /// <returns>true if a piece was moved, false otherwise</returns>
        public bool MovePiece(int x, int y)
        {
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            if (!this.pieceIsSelected)
                return false;
            this.pieceIsSelected = false;
            if (Piece.attack(this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y],
                this.boardState[x / scaleX, y / scaleY]) == null)
                return false;
            if (Math.Abs(this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y]) != 9)
            {
                if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) > 1 || Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) > 1)
                    return false;
            }
            else
            { //Check for the special 9 cases.
                if(Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) > 1)
                {
                    if (((x / scaleX) - this.pieceSelectedCoords.X) > 1)
                    {
                        for(int i = 1; i < (x / scaleX) - this.pieceSelectedCoords.X; i++)
                        {
                            if(this.boardState[this.pieceSelectedCoords.X+i, this.pieceSelectedCoords.Y] != 0)
                                return false;
                        }
                    }
                    else if (((x / scaleX) - this.pieceSelectedCoords.X) < -1)
                    {
                        for(int i = -1; i > (x / scaleX) - this.pieceSelectedCoords.X; i--)
                        {
                            if(this.boardState[this.pieceSelectedCoords.X+i, this.pieceSelectedCoords.Y] != 0)
                                return false;
                        }
                    }
                }
                else if (Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) > 1)
                {
                    if (((y / scaleY) - this.pieceSelectedCoords.Y) > 1)
                    {
                        for (int i = 1; i < (y / scaleY) - this.pieceSelectedCoords.Y; i++)
                        {
                            if (this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y + i] != 0)
                                return false;
                        }
                    }
                    else if (((y / scaleY) - this.pieceSelectedCoords.Y) < -1)
                    {
                        for (int i = -1; i > (y / scaleY) - this.pieceSelectedCoords.Y; i--)
                        {
                            if (this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y + i] != 0)
                                return false;
                        }
                    }
                }
            }
            if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) >= 1 && Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) >= 1)
                return false;
            if (Math.Abs((x / scaleX) - this.pieceSelectedCoords.X) == 0 && Math.Abs((y / scaleY) - this.pieceSelectedCoords.Y) == 0)
                return false;
            int defender = this.boardState[x / scaleX, y / scaleY];
            this.boardState[x / scaleX, y / scaleY] = Piece.attack(this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y], this.boardState[x / scaleX, y / scaleY]).Value;
            if ((defender == 0) || this.boardState[x / scaleX, y / scaleY]==0)
                this.lastFought = new Point(-1, -1);
            else
                this.lastFought = new Point(x / scaleX, y / scaleY);
            this.boardState[this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y] = 0;
            if (defender == 12)
            {
                if (this.isSinglePlayer)
                    this.EndGameTextBox.Text = "YOU LOST! HA! DUMMY!";
                else
                    this.EndGameTextBox.Text = "RED PLAYER WINS.";

                this.backPanel.Enabled = false;
                this.EndGamePanel.Visible = true;
                this.EndGamePanel.Enabled = true;
                this.EndGamePanel.Focus();
            }
            else if (defender == -12)
            {
                if (this.isSinglePlayer)
                    this.EndGameTextBox.Text = "YOU ARE VICTORIOUS. READY PLAYER 1.";
                else
                    this.EndGameTextBox.Text = "BLUE PLAYER WINS.";

                //this.backPanel.Enabled = false;
                this.EndGamePanel.Visible = true;
                this.EndGamePanel.Enabled = true;
                this.EndGamePanel.Focus();
            }
            else { this.nextTurn(); }
            return true;
        }
        /// <summary>
        /// Loads a gamestate from the given reader 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns> True if successful </returns> 
        public bool loadGame(TextReader reader) 
        {
            string[] lines = new string[100]; //Make a Standard max size later
            string line = reader.ReadLine();
            lines[0] = line;
            int i = 0;
            while (line != null) 
            {
                lines[i] = line;
                line = reader.ReadLine();
                i++;
            }

            string[] numbers = lines[0].Split(' ');
            this.turn = -2*Convert.ToInt32(numbers[0]);
            if (numbers[1] == "0")
                this.isSinglePlayer = false;
            else
            {
                this.isSinglePlayer = true;
                this.ai = new AI(this, -1, Convert.ToInt32(numbers[2]));
            }

            numbers = lines[1].Split(' ');
            int[,] newBoard = new int[numbers.Length, i - 1];

            for (int k = 1; k< i; k++ )
            {
                numbers = lines[k].Split(' ');
                for(int j =0; j< numbers.Length; j++)
                {
                    newBoard[j, k-1] = Convert.ToInt32(numbers[j]);
                }
            }
            this.boardState = newBoard;
                return true;
        }
        /// <summary>
        /// Loads a premade piece setup onto the current team's side of the board
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns> True if Successful
        public bool loadSetUp(TextReader reader)
        {
            if (!this.preGameActive || (this.boardState.GetLength(0) != 10) || (this.boardState.GetLength(1) != 10) || (Math.Abs(turn)==2)) return false;
            string[] lines = new string[4]; 
            string line = reader.ReadLine();
            lines[0] = line;
            int i = 0;
            while (line != null)
            {
                lines[i] = line;
                line = reader.ReadLine();
                i++;
            }
            string[] numbers;
            this.placements = (int[])this.defaults.Clone(); 
            if (turn > 0)
            {
                for (int j = 6; j < 10; j++)
                {
                    numbers = lines[j - 6].Split(' ');
                    for (int k = 0; k < 10; k++)
                    {
                        boardState[k, j] = Convert.ToInt32(numbers[k]);
                        if(Convert.ToInt32(numbers[k])!=0) this.placements[Math.Abs(boardState[k, j])] -= 1;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    numbers = lines[j].Split(' ');
                    for (int k = 0; k < 10; k++)
                    {
                        boardState[9-k, 3-j] = turn*Convert.ToInt32(numbers[k]);
                        this.placements[Math.Abs(boardState[9-k, 3-j])] -= 1;
                    }
                }
            }
            if(!this.testing)
            {
                backPanel.Invalidate();

                for (i = 0; i < this.placements.Length; i++)
                {
                    if (this.placements[i] != 0)
                    {
                        this.donePlacingButton.Enabled = false;
                        return true;
                    }
                }
                this.donePlacingButton.Enabled = true;
            }
            return true;

        }
        /// <summary>
        /// Saves a gamestate into the string or file in the given writer
        /// </summary>
        /// <param name="writer"></param>
        /// <returns> True if successful </returns> 
        public bool saveGame(TextWriter writer)
        {
            if ((this.turn == 0)||(this.preGameActive)||(Math.Abs(this.turn)==2)) return false;

            string buffer = "";
            if (isSinglePlayer)
                buffer = " 1";
            else
                buffer = " 0";
            writer.WriteLine(this.turn + buffer);
            for (int i = 0; i < boardState.GetLength(1); i++ )
            {
                buffer = "";
                for (int j = 0; j < boardState.GetLength(0)-1; j++)
                {
                    buffer += boardState[j, i] + " ";
                }
                buffer += boardState[boardState.GetLength(0) - 1, i];
                writer.WriteLine(buffer);
            }
  
            return true;
        }
        /// <summary>
        /// Saves the set up of the current teams pieces into a file
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool saveSetUp(TextWriter writer)
        {
            string buffer = "";
            if (!preGameActive || (this.boardState.GetLength(0) != 10) || (this.boardState.GetLength(1) != 10)||(Math.Abs(turn)==2)) return false;
            if (turn > 0)
            {
                for (int i = 6; i < 10; i++)
                {

                    for (int j = 0; j < 9; j++)
                    {
                        buffer += boardState[j, i] + " ";

                    }
                    buffer += boardState[9, i];
                    writer.WriteLine(buffer);
                    buffer = "";
                }
            }
            else
            {
                for(int i =0; i< 4; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        buffer += Math.Abs(boardState[9 - j, 3 - i])+ " ";
                    }
                    buffer += Math.Abs(boardState[0, 3 - i]);
                    writer.WriteLine(buffer);
                    buffer = "";
                }
            }
                return true;
        }
        /// <summary>
        /// Receives clicks on the back panel and directs them to the game as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (preGameActive)
            {
                bool? piecePlaced = placePiece(this.piecePlacing, e.X, e.Y);
                backPanel.Focus();

                // Only run if the placement succeeded
                if (piecePlaced.Value)
                {
                    int scaleX = this.panelWidth / this.boardState.GetLength(0);
                    int scaleY = this.panelHeight / this.boardState.GetLength(1);
                    //This makes it so it only repaints the rectangle where the piece is placed
                    Rectangle r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                    backPanel.Invalidate(r);

                    for (int i = 0; i < this.placements.Length; i++)
                    {
                        if (this.placements[i] != 0)
                        {
                            this.donePlacingButton.Enabled = false;
                            return;
                        }
                    }
                    this.donePlacingButton.Enabled = true;
                }
            }
            else if(this.pieceIsSelected)
            {
                int scaleX = this.panelWidth / this.boardState.GetLength(0);
                int scaleY = this.panelHeight / this.boardState.GetLength(1);
                int[,] pieceMoves = this.GetPieceMoves(this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y);
                for (int x = 0; x < this.boardState.GetLength(0); x++)
                    for (int y = 0; y < this.boardState.GetLength(1); y++)
                        if (pieceMoves[x, y] == 1)
                            backPanel.Invalidate(new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY));
                this.MovePiece(e.X, e.Y);
                if (this.EndGamePanel.Enabled == true)
                    return;
                //This makes it so it only repaints the rectangle where the piece is placed
                Rectangle r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                backPanel.Invalidate(r);
                r = new Rectangle(this.pieceSelectedCoords.X * scaleX, this.pieceSelectedCoords.Y * scaleY, scaleX, scaleY);
                backPanel.Invalidate(r);
            }
            else
            {
                this.SelectPiece(e.X, e.Y);
                int scaleX = this.panelWidth / this.boardState.GetLength(0);
                int scaleY = this.panelHeight / this.boardState.GetLength(1);
               // Rectangle r;
                //This makes it so it only repaints the rectangle where the piece is placed
                int[,] pieceMoves = this.GetPieceMoves(this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y);
                for (int x = 0; x < this.boardState.GetLength(0); x++)
                    for(int y = 0; y < this.boardState.GetLength(1); y++)
                        if (pieceMoves[x, y] == 1)
                            backPanel.Invalidate(new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY));
                if (pieceIsSelected)
                    pieceMoves = this.GetPieceMoves(this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y);
                //r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                backPanel.Invalidate(new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY));
            }
            //backPanel.Invalidate();
        }

        /// <summary>
        /// Receives keypresses on the back window and directs them to the game as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(this.turn != 0 && !this.preGameActive)
            {
                KeysConverter kc = new KeysConverter();
                string keyChar = kc.ConvertToString(e.KeyCode);
                if (e.KeyCode == Keys.Escape)
                {
                    this.PauseMenuExitButton.Visible = !this.PauseMenuExitButton.Visible;
                    //Make the escape/pause/whatever panel visible
                }
                else if(e.KeyCode == Keys.ShiftKey)
                {
                    if (this.SidePanel.Visible && !this.testing)
                    {
                        this.backPanel.Focus();
                        this.SidePanelOpenButton.Text = "Open Side";
                    }
                    else if(!this.testing)
                    {
                        this.SidePanelOpenButton.Text = "Close Side";
                    }
                    this.SidePanel.Visible = !this.SidePanel.Visible;
                }
            }
            else if (this.preGameActive)
            {
                KeysConverter kc = new KeysConverter();
                string keyChar = kc.ConvertToString(e.KeyCode);
                if(e.KeyCode==Keys.Escape)
                {
                    this.PauseMenuExitButton.Visible = !this.PauseMenuExitButton.Visible;
                    //Make the escape/pause/whatever panel visible
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    if (this.SidePanel.Visible && !this.testing)
                    {
                        this.backPanel.Focus();
                        this.SidePanelOpenButton.Text = "Open Side";
                    }
                    else if (!this.testing)
                    {
                        this.SidePanelOpenButton.Text = "Close Side";
                    }
                    this.SidePanel.Visible = !this.SidePanel.Visible;
                }
                double num;
                if (double.TryParse(keyChar, out num))
                {
                    this.piecePlacing = ((int)num) * this.turn;
                }
                else
                {
                    if (keyChar == "S")
                        this.piecePlacing = 10 * this.turn;
                    else if (keyChar == "B")
                        this.piecePlacing = 11 * this.turn;
                    else if (keyChar == "F")
                        this.piecePlacing = 12 * this.turn;
                }
            }
        }

        private void SidePanelOpenButton_MouseClick(object sender, MouseEventArgs e)
        {
            //Makes the side panel open when the button is clicked
            if (this.SidePanel.Visible && !this.testing)
            {
                this.backPanel.Focus();
                this.SidePanelOpenButton.Text = "Open Side";
            }
            else if (!this.testing)
            {
                this.SidePanelOpenButton.Text = "Close Side";
            }
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

        /// <summary>
        /// Called by the button that means the user is done placing pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void donePlacingButton_click(object sender, EventArgs e)
        {
            //nextTurn();
            this.piecePlacing *= -1;
            //if (turn == -1) for (int row = 6; row < boardState.GetLength(1); row++) fillRow(0, row);
            //if (turn == 1) for (int row = 4; row < 6; row++) fillRow(0, row);
            if (turn == 1)
                for (int i = 0; i < 4; i++)
                    for (int x = 0; x < 10; x++)
                        this.boardState[x, i] = 0;
            //if (this.turn == -1)
            ((Button)sender).Enabled = false;
            nextTurn();
            //for (int x = 0; x < this.boardState.GetLength(0); x++) this.boardState[x, row] = value;
        }

        private void SinglePlayerButton_click(object sender, EventArgs e)
        {
            this.isSinglePlayer = true;
            StartButton_Click(sender, e);
        }

        /// <summary>
        /// Finds all of the possible moves for a piece with the given X and Y coordinates using the games board state.
        /// </summary>
        /// <param name="pieceX">X position in the board state (not in pixels)</param>
        /// <param name="pieceY">Y position in the board state (not in pixels)</param>
        /// <returns>A 2D array containing 1 in every space where the deisgnated piece can move and 0 otherwise</returns>
        public int[,] GetPieceMoves(int pieceX, int pieceY)
        {
            return GetPieceMoves(pieceX, pieceY, this.boardState);
        }

        /// <summary>
        /// Finds all of the possible moves for a piece with the given X and Y coordinates using the board state passed in.
        /// </summary>
        /// <param name="X">>X position in the board state (not in pixels)</param>
        /// <param name="Y">Y position in the board state (not in pixels)</param>
        /// <param name="boardState">A 2D array representing the state of the board.</param>
        /// <returns>A 2D array containing 1 in every space where the deisgnated piece can move and 0 otherwise</returns>
        public int[,] GetPieceMoves(int X, int Y, int[,] boardState)
        {
            int[,] moveArray = new int[boardState.GetLength(1), boardState.GetLength(0)];
            if ((Math.Abs(boardState[X, Y]) == 0) || (Math.Abs(boardState[X, Y]) == 11) || (Math.Abs(boardState[X, Y]) == 12) || (Math.Abs(boardState[X, Y]) == 42))
                return moveArray;
            if (Math.Abs(boardState[X, Y]) == 9)
            {
                //for (int yD = Y + 1; yD < boardState.GetLength(1) && boardState[X, yD] == 0; yD++)
                //    moveArray[X, yD] = 1;
                //for (int yU = Y - 1; yU >= 0 && boardState[X, yU] == 0; yU--)
                //    moveArray[X, yU] = 1;
                //for (int xR = X + 1; xR < boardState.GetLength(0) && boardState[xR, Y] == 0; xR++)
                //    moveArray[xR, Y] = 1;
                //for (int xL = X - 1; xL >= 0 && boardState[xL, Y] == 0; xL--)
                //    moveArray[xL, Y] = 1;
                for (int yD = Y + 1; yD < boardState.GetLength(1) && ((Math.Sign(boardState[X, yD]) != Math.Sign(boardState[X, Y])) && boardState[X, yD] != 42); yD++)
                {
                    moveArray[X, yD] = 1;
                    if ((Math.Sign(boardState[X, yD]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[X, yD]) != 0))
                        break;
                }
                for (int yU = Y - 1; yU >= 0 && ((Math.Sign(boardState[X, yU]) != Math.Sign(boardState[X, Y])) && boardState[X, yU] != 42); yU--)
                {
                    moveArray[X, yU] = 1;
                    if ((Math.Sign(boardState[X, yU]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[X, yU]) != 0))
                        break;
                }
                for (int xR = X + 1; xR < boardState.GetLength(0) && ((Math.Sign(boardState[xR, Y]) != Math.Sign(boardState[X, Y])) && boardState[xR, Y] != 42); xR++)
                {
                    moveArray[xR, Y] = 1;
                    if ((Math.Sign(boardState[xR, Y]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[xR, Y]) != 0))
                        break;
                }
                for (int xL = X - 1; xL >= 0 && ((Math.Sign(boardState[xL, Y]) != Math.Sign(boardState[X, Y])) && boardState[xL, Y] != 42); xL--)
                {
                    moveArray[xL, Y] = 1;
                    if ((Math.Sign(boardState[xL, Y]) != Math.Sign(boardState[X, Y])) && (Math.Sign(boardState[xL, Y]) != 0))
                        break;
                }
            }
            if (Y > 0)
                if ((Math.Sign(boardState[X, Y - 1]) != Math.Sign(boardState[X, Y])) && boardState[X, Y - 1] != 42)
                    moveArray[X, Y - 1] = 1;
            if (Y < boardState.GetLength(1) - 1)
                if ((Math.Sign(boardState[X, Y + 1]) != Math.Sign(boardState[X, Y])) && boardState[X, Y + 1] != 42)
                    moveArray[X, Y + 1] = 1;
            if (X < boardState.GetLength(0) - 1)
                if ((Math.Sign(boardState[X + 1, Y]) != Math.Sign(boardState[X, Y])) && boardState[X + 1, Y] != 42)
                    moveArray[X + 1, Y] = 1;
            if (X > 0)
                if ((Math.Sign(boardState[X - 1, Y]) != Math.Sign(boardState[X, Y])) && boardState[X - 1, Y] != 42)
                    moveArray[X - 1, Y] = 1;
            return moveArray;
        }

        private void PlayAgainButton_Click(object sender, EventArgs e)
        {
           
            this.boardState = new int[this.boardState.GetLength(0), this.boardState.GetLength(1)];
            this.turn = 0;
            this.preGameActive = true;
            this.lastFought = new Point(-1, -1);
            this.placements = (int[])this.defaults.Clone();
            nextTurn();
            this.EndGamePanel.Visible = false;
            this.EndGamePanel.Enabled = false;
            this.backPanel.Enabled = true;
            this.backPanel.Focus();
            this.backPanel.Invalidate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSetUpButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if(path.EndsWith("\\bin\\Debug")||path.EndsWith("\\bin\\Release"))
            {
                for(int i =0; i<path.Length-3; i++)
                {
                    if ((path[i] == '\\') && (path[i + 1] == 'b') && (path[i + 2] == 'i') && (path[i + 3] == 'n')) 
                    {
                        path = path.Substring(0, i);
                        break;
                    }
                }
            }
            dialog.InitialDirectory = System.IO.Path.Combine(path, @"Resources\Presets");
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(dialog.FileName);
                saveSetUp(writer);
                writer.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSetUpButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (path.EndsWith("\\bin\\Debug") || path.EndsWith("\\bin\\Release"))
            {
                for (int i = 0; i < path.Length - 3; i++)
                {
                    if ((path[i] == '\\') && (path[i + 1] == 'b') && (path[i + 2] == 'i') && (path[i + 3] == 'n'))
                    {
                        path = path.Substring(0, i);
                        break;
                    }
                }
            }
            dialog.InitialDirectory = System.IO.Path.Combine(path, @"Resources\Presets");
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream file = null;
                try
                {
                    if ((file = dialog.OpenFile()) != null)
                    {
                        StreamReader reader = new StreamReader(file);
                        loadSetUp(reader);
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message);
                }
            }
        }

        private void NextTurnButton_Click(object sender, EventArgs e)
        {
            NextTurnButton.Visible = false;
            this.nextTurn();
        }
    }
}
