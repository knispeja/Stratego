using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategoTest
{
    class GUITest
    {
        [TestFixture()]
        class SaveLoadTests
        {
            [TestCase()]
            public void TestToggleSidePanel()
            {
                DummyStrategoWin dummy = new DummyStrategoWin();

                Assert.AreEqual(Stratego.StrategoWin.OPEN_SIDE_PANEL_TEXT, dummy.getButtonText());

                dummy.toggleSidePanelVisiblity();

                Assert.AreEqual(Stratego.StrategoWin.CLOSE_SIDE_PANEL_TEXT, dummy.getButtonText());
            }
        }
    }
}
