using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrategoTest
{
    class DummyStrategoWin : Stratego.StrategoWin
    {
        public DummyStrategoWin()
        {
        }

        public void toggleSidePanelVisiblity()
        {
            this.toggleSidePanelOpen();
        }

        public string getButtonText()
        {
            return this.getSidePanelButtonText();
        }

        public bool sidePanelVisible()
        {
            return this.isSidePanelVisible();
        }
    }
}
