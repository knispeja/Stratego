using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Drawing.Drawing2D;

namespace Stratego
{
    public partial class StrategoWin : Form, GUICallback
    {

        /// <summary>
        /// An array that holds the different keys for the Konami code cheat activation
        /// </summary>
        private Keys[] konami = new Keys[8] { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right };

        /// <summary>
        /// Stores how far through the Konami code the player has entered
        /// </summary>
        private int konamiIndex = 0;

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

        private StrategoGame game;

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

            this.game = new StrategoGame(this);
            this.level = -1;

            this.backPanel.LostFocus += onBackPanelLostFocus;
            this.backPanel.Focus();
        }

        /// <summary>
        /// Initializer for the testing framework.
        /// Excludes all GUI elements from initialization.
        /// </summary>
        /// <param name="windowWidth">Used for a simulated GUI window width</param>
        /// <param name="windowHeight">Used for a simulated GUI window height</param>
        /// <param name="boardState">Modified initial board state for ease of testing</param>
        public StrategoWin(int windowWidth, int windowHeight, Gameboard boardState)
        {
            this.panelWidth = windowWidth;
            this.panelHeight = windowHeight;
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
            
            this.game.resetPlacements();
            this.backPanel.BackgroundImage = Properties.Resources.BoardUpdate;

            this.game.nextTurn();
            this.LoadButton.Visible = false;
            this.LoadButton.Enabled = false;
            this.ExitMainButton.Visible = false;
            this.ExitMainButton.Enabled = false;
            this.CampaignButton.Visible = false;
            this.CampaignButton.Enabled = false;
            this.SinglePlayerButton.Visible = false;
            this.SinglePlayerButton.Enabled = false;
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
                this.game.setPiecePlacing(((Button)sender).Tag.ToString());
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
            if (this.game.turn != 0)
            {
                this.panelWidth = this.backPanel.Width;
                this.panelHeight = this.backPanel.Height;

                Pen pen = new Pen(Color.White, 1);
                Graphics g = e.Graphics;

                Gameboard boardState = this.game.boardState;
                int num_cols = boardState.getWidth();
                int num_rows = boardState.getHeight();
                int col_inc = panelWidth / num_cols;
                int row_inc = panelHeight / num_rows;

                // Draw vertical gridlines
                for (int i = 0; i < num_cols + 1; i++)
                {
                    g.DrawLine(pen, col_inc * i, 0, col_inc * i, panelHeight);
                }

                // Draw horizontal gridlines
                for (int j = 0; j < num_rows + 1; j++)
                {
                    g.DrawLine(pen, 0, row_inc * j, panelWidth, row_inc * j);
                }

                int[,] pieceMoves = new int[num_rows, num_cols];

                GamePiece selectedGamePiece = null;
                if (!BoardPosition.isNull(this.game.selectedPosition))
                {
                    pieceMoves = this.game.GetPieceMoves(this.game.selectedPosition.getX(), this.game.selectedPosition.getY());
                    selectedGamePiece = this.game.boardState.getPiece(this.game.selectedPosition);
                }

                // Large loop which draws the necessary circles/images that represent pieces
                int diameter = Math.Min(col_inc, row_inc);
                int paddingX = (col_inc - diameter) / 2;
                int paddingY = (row_inc - diameter) / 2;
                int scaleX = this.panelWidth / boardState.getWidth();
                int scaleY = this.panelHeight / boardState.getHeight();

                for (int x = 0; x < boardState.getWidth(); x++)
                {
                    for (int y = 0; y < boardState.getHeight(); y++)
                    {
                        GamePiece piece = boardState.getPiece(x, y);
                        if (piece != null && piece.getTeamCode() != StrategoGame.NO_TEAM_CODE)
                        {
                            Brush b = new SolidBrush(piece.getPieceColor());
                            pen.Color = Color.FromArgb(200, 200, 255);

                            int cornerX = x * col_inc + paddingX;
                            int cornerY = y * row_inc + paddingY;
                            Rectangle r = new Rectangle(x * scaleX + (scaleX - (int)(scaleY * .55)) / 2, y * scaleY + 5, (int)(scaleY * .55), scaleY - 10);

                            if (this.game.turn == piece.getTeamCode() || boardState.getLastFought() != null && boardState.getLastFought().Equals(new Point(x, y)))
                            {
                                Image imag = piece.getPieceImage();
                                e.Graphics.DrawImage(imag, r);
                                if (piece == selectedGamePiece)
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
                            b.Dispose();
                            
                        }
                        if (pieceMoves[x, y] == 1)
                        {
                            Rectangle r = new Rectangle(x * scaleX + 1, y * scaleY + 1, scaleX - 2, scaleY - 2);
                            //b.Color = Color.FromArgb(90, 90, 255);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 130, 130, 130)), r);
                        }
                    }
                }
                GraphicsPath p = new GraphicsPath();
                String[] killFeed = this.game.getKillFeed();
                for (int i = 0; i < StrategoGame.KILL_FEED_SIZE; i++)
                {
                    p.AddString(
                        killFeed[i],             // text to draw
                        FontFamily.GenericSansSerif,  // or any other font family
                        (int)FontStyle.Bold,      // font style (bold, italic, etc.)
                        g.DpiY * 20 / 72,       // em size
                        new Point(0, 25 * (i + 1)),              // location where to draw text
                        new StringFormat());          // set options here (e.g. center alignment)
                    g.DrawPath(Pens.Black, p);
                    g.FillPath(Brushes.White, p);
                    p.Reset();
                }
                pen.Dispose();
            }
        }

        /// <summary>
        /// Receives clicks on the back panel and directs them to the game as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.game.turn == 0 || this.OptionsPanel.Visible || this.EndGamePanel.Visible) return;

            Gameboard boardState = this.game.boardState;
            int scaleX = this.panelWidth / boardState.getWidth();
            int scaleY = this.panelHeight / boardState.getHeight();
            int boardX = e.X / scaleX;
            int boardY = e.Y / scaleY;

            if (this.game.preGameActive)
            {
                // Only run if the placement succeeded
                if (this.game.placePiece(boardX, boardY))
                {
                    this.game.resetPiecePlacing();
                    //This makes it so it only repaints the rectangle where the piece is placed
                    Rectangle r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                    this.backPanel.Invalidate(r);

                    this.donePlacingButton.Enabled = this.game.isDonePlacing();
                }
            }
            else if (!BoardPosition.isNull(this.game.selectedPosition))
            {
                int[,] pieceMoves = this.game.GetPieceMoves(this.game.selectedPosition.getX(), this.game.selectedPosition.getY());

                //Rectangle r = new Rectangle(this.game.selectedPosition.getX() * scaleX, this.game.selectedPosition.getY() * scaleY, scaleX, scaleY);
                //this.backPanel.Invalidate(r);
                Rectangle r;
                this.game.MovePiece(boardX, boardY);
                for (int x = 0; x < this.game.boardState.getWidth(); x++)
                    for (int y = 0; y < this.game.boardState.getHeight(); y++)
                        if (pieceMoves[x, y] == 1)
                            this.backPanel.Invalidate(new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY));

                if (this.EndGamePanel.Enabled == true)
                {
                    backPanel.Invalidate();
                    return;
                }
                //This makes it so it only repaints the rectangle where the piece is placed
                r = new Rectangle((int)(e.X / scaleX) * scaleX, (int)(e.Y / scaleY) * scaleY, scaleX, scaleY);
                this.backPanel.Invalidate(r);

                r = new Rectangle(this.game.selectedPosition.getX() * scaleX, this.game.selectedPosition.getY() * scaleY, scaleX, scaleY);
                this.backPanel.Invalidate(r);
            }
            else if (this.game.SelectPiece(boardX, boardY).Value)
            {
                //This makes it so it only repaints the rectangle where the piece is placed
                int[,] pieceMoves = this.game.GetPieceMoves(this.game.selectedPosition.getX(), this.game.selectedPosition.getY());
                for (int x = 0; x < this.game.boardState.getWidth(); x++)
                    for (int y = 0; y < this.game.boardState.getHeight(); y++)
                        if (pieceMoves[x, y] == 1)
                            this.backPanel.Invalidate(new Rectangle(x * scaleX, y * scaleY, scaleX, scaleY));
                if (!BoardPosition.isNull(this.game.selectedPosition))
                    pieceMoves = this.game.GetPieceMoves(this.game.selectedPosition.getX(), this.game.selectedPosition.getY());

                this.backPanel.Invalidate(new Rectangle((int) (e.X * scaleX) * scaleX, (int)(e.Y / scaleY) *scaleY, scaleX, scaleY));
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

            if (this.game.turn != 0 && !this.game.preGameActive)
            {
                if (e.KeyCode == Keys.PageUp && this.game.skippableLevels && this.level > 0)
                {
                    if (this.level < this.levelImages.Length)
                        this.loadNextLevel();
                    else
                        this.gameOver(1);
                }
                else if (e.KeyCode == Keys.PageDown && this.game.skippableLevels && this.level > 1 && this.EndGamePanel.Visible)
                {
                    this.level -= 2;
                    this.loadNextLevel();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    // Change the pause panel's visibility to whatever it's not
                    this.OptionsPanel.Visible = !this.OptionsPanel.Visible;
                }
                else if (e.KeyCode == Keys.ShiftKey && this.game.preGameActive)
                {
                    if (this.SidePanel.Visible)
                        this.SidePanelOpenButton.Text = "Open Side";
                    else
                        this.SidePanelOpenButton.Text = "Close Side";
                    this.SidePanel.Visible = !this.SidePanel.Visible;
                }
                else if (e.KeyCode == Keys.Enter && Math.Abs(this.game.turn) == 2)
                {
                    NextTurnButton.Visible = false;
                    this.NextTurnButton.Enabled = false;
                    this.game.nextTurn();
                }
            }
            else if (this.game.preGameActive)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    //Make the escape/pause/options panel visible
                    this.OptionsPanel.Visible = !this.OptionsPanel.Visible;
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    toggleSidePanelOpen();
                }

