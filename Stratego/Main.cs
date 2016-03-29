using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Stratego
{
    public partial class StrategoWin : Form
    {
        /// <summary>
        /// The default amount of pieces for each piece. (EX: 0 0s; 1 1; 1 2; 2 3s; 4 4s; etc..)
        /// </summary>
        public static readonly int[] defaults = new int[13] { 0, 1, 1, 2, 3, 4, 4, 4, 5, 8, 1, 6, 1 };
        //public readonly int[] defaults = new int[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
        //public readonly int[] defaults = new int[13] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 

        /// <summary>
        /// An array that holds the different keys for the Konami code cheat activation
        /// </summary>
        private Keys[] konami = new Keys[8] {Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right};

        /// <summary>
        /// Stores how far through the Konami code the player has entered
        /// </summary>
        private int konamiIndex = 0;

        /// <summary>
        /// Whether or not the game is in testing mode
        /// </summary>
        bool testing = false; // TODO: We should really probably get rid of this :| it basically disables the GUI for the automated tests, but...
                              // at the very least, it's being used way too often. It should really only be used as a GUI-disabler
       
        /// <summary>
        /// Current level of the game. Equals -1 if not in campaign mode
        /// </summary>
        public int level { get; set; }  
        /// <summary>
        /// List of all images for campaign levels
        /// </summary>
        private readonly Bitmap[] levelImages = new Bitmap[] { Properties.Resources.Level1Map, Properties.Resources.Level2Map, Properties.Resources.Level3Map,
            Properties.Resources.Level4Map, Properties.Resources.Level5Map};

        /// <summary>
        /// The piece currently being placed by the user
        /// </summary>
        int piecePlacing = 0;

        /// <summary>
        /// Placeholder for which button on the placement side panel is being used
        /// </summary>
        int activeSidePanelButton = 0;

        /// <summary>
        /// Used by the timer to keep track of the title screen's music & sounds
        /// </summary>
        int ticks = 0;

        /// <summary>
        /// Width of the enclosing panel in pixels
        /// </summary>
        public int panelWidth { get; set; }

        /// <summary>
        /// Height of the enclosing panel in pixels
        /// </summary>
        public int panelHeight { get; set; }

        /// <summary>
        /// The 2DArray full of all pieces on the board
        /// </summary>
        public GamePiece[,] boardState { get; set; }

        /// <summary>
        /// The array which holds information on how many pieces of each type can still be placed
        /// </summary>
        public int[] placements;

        /// <summary>
        /// Whether or not the pre game has begun
        /// </summary>
        public bool preGameActive { get; set; }

        /// <summary>
        /// -1 for player2 and 1 for player 1. 0 when game isn't started. 
        /// 2 for transition from player1 to player2; -2 for transition from player2 to player1.
        /// </summary>
        public int turn { get; set; }            

        /// <summary>
        /// Whether player 2 is an AI or not
        /// </summary>
        public Boolean isSinglePlayer { get; set; }

        /// <summary>
        /// If bombs can be moved
        /// </summary>
        public Boolean movableBombs { get; set; }

        /// <summary>
        /// If flags can be moved
        /// </summary>
        public Boolean movableFlags { get; set; }

        /// <summary>
        /// Coordinates of the last piece to win a battle
        /// </summary>
        public Point lastFought { get; set; }

        /// <summary>
        /// The AI that the player will play against, if they choose single player.
        /// </summary>
        public AI ai;

        /// <summary>
        /// If levels can be skipped using keypresses
        /// </summary>
        private Boolean skippableLevels { get; set; }

        private GamePiece selectedGamePiece;

        private int currTeam;

        /// <summary>
        /// Initializer for normal play (initializes GUI).
        /// Not to be used for testing!
        /// </summary>
        public StrategoWin()
        {
            SoundPlayerAsync.PlaySound(Properties.Resources.BattleDramatic);
            InitializeComponent();
            this.DoubleBuffered = true;
            this.StartButton.FlatStyle = FlatStyle.Flat;
            this.StartButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, Color.Red);
            this.StartButton.FlatAppearance.BorderSize = 0;
            this.StartButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, Color.Red);
            this.panelWidth = this.backPanel.Width;
            this.panelHeight = this.backPanel.Height;
            this.turn = 0;
            this.preGameActive = false;
            this.skippableLevels = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            this.level = -1;
            this.backPanel.LostFocus += onBackPanelLostFocus;
            this.selectedGamePiece = null;
            this.currTeam = 0;
            // Initialize the board state with invalid spaces in the enemy player's side
            // of the board and empty spaces everywhere else. To be changed later!
            boardState = new GamePiece[10, 10];
            for (int row = 0; row < 6; row++) fillRow(42, row);

            this.ai = new AI(this, -1);

            this.backPanel.Focus();
        }

        /*

        private static void MusicInBackground()
        {
            UnmanagedMemoryStream uM = Properties.Resources.BattleDramatic;
            SoundPlayer sp = new SoundPlayer(uM);
            while (true)
            {
                sp.PlaySync();
            }
        }

        */

        /// <summary>
        /// Initializer for the testing framework.
        /// Excludes all GUI elements from initialization.
        /// </summary>
        /// <param name="windowWidth">Used for a simulated GUI window width</param>
        /// <param name="windowHeight">Used for a simulated GUI window height</param>
        /// <param name="boardState">Modified initial board state for ease of testing</param>
        public StrategoWin(int windowWidth, int windowHeight, GamePiece[,] boardState)
        {
            this.testing = true;
            this.panelWidth = windowWidth;
            this.panelHeight = windowHeight;
            this.boardState = boardState;
            this.placements = StrategoWin.defaults;
            this.preGameActive = false;
            this.isSinglePlayer = false;
            this.lastFought = new Point(-1, -1);
            this.movableBombs = false;
            this.movableFlags = false;
            this.ai = new AI(this, -1);
           // Image imag = Properties.Resources.cursor.Tag;
            //System.Windows.Forms.Cursor.Current = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("sword"));
            //System.Windows.Forms.Cursor.Current = new Cursor(GetType(), "sword.cur");
            //this.Cursor = new Cursor(GetType(), "sword.cur");
        }

        /// <summary>
        /// Called by the start button on the main menu.
        /// </summary>
        /// <param name="sender">Button that was pressed</param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            /* Make function for below action - using dead memory stream to signify clearing all music on the other thread */
            Stream streamDead = new MemoryStream();
            SoundPlayerAsync.PlaySound(streamDead);

            SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
            sound.Play();
            this.FireBox.Dispose();
            this.placements = StrategoWin.defaults;
            this.backPanel.BackgroundImage = Properties.Resources.BoardUpdate;
            nextTurn();
            this.LoadButton.Visible = false;
            this.LoadButton.Enabled = false;
            this.ExitMainButton.Visible = false;
            this.ExitMainButton.Enabled = false;
            this.CampaignButton.Visible = false;
            this.CampaignButton.Enabled = false;
            this.SinglePlayerButton.Visible = false;
            this.SinglePlayerButton.Enabled= false;
            this.SidePanelOpenButton.Visible = true;
            foreach (var button in this.SidePanel.Controls.OfType<Button>())
                if (button.Name != donePlacingButton.Name && button.Name != saveSetUpButton.Name && button.Name != loadSetUpButton.Name)
                    button.Click += SidePanelButtonClick;
            this.backPanel.Focus();
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
                this.LoadButton.Enabled = true;
                this.ExitMainButton.Visible = true;
                this.ExitMainButton.Enabled = true;
                this.CampaignButton.Visible = true;
                this.CampaignButton.Enabled = true;
                this.SinglePlayerButton.Visible = true;
                this.SinglePlayerButton.Enabled = true;
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
                        GamePiece piece = this.boardState[x, y];
                        Brush b = new SolidBrush(piece.getPieceColor());
                        pen.Color = Color.FromArgb(200, 200, 255);
                        // pen.Color = Color.FromArgb(255, 200, 200);
                        int cornerX = x * col_inc + paddingX;
                        int cornerY = y * row_inc + paddingY;
                        Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);
                        if (pieceMoves[x, y] == 1)
                        {
                            r = new Rectangle(x * scaleX + 1, y * scaleY + 1, scaleX - 2, scaleY - 2);
                            //b.Color = Color.FromArgb(90, 90, 255);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 130, 130, 130)), r);
                        }
                        else if (this.currTeam == piece.getTeamCode() || this.lastFought.Equals(new Point(x, y)))
                        {
                            Image imag = piece.getPieceImage();
                            e.Graphics.DrawImage(imag, r);
                            if (this.selectedGamePiece.Equals(piece))
                            {
                                pen.Color = Color.FromArgb(10, 255, 10);
                            }
                            g.DrawRectangle(pen, r);
                        }
                        else
                        {
                            g.DrawRectangle(pen, r);
                            g.FillRectangle(b, r);
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
        public GamePiece getPiece(int x, int y) 
        {
            return this.boardState[x,y];
        }

        /// <summary>
        /// Fills the given row in the board state with the given value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
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
        /// Looks at the current turn, and changes it to whatever the next turn should be.
        /// Handles global game variables like the stage of the game and so on.
        /// Also sends a call to the AI to notify it that it's time to take its turn when necessary.
        /// </summary>
        public void nextTurn() 
        {
            /*
            if (!testing)
                this.backPanel.Invalidate();
            */

            // We just came here from the main menu
            if(this.currTeam == 0)
            {
                preGameActive = true;
                this.turn = 1;
            }
            // It's blue player's turn
            else if (this.turn == 1)
            {
                if (this.preGameActive)
                {
                    this.turn = -1;
                    this.placements = StrategoWin.defaults;
                }
                else
                {
                    this.turn = 2;
                    if (!this.checkMoves())
                        this.gameOver(1);
                    else
                    {
                        if (!testing)
                        {
                            if (!this.isSinglePlayer)
                                NextTurnButton.Text = "Player 2's Turn";
                            else
                                NextTurnButton.Text = "AI's Turn";
                            NextTurnButton.Visible = true;
                            this.NextTurnButton.Enabled = true;
                        }
                    }
                }
            }
            // It's red player's turn
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
                    if (!this.testing)
                    {
                        this.SidePanelOpenButton.Visible = false;
                        this.SidePanel.Visible = false;
                    }
                }
                if (!this.isSinglePlayer || (this.lastFought != new Point(-1, -1))) this.turn = -2;
                else this.turn = 1;
                if (!this.checkMoves())
                    this.gameOver(-1);
                else
                {
                    if (!testing && (!this.isSinglePlayer || (this.lastFought != new Point(-1, -1))))
                    {
                        NextTurnButton.Text = "Player 1's Turn";
                        NextTurnButton.Visible = true;
                        this.NextTurnButton.Enabled = true;

                    }
                }
                
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
        public bool SelectPiece(int x, int y)
        {
            /*
                The if-block immediate below is NOT what we want, but haven't changed it yet to remind us
                to change the general strategy other places too. We'll need to come back and spot-check this stuff.
            */
            if ((Math.Abs(turn) == 2) || (turn == -1 && isSinglePlayer))
            {
                return false;
            }
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            int boardX = x / scaleY;
            int boardY = y / scaleY;
            if (this.selectedGamePiece != null)
            {
                return true;
            }
            GamePiece potentialSel = this.boardState[boardX, boardY];
            if (potentialSel == null)
            {
                this.selectedGamePiece = null;
                return false;
            }
            if ((!potentialSel.isMovable() || potentialSel.getTeamCode() != this.currTeam))
            {
                this.selectedGamePiece = null;
                return false;
            }
            this.selectedGamePiece = potentialSel;
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
            int boardX = x / scaleX;
            int boardY = y / scaleY;
            GamePiece defender = this.boardState[boardX, boardY];
            if (this.selectedGamePiece == null)
            {
                return false;
            }
            else if(this.selectedGamePiece.getXVal() == boardX && this.selectedGamePiece.getYVal() == boardY)
            {
                // Initialize "Selection Phase"
                this.selectedGamePiece = null;
                return false;
            }
            else if (defender == null)
            {
                this.lastFought = new Point(-1, -1);
                this.boardState[boardX, boardY] = this.selectedGamePiece;
                this.boardState[this.selectedGamePiece.getXVal(), this.selectedGamePiece.getYVal()] = null;
                this.selectedGamePiece.setXVal(boardX);
                this.selectedGamePiece.setYVal(boardY);
                this.selectedGamePiece = null;
                return true;
            }
            else
            {
                if (this.selectedGamePiece.getTeamCode() == defender.getTeamCode())
                {
                    this.selectedGamePiece = null;
                    return false;
                }
                int numOfSpacesPoss = this.selectedGamePiece.getLimitToMovement();
                int deltaX = Math.Abs(boardX - this.selectedGamePiece.getXVal());
                int deltaY = Math.Abs(boardY - this.selectedGamePiece.getYVal());
                if(numOfSpacesPoss < deltaX || numOfSpacesPoss < deltaY)
                {
                    this.selectedGamePiece = null;
                    return false;
                }
            }
            battlePieces(this.selectedGamePiece, defender);
            this.lastFought = new Point(boardX, boardY);
            if (!defender.isAlive() && defender.isEssential())
            {
                gameOver(this.selectedGamePiece.getTeamCode());
            }
            if (!this.selectedGamePiece.isAlive() && this.selectedGamePiece.isEssential())
            {
                gameOver(defender.getTeamCode());
            }
            else {
                this.nextTurn();
            }
            this.selectedGamePiece = null;
            return true;
        }

        private void battlePieces(GamePiece attacker, GamePiece defender)
        {
            attacker.attack(defender);
            defender.defend(attacker);
            if (!defender.isAlive())
            {
                this.boardState[defender.getXVal(), defender.getYVal()] = null;
            }
            if (!attacker.isAlive())
            {
                this.boardState[attacker.getXVal(), attacker.getYVal()] = null;
            }
            else
            {
                this.boardState[defender.getXVal(), defender.getYVal()] = attacker;
            }
        }

        /// <summary>
        /// Loads a premade piece setup onto the current team's side of the board
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns> True if Successful
        public bool loadSetup(string fileName)
        {
            if (!this.preGameActive || (this.boardState.GetLength(0) != 10) || (this.boardState.GetLength(1) != 10) || (Math.Abs(turn)==2)) return false;

            loadSetupData(fileName);

            if(!this.testing)
            {
                this.backPanel.Invalidate();

                for (int i = 1; i < this.placements.Length; i++)
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
        /// Receives clicks on the back panel and directs them to the game as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.turn == 0 || this.OptionsPanel.Visible|| this.EndGamePanel.Visible) return;

            if (preGameActive)
            {
                bool? piecePlaced = placePiece(this.piecePlacing, e.X, e.Y);

                // Only run if the placement succeeded
                if (piecePlaced.Value)
                {
                    int scaleX = this.panelWidth / this.boardState.GetLength(0);
                    int scaleY = this.panelHeight / this.boardState.GetLength(1);
                    //This makes it so it only repaints the rectangle where the piece is placed
                    Rectangle r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                    this.backPanel.Invalidate(r);

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
                            this.backPanel.Invalidate(new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY));

                Rectangle r = new Rectangle(this.pieceSelectedCoords.X * scaleX, this.pieceSelectedCoords.Y * scaleY, scaleX, scaleY);
                this.backPanel.Invalidate(r);
                this.MovePiece(e.X, e.Y);
                if (this.EndGamePanel.Enabled == true)
                    return;
                //This makes it so it only repaints the rectangle where the piece is placed
                r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                this.backPanel.Invalidate(r);
                r = new Rectangle(this.pieceSelectedCoords.X * scaleX, this.pieceSelectedCoords.Y * scaleY, scaleX, scaleY);
                this.backPanel.Invalidate(r);
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
                            this.backPanel.Invalidate(new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY));
                if (pieceIsSelected)
                    pieceMoves = this.GetPieceMoves(this.pieceSelectedCoords.X, this.pieceSelectedCoords.Y);
                //r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                this.backPanel.Invalidate(new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY));
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
            if (this.konami[this.konamiIndex] == e.KeyCode)
            {
                konamiIndex++;
                if (konamiIndex >= this.konami.Length)
                {
                    konamiIndex = 0;
                    konamiCodeEntered();
                }
            }
            else
                konamiIndex = 0;


            if(this.turn != 0 && !this.preGameActive)
            {
                if (e.KeyCode == Keys.PageUp && this.skippableLevels && this.level>0)
                {
                    if (this.level < this.levelImages.Length)
                        this.loadNextLevel();
                    else
                        this.gameOver(1);
                }
                else if(e.KeyCode == Keys.PageDown && this.skippableLevels && this.level>1 &&this.EndGamePanel.Visible)
                {
                    this.level-=2;
                    this.loadNextLevel();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    // Change the pause panel's visibility to whatever it's not
                    this.OptionsPanel.Visible = !this.OptionsPanel.Visible;
                }
                else if(e.KeyCode == Keys.ShiftKey && this.preGameActive)
                {
                    if (this.SidePanel.Visible)
                        this.SidePanelOpenButton.Text = "Open Side";
                    else
                        this.SidePanelOpenButton.Text = "Close Side";
                    this.SidePanel.Visible = !this.SidePanel.Visible;
                }
                else if(e.KeyCode == Keys.Enter && Math.Abs(turn) == 2)
                {
                    NextTurnButton.Visible = false;
                    this.NextTurnButton.Enabled = false;
                    this.nextTurn();
                }
            }
            else if (this.preGameActive)
            {
                KeysConverter kc = new KeysConverter();
                string keyChar = kc.ConvertToString(e.KeyCode);
                if(e.KeyCode==Keys.Escape)
                {
                    //Make the escape/pause/options panel visible
                    this.OptionsPanel.Visible = !this.OptionsPanel.Visible;
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    // Either open or close the side panel depending on whatever
                    if (this.SidePanel.Visible)
                    {
                        this.SidePanelOpenButton.Text = "Open Side";
                    }
                    else
                        this.SidePanelOpenButton.Text = "Close Side";
                    this.SidePanel.Visible = !this.SidePanel.Visible;
                }
                double num;
                if (double.TryParse(keyChar, out num))
                    this.piecePlacing = ((int)num) * this.turn;
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

        /// <summary>
        /// Trigger that activates when the SidePanelOpenButton is clicked. Simply opens the side panel and changes the text on the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SidePanelOpenButton_MouseClick(object sender, MouseEventArgs e)
        {
            //Makes the side panel open when the button is clicked
            if (this.SidePanel.Visible && !this.testing)
            {
                this.SidePanelOpenButton.Text = "Open Side";
            }
            else if (!this.testing)
            {
                this.SidePanelOpenButton.Text = "Close Side";
            }
            this.SidePanel.Visible = !this.SidePanel.Visible;
        }

        /// <summary>
        /// Trigger that activates when the PauseMenuExitButton is pressed, exits the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseMenuExitButton_Click(object sender, EventArgs e)
        {
            //Makes the program close if the exit button is pressed
            if (Application.MessageLoop)
                Application.Exit();
        }

        /// <summary>
        /// Moves the PauseMenuExitButton to change position based on screen size when it becomes visible/invisible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseMenuExitButton_VisibleChanged(object sender, EventArgs e)
        {
            //Sets the exit button to be in the center of the panel whenever it is made visible/not
            int scaleX = this.panelWidth / this.boardState.GetLength(0);
            int scaleY = this.panelHeight / this.boardState.GetLength(1);
            this.Location = new Point(this.panelWidth / 2 - ((Button)sender).Width / 2, this.panelHeight / 2 - ((Button)sender).Height / 2);
        }

        /// <summary>
        /// Trigger that activates when the removeCheckBox is checked or unchecked. Enables/disables deleting of pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when the SinglePlayerButton in the main menu is pressed, throws the player into a new single player game against the AI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        public int[,] GetPieceMoves(int X, int Y, GamePiece[,] boardState)
        {
            int[,] moveArray = new int[boardState.GetLength(1), boardState.GetLength(0)];

            GamePiece pieceInQuestion = boardState[X, Y];
            if (pieceInQuestion.isMovable() || pieceInQuestion.getLimitToMovement() == 0)
            {
                return moveArray;
            }
            int startingX = pieceInQuestion.getXVal();
            int startingY = pieceInQuestion.getYVal();
            int spacesPossible = pieceInQuestion.getLimitToMovement();
            for(int k = startingX; k <= startingX + spacesPossible; k++)
            {
            }
            return moveArray;
        }

        /// <summary>
        /// Handles what happens when the user clicks the play again button after one game has finished.
        /// Should basically reinitialize the entire board and everything.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayAgainButton_Click(object sender, EventArgs e)
        {
            if (this.level > this.levelImages.Length)
            {
                Application.Restart();
            }
            else if (this.level > -1)
            {
                loadNextLevel();
            }
            else
            {
                this.boardState = new int[this.boardState.GetLength(0), this.boardState.GetLength(1)];
                for (int row = 0; row < 6; row++) fillRow(42, row);
                this.turn = 0;
                this.preGameActive = true;
                this.lastFought = new Point(-1, -1);
                this.placements = StrategoWin.defaults;
                this.ai = new AI(this, -1);
                nextTurn();
                this.SidePanel.Visible = false;
                this.SidePanelOpenButton.Visible = true;
                this.SidePanelOpenButton.Text = "Open Side";
            }
            this.EndGamePanel.Visible = false;
            this.EndGamePanel.Enabled = false;
            this.backPanel.Enabled = true;
            this.backPanel.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSetUpButton_Click(object sender, EventArgs e)
        {
            if (!preGameActive || (this.boardState.GetLength(0) != 10) || (this.boardState.GetLength(1) != 10) || (Math.Abs(turn) == 2)) return;

            SaveLoadOperations.saveSetup(getSetupData());
        }

        /// <summary>
        /// Handles what happens when the "load setup" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSetUpButton_Click(object sender, EventArgs e)
        {
            loadSetup(SaveLoadOperations.loadSetup());
        }

        /// <summary>
        /// Trigger that is called when the next turn button is pressed. It flips the turn to the inbetween state, which waits for player confirmation to flip the turn to the other player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextTurnButton_Click(object sender, EventArgs e)
        {
            NextTurnButton.Visible = false;
            this.NextTurnButton.Enabled = false;
            this.nextTurn();

        }

        /// <summary>
        /// Trigger that is called when the x button on the options panel is clicked, it makes the options panel no longer visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xButton_Click(object sender, EventArgs e)
        {
            this.OptionsPanel.Visible = !this.OptionsPanel.Visible;
        }

        /// <summary>
        /// Trigger that is called when the convede button in the options manu is clicked, it causes the player who clicked the button to lost the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void concedeButton_Click(object sender, EventArgs e)
        {
            // This should always be true, but if 0 was passed into gameOver() bad things would happen
            if (Math.Abs(this.turn) == 1)
                gameOver(-1*this.turn);
        }

        /// <summary>
        /// Returns to the main menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnToMenuButton_Click(object sender, EventArgs e)
        {
            // Just restart the application, since the main menu opens right at the beginning anyway
            Application.Restart();
        }

        /// <summary>
        /// Changes the difficulty variable whenever the difficulty in the options menu is altered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AIDifficultyChanger_SelectedItemChanged(object sender, EventArgs e)
        {
            if (this.ai != null)
                this.ai.difficulty = Convert.ToInt32(this.AIDifficultyChanger.SelectedItem);
        }

        /// <summary>
        /// Flips the focus back to the back panel whenever it loses focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onBackPanelLostFocus(object sender, EventArgs e)
        {
            this.backPanel.Focus();
        }

        /// <summary>
        /// Handles the end of the game when player on the given team wins
        /// </summary>
        /// <param name="winnerTeam"></param>
        public void gameOver(int winnerTeam)
        {
            if (!this.testing)
            {
                this.OptionsPanel.Visible = false;
                if(this.level>-1)
                {
                    if (winnerTeam == 1)
                    {
                        if(this.level == this.levelImages.Length)
                        {
                            this.PlayAgainButton.Text = "Main Menu";
                            this.victoryLabel.Text = "YOU BEAT THE CAMPAIGN!";
                            this.level++;
                        }
                        else
                            this.PlayAgainButton.Text = "Next Level";
                    }
                    else
                    {
                        this.level--;
                        this.PlayAgainButton.Text = "Try Again";
                    }
                }
                else
                {
                    this.PlayAgainButton.Text = "Play Again";
                }
                if (this.level < this.levelImages.Length)
                {
                    if (winnerTeam == 1)
                    {
                        if (this.isSinglePlayer)
                            this.victoryLabel.Text = "YOU ARE VICTORIOUS, PLAYER 1.";
                        else
                            this.victoryLabel.Text = "BLUE PLAYER WINS.";
                    }
                    else
                    {
                        if (this.isSinglePlayer)
                            this.victoryLabel.Text = "WOW, YOU LOST TO THAT? ...SERIOUSLY?";
                        else
                            this.victoryLabel.Text = "RED PLAYER WINS.";
                    }
                }


                this.EndGamePanel.Visible = true;
                this.EndGamePanel.Enabled = true;
            }
        }

        public Boolean checkMoves()
        {
            for (int x1 = 0; x1 < this.boardState.GetLength(0); x1++)
            {
                for (int y1 = 0; y1 < this.boardState.GetLength(1); y1++)
                {
                    int piece = boardState[x1, y1];
                    if ((piece<0&&(this.turn==-1||this.turn==2))||(piece>0&&(this.turn==1||turn==-2)))
                    {
                        int[,] validPlaces = GetPieceMoves(x1, y1, this.boardState);
                        for (int x2 = 0; x2 < this.boardState.GetLength(0); x2++)
                        {
                            for (int y2 = 0; y2 < this.boardState.GetLength(1); y2++)
                            {
                                if (validPlaces[x2, y2] == 1)
                                    return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// What to do if the movable bomb check box in the options menu is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void movableBombCB_CheckedChanged(object sender, EventArgs e)
        {
            this.movableBombs = !this.movableBombs;
        }

        /// <summary>
        /// What to do if the movable flag check box in the options menu is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void movableFlagCB_CheckedChanged(object sender, EventArgs e)
        {
            this.movableFlags = !this.movableFlags;
        }

        /// <summary>
        /// What to do when the konami code is entered
        /// </summary>
        private void konamiCodeEntered()
        {
            this.movableBombCB.Enabled = true;
            this.movableFlagCB.Enabled = true;
            this.skippableLevels = true;
        }

        /// <summary>
        /// Loads the next level of the campaign. If the game is not in campaign mode, it starts at the first level of the campaign
        /// </summary>
        private void loadNextLevel()
        {
            if (this.level < -1 && (this.level+1)>=this.levelImages.Length) return;
            this.level++;
            if (this.level == 0) this.level++;
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
            path += @"\Resources\SaveGames\Levels\Level" + this.level + ".strat";

            loadSaveData(SaveLoadOperations.loadSaveData(path));

            this.preGameActive = false;
            this.lastFought = new Point(-1, -1);

            if (!this.testing)
            {
                this.backPanel.BackgroundImage = this.levelImages[level - 1];
                if (this.turn == -2) this.NextTurnButton.Text = "Player 1's Turn";
                else this.NextTurnButton.Text = "AI's Turn";
                this.NextTurnButton.Visible = true;
                this.NextTurnButton.Enabled = true;
                this.backPanel.Invalidate();

            }
        }

        private void CampaignButton_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
            sound.Play();
            this.FireBox.Dispose();
            this.backPanel.BackgroundImage = Properties.Resources.BoardUpdate;
            this.LoadButton.Visible = false;
            this.LoadButton.Enabled = false;
            this.ExitMainButton.Visible = false;
            this.ExitMainButton.Enabled = false;
            this.CampaignButton.Visible = false;
            this.CampaignButton.Enabled = false;
            this.SinglePlayerButton.Visible = false;
            this.SinglePlayerButton.Enabled = false;
            this.SidePanelOpenButton.Visible = false;

            loadNextLevel();
            backPanel.Focus();
        }

        /// <summary>
        /// A trigger that activates when the SaveButton in the main menu is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // if ((this.turn == 0) || (this.preGameActive) || (Math.Abs(this.turn) == 2)) return; // TODO: this line may be necessary?
            SaveLoadOperations.saveGame(getSaveData());
        }

        /// <summary>
        /// A trigger that activates when the LoadButton in the main manu is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            SaveData? saveData = SaveLoadOperations.loadGame();

            if (saveData != null)
            {
                loadSaveData(saveData ?? default(SaveData));

                try
                {
                    SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
                    sound.Play();
                    this.FireBox.Dispose();
                    this.backPanel.BackgroundImage = Properties.Resources.BoardUpdate;
                    this.LoadButton.Visible = false;
                    this.LoadButton.Enabled = false;
                    this.ExitMainButton.Visible = false;
                    this.ExitMainButton.Enabled = false;
                    this.CampaignButton.Visible = false;
                    this.CampaignButton.Enabled = false;
                    this.SinglePlayerButton.Visible = false;
                    this.SinglePlayerButton.Enabled = false;
                    this.SidePanelOpenButton.Visible = false;
                    if (this.turn == 2 && this.isSinglePlayer)
                        this.NextTurnButton.Text = "AI's Turn";
                    else if (this.turn == 2)
                        this.NextTurnButton.Text = "Player 2's Turn";
                    if (this.isSinglePlayer)
                    {
                        this.AIDifficultyChanger.Text = this.ai.difficulty.ToString();
                    }
                    this.NextTurnButton.Visible = true;
                    this.NextTurnButton.Enabled = true;
                    this.preGameActive = false;
                    this.lastFought = new Point(-1, -1);

                    foreach (var button in this.SidePanel.Controls.OfType<Button>())
                        if (button.Name != donePlacingButton.Name && button.Name != saveSetUpButton.Name && button.Name != loadSetUpButton.Name)
                            button.Click += SidePanelButtonClick;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message);
                }
            }
            backPanel.Focus();
        }

        private SaveData getSaveData()
        {
            return new SaveData(
                this.boardState,
                this.ai.difficulty,
                this.turn,
                this.isSinglePlayer
                );
        }

        private void loadSaveData(SaveData data)
        {
            this.boardState = data.boardState;
            this.turn = data.turn;
            this.isSinglePlayer = data.isSinglePlayer;
            this.ai.difficulty = data.difficulty;

            if (this.isSinglePlayer)
                this.ai = new AI(this, -1, data.difficulty);
        }

        public SetupData getSetupData()
        {
            return new SetupData(
                this.boardState,
                this.placements,
                this.turn
                );
        }

        private void loadSetupData(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);

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
            int[] placements = StrategoWin.defaults;
            if (turn > 0)
            {
                for (int j = 6; j < 10; j++)
                {
                    numbers = lines[j - 6].Split(' ');
                    for (int k = 0; k < 10; k++)
                    {
                        boardState[k, j] = Convert.ToInt32(numbers[k]);
                        if (Convert.ToInt32(numbers[k]) != 0) placements[Math.Abs(boardState[k, j])] -= 1;
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
                        boardState[9 - k, 3 - j] = turn * Convert.ToInt32(numbers[k]);
                        if (Convert.ToInt32(numbers[k]) != 0) this.placements[Math.Abs(boardState[9 - k, 3 - j])] -= 1;
                    }
                }
            }

            reader.Close();
        }
    }
}
