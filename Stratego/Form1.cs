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
using System.Media;

namespace Stratego
{
    public partial class StrategoWin : Form
    {
        ArrayList Piece { get; set; }
        public StrategoWin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SoundPlayer sound = new SoundPlayer(Properties.Resources.no);
            sound.Play();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
