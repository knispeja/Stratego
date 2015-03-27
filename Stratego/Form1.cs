using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Stratego
{
    public partial class StrategoWin : Form
    {
        ArrayList Piece { get; set; }
        public StrategoWin()
        {
            InitializeComponent();
        }
    }
}
