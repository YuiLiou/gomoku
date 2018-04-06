using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace gomoku 
{
    abstract class Piece:PictureBox
    {
        private static readonly int image_width = 50;
        public Piece (int x, int y)
        {
            this.BackColor = Color.Transparent;
            this.Location = new Point(x - image_width / 2, y- image_width / 2);
            this.Size = new Size(image_width, image_width);
        }
        public abstract PieceType GetPieceType();
    }
}
