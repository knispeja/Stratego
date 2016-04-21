using Stratego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategoTest
{
    class TestCallback : GUICallback
    {
        public void adjustTurnButtonState(string buttonText)
        {
            return;
        }

        public void gameOver(int teamCode)
        {
            return;
        }

        public void invalidateBackpanel()
        {
            return;
        }

        public void setSidePanelVisibility(bool visible)
        {
            return;
        }

        public void onNextTurnButtonClick()
        {
            return;
        }
    }
}
