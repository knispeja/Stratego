using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stratego
{
    [Serializable]
    public abstract class GamePiece
    {
        [NonSerialized]
        public static readonly string NULL_PIECE_NAME = "null_piece";

        [NonSerialized]
        public static readonly Color BLUE_TEAM_COLOR = Color.FromArgb(25, 25, 175);

        [NonSerialized]
        public static readonly Color RED_TEAM_COLOR = Color.FromArgb(175, 25, 25);

        protected string pieceName;
        protected int pieceRank;
        protected int teamCode;
        protected bool lifeStatus;
        protected Color pieceColor;

        protected BattleBehavior attackBehavior;
        protected BattleBehavior defendBehavior;

        protected bool essential;

        protected int limitToMovement;

        protected bool movable;

        private int xVal;
        private int yVal;

        public GamePiece(int teamCode)
        {
            this.pieceRank = 42;
            this.pieceName = "null";
            this.teamCode = teamCode;
            this.lifeStatus = true;
            setPieceColorToMatchTeam();
            this.attackBehavior = new DefaultComparativeFate();
            this.defendBehavior = new DefaultComparativeFate();

            this.limitToMovement = 1;

            this.movable = true;
            this.xVal = -1;
            this.yVal = -1;

            this.essential = false;
        }

        public abstract Image getPieceImage();

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

        public void setTeamCode(int teamCode)
        {
            this.teamCode = teamCode;
            setPieceColorToMatchTeam();
        }

        public Color getPieceColor()
        {
            return this.pieceColor;
        }

        private void setPieceColorToMatchTeam()
        {
            this.pieceColor = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? BLUE_TEAM_COLOR : RED_TEAM_COLOR;
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

        public void setAttackBehavior(BattleBehavior newBB)
        {
            this.attackBehavior = newBB;
        }

        public void setDefendBehavior(BattleBehavior newBB)
        {
            this.defendBehavior = newBB;
        }
    }
}
