﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Stratego
{
    abstract class GamePiece
    {
        public static readonly Dictionary<int, Color> colorDict = new Dictionary<int, Color>();

        protected Dictionary<int, Image> imageDict = new Dictionary<int, Image>();
        protected string pieceName;
        protected int pieceRank;
        protected int teamCode;
        protected Boolean lifeStatus;
        protected Color pieceColor;
        protected Image pieceImage;

        public GamePiece(int teamCode)
        {
            colorDict.Add(0, Color.FromArgb(25, 25, 175));
            colorDict.Add(1, Color.FromArgb(175, 25, 25));

            this.pieceRank = 42;
            this.pieceName = "null";
            this.teamCode = teamCode;
            this.lifeStatus = true;
            this.pieceImage = null;
            this.pieceColor = colorDict[teamCode];
        }

        public virtual void attack(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            int comparisonValue = this.compareRanks(otherRank);
            if (otherRank != SpyPiece.SPY_RANK && (comparisonValue <= 0 
                || otherRank == BombPiece.BOMB_RANK))
            {
                this.killPiece();
            }
        }

        public virtual void defend(GamePiece otherPiece)
        {
            int otherRank = otherPiece.getPieceRank();
            int comparisonValue = this.compareRanks(otherRank);
            if (comparisonValue <= 0 || otherRank == BombPiece.BOMB_RANK 
                || otherRank == SpyPiece.SPY_RANK)
            {
                this.killPiece();
            }
        }

        public virtual int compareRanks(int otherRank)
        {
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
    }
}