                double num;
                KeysConverter kc = new KeysConverter();
                string keyChar = kc.ConvertToString(e.KeyCode);

                if (double.TryParse(keyChar, out num))
                {
                    this.game.setPiecePlacing((int) num);
                }
                else
                {
                    this.game.setPiecePlacing(keyChar);
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
            toggleSidePanelOpen();
        }

        private void toggleSidePanelOpen()
        {
            this.SidePanelOpenButton.Text = this.SidePanel.Visible ? "Open Side" : "Close Side";
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
            int scaleX = this.panelWidth / this.game.boardState.getWidth();
            int scaleY = this.panelHeight / this.game.boardState.getHeight();
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
                //this.activeSidePanelButton = this.piecePlacing;
                this.activeSidePanelButton = 0;
            }
            this.game.setPiecePlacing(this.activeSidePanelButton);
        }

        /// <summary>
        /// Called by the button that means the user is done placing pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void donePlacingButton_click(object sender, EventArgs e)
        {
            //nextTurn();
            //this.piecePlacing.setTeamCode(this.game.turn);
            //if (turn == -1) for (int row = 6; row < boardState.GetLength(1); row++) fillRow(0, row);
            //if (turn == 1) for (int row = 4; row < 6; row++) fillRow(0, row);
            if (this.game.turn == StrategoGame.BLUE_TEAM_CODE)
                for (int i = 0; i < 4; i++)
                    for (int x = 0; x < 10; x++)
                        this.game.boardState.setPiece(x, i, null);
            //if (this.game.turn == -1)
            ((Button)sender).Enabled = false;
            this.game.nextTurn();
            //for (int x = 0; x < this.game.boardState.getWidth(); x++) this.game.boardState.getPiece(x, row] = value;
        }

        /// <summary>
        /// Called when the SinglePlayerButton in the main menu is pressed, throws the player into a new single player game against the AI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SinglePlayerButton_click(object sender, EventArgs e)
        {
            this.game.isSinglePlayer = true;
            StartButton_Click(sender, e);
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
                this.game.boardState = new Gameboard(this.game.boardState.getWidth(), this.game.boardState.getHeight());
                for (int row = 0; row < 6; row++) this.game.boardState.fillRow(null, row);
                this.game.turn = 0;
                this.game.preGameActive = true;
                this.game.resetPlacements();
                //this.game.ai = new AI_Old(this, -1);
                this.game.nextTurn();
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
            if (!this.game.preGameActive || (this.game.boardState.getWidth() != 10) || (this.game.boardState.getHeight() != 10) || (Math.Abs(this.game.turn) == 2)) return;

            SaveLoadOperations.saveSetup(getSetupData());
        }

        /// <summary>
        /// Handles what happens when the "load setup" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSetUpButton_Click(object sender, EventArgs e)
        {
            if (!this.game.preGameActive || (this.game.boardState.getWidth() != 10) || (this.game.boardState.getHeight() != 10) || (Math.Abs(this.game.turn) == 2))
                return;

            SetupData data = SaveLoadOperations.loadSetup();
            if (data == null)
                return;

            loadSetupData(data);

            this.backPanel.Invalidate();

            this.donePlacingButton.Enabled = this.game.isDonePlacing();

            return;
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
            this.game.nextTurn();
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
            if (Math.Abs(this.game.turn) == 1)
                gameOver(-1 * this.game.turn);
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
            //if (this.game.ai != null)
            //    this.game.ai.difficulty = Convert.ToInt32(this.AIDifficultyChanger.SelectedItem);
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
            this.OptionsPanel.Visible = false;
            if (this.level > -1)
            {
                if (winnerTeam == 1)
                {
                    if (this.level == this.levelImages.Length)
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
                    if (this.game.isSinglePlayer)
                        this.victoryLabel.Text = "YOU ARE VICTORIOUS, PLAYER 1.";
                    else
                        this.victoryLabel.Text = "BLUE PLAYER WINS.";
                }
                else
                {
                    if (this.game.isSinglePlayer)
                        this.victoryLabel.Text = "WOW, YOU LOST TO THAT? ...SERIOUSLY?";
                    else
                        this.victoryLabel.Text = "RED PLAYER WINS.";
                }
            }


            this.EndGamePanel.Visible = true;
            this.EndGamePanel.Enabled = true;
        }

        /// <summary>
        /// What to do if the movable bomb check box in the options menu is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void movableBombCB_CheckedChanged(object sender, EventArgs e)
        {
            this.game.movableBombs = !this.game.movableBombs;
        }

        /// <summary>
        /// What to do if the movable flag check box in the options menu is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void movableFlagCB_CheckedChanged(object sender, EventArgs e)
        {
            this.game.movableFlags = !this.game.movableFlags;
        }

        /// <summary>
        /// What to do when the konami code is entered
        /// </summary>
        private void konamiCodeEntered()
        {
            this.movableBombCB.Enabled = true;
            this.movableFlagCB.Enabled = true;
            this.game.skippableLevels = true;
        }

        /// <summary>
        /// Loads the next level of the campaign. If the game is not in campaign mode, it starts at the first level of the campaign
        /// </summary>
        private void loadNextLevel()
        {
            if (this.level < -1 && (this.level + 1) >= this.levelImages.Length) return;
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

            if (!loadSaveData(SaveLoadOperations.loadSaveData(path)))
                return;

            this.game.preGameActive = false;

            this.backPanel.BackgroundImage = this.levelImages[level - 1];
            if (this.game.turn == -2) this.NextTurnButton.Text = "Player 1's Turn";
            else this.NextTurnButton.Text = "AI's Turn";
            this.NextTurnButton.Visible = true;
            this.NextTurnButton.Enabled = true;
            this.backPanel.Invalidate();
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
            // if ((this.game.turn == 0) || (this.game.preGameActive) || (Math.Abs(this.game.turn) == 2)) return; // TODO: this line may be necessary?
            SaveLoadOperations.saveGame(getSaveData());
            //SaveLoadOperations.updateOldSavefile();
        }

        /// <summary>
        /// A trigger that activates when the LoadButton in the main manu is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!loadSaveData(SaveLoadOperations.loadGame()))
                return;

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
            if (this.game.turn == 2 && this.game.isSinglePlayer)
                this.NextTurnButton.Text = "AI's Turn";
            else if (this.game.turn == 2)
                this.NextTurnButton.Text = "Player 2's Turn";
            if (this.game.isSinglePlayer)
            {
                //this.AIDifficultyChanger.Text = this.game.ai.difficulty.ToString();
            }
            this.NextTurnButton.Visible = true;
            this.NextTurnButton.Enabled = true;
            this.game.preGameActive = false;

            foreach (var button in this.SidePanel.Controls.OfType<Button>())
                if (button.Name != donePlacingButton.Name && button.Name != saveSetUpButton.Name && button.Name != loadSetUpButton.Name)
                    button.Click += SidePanelButtonClick;

            backPanel.Focus();
        }

        private SaveData getSaveData()
        {
            return new SaveData(
                this.game.boardState,
                //this.game.ai.difficulty,
                0,
                this.game.turn,
                this.game.isSinglePlayer
                );
        }

        private bool loadSaveData(SaveData data)
        {
            if (data == null)
                return false;

            this.game.boardState = data.boardState;
            this.game.turn = data.turn;
            this.game.isSinglePlayer = data.isSinglePlayer;

            return true;

            //this.game.ai.difficulty = data.difficulty;

            //if (this.game.isSinglePlayer)
            //    this.game.ai = new AI_Old(this, -1, data.difficulty);
        }

        public SetupData getSetupData()
        {
            return new SetupData(
                this.game.boardState,
                this.game.getPlacements(),
                this.game.turn
                );
        }

        private void loadSetupData(SetupData data)
        {
            Gameboard setupBoard = data.boardState;
            if (this.game.turn == StrategoGame.RED_TEAM_CODE)
                setupBoard.flipBoard();

            this.game.boardState.overridePiecesOfTeam(setupBoard, this.game.turn);
           
            this.game.setPlacements(data.getPlacementsDictionary());
        }

        public void adjustTurnButtonState(string buttonText)
        {
            NextTurnButton.Text = buttonText;
            NextTurnButton.Visible = true;
            this.NextTurnButton.Enabled = true;
        }

        public void invalidateBackpanel()
        {
            this.backPanel.Invalidate();
        }

        public void setSidePanelVisibility(bool visible)
        {
            this.SidePanelOpenButton.Visible = visible;
            this.SidePanel.Visible = visible;
        }
    }
}
