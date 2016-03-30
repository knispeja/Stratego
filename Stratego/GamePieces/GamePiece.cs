using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stratego
{
    public abstract class GamePiece
    {
        protected Dictionary<int, Color> colorDict;

        protected Dictionary<int, Image> imageDict;
        protected string pieceName;
        protected int pieceRank;
        protected int teamCode;
        protected Boolean lifeStatus;
        protected Color pieceColor;
        protected Image pieceImage;

        protected BattleBehavior attackBehavior;
        protected BattleBehavior defendBehavior;

        protected Boolean essential;

        protected int limitToMovement;

        protected Boolean movable;

        private int xVal;
        private int yVal;

        public GamePiece(int teamCode)
        {
            colorDict = new Dictionary<int, Color>();
            imageDict = new Dictionary<int, Image>();

            colorDict.Add(StrategoWin.BLUE_TEAM_CODE, Color.FromArgb(25, 25, 175));
            colorDict.Add(StrategoWin.RED_TEAM_CODE, Color.FromArgb(175, 25, 25));

            this.pieceRank = 42;
            this.pieceName = "null";
            this.teamCode = teamCode;
            this.lifeStatus = true;
            this.pieceImage = null;
            this.pieceColor = colorDict[teamCode];
            this.attackBehavior = new DefaultComparativeFate();
            this.defendBehavior = new DefaultComparativeFate();

            this.limitToMovement = 1;

            this.movable = true;
            this.xVal = -1;
            this.yVal = -1;

            this.essential = false;
        }

        public Boolean isEssential()
        {
            return this.essential;
        }

        public void attack(GamePiece otherPiece)
        {
            if(this.attackBehavior.decideFate(this, otherPiece))
            {
                this.killPiece();
            }
        }

        public void defend(GamePiece otherPiece)
        {
            if(this.defendBehavior.decideFate(this, otherPiece))
            {
                this.killPiece();
            }
        }

        public int compareRanks(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            if (otherRank == this.pieceRank)
            {
                return 0;   
            }
            else if (otherRank > this.pieceRank)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public Boolean isAlive()
        {
            return lifeStatus;
        }

        protected void killPiece()
        {
            this.lifeStatus = false;
        }

        public int getPieceRank()
        {
            return pieceRank;
        }

        public String getPieceName()
        {
            return pieceName;
        }

        public int getTeamCode()
        {
            return teamCode;
        }

        public Image getPieceImage()
        {
            return this.pieceImage;
        }

        public Color getPieceColor()
        {
            return this.pieceColor;
        }

        public int getLimitToMovement()
        {
            return limitToMovement;
        }

        public Boolean isMovable()
        {
            return this.movable;
        }

        public void setMovable(Boolean movability)
        {
            this.movable = movability;
        }

        public int getXVal()
        {
            return this.xVal;
        }

        public int getYVal()
        {
            return this.yVal;
        }

        public void setXVal(int newX)
        {
            this.xVal = newX;
        }

        public void setYVal(int newY)
        {
            this.yVal = newY;
        }
    }
}
