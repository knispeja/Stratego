using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratego
{
    public interface GUICallback
    {
        void adjustTurnButtonState(String buttonText);
        void invalidateBackpanel();
        void gameOver(int teamCode);
        void setSidePanelVisibility(Boolean visible);
    }
}
