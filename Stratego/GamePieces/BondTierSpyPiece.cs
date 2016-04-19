using Stratego.BattleBehaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego.GamePieces
{
    [Serializable]
    public class BondTierSpyPiece : GamePiece
    {
        public static readonly String BOND_NAME = "BondJamesBond";
        public static readonly int BOND_RANK = int.MaxValue;

        public BondTierSpyPiece(int teamCode) : base(teamCode)
        {
            this.pieceRank = BOND_RANK;
            this.pieceName = BOND_NAME;
            this.attackBehavior = new DiesToBondAndMarshall();
            this.defendBehavior = new DiesToBondAndMarshall();
            this.limitToMovement = 2;
            this.movable = false;
        }

        public override Image getPieceImage()
        {
            return this.teamCode == StrategoGame.BLUE_TEAM_CODE ? Properties.Resources.BlueBond : Properties.Resources.RedBond;
        }
    }
}
