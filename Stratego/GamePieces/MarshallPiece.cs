﻿using System;

namespace Stratego
{
    [Serializable]
    public class MarshallPiece : GamePiece
    {
        public static readonly String MARSHALL_NAME = "Marshall";
        public static readonly int MARSHALL_RANK = 10;

        public MarshallPiece (int teamCode) : base (teamCode)
        {
            this.pieceImage = (this.teamCode == StrategoGame.BLUE_TEAM_CODE) ? Properties.Resources.BlueMarshall : Properties.Resources.RedMarshall;

            this.pieceRank = MARSHALL_RANK;
            this.pieceName = MARSHALL_NAME;
            this.defendBehavior = new DiesToSpy();
        }
    }
}
