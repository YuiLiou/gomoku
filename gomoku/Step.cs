using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku
{
    class Step
    {
        public int x { get; set; }
        public int y { get; set; }
        public int weight { get; set; }
        public PieceType color { get; set; }
        public Step()
        {
            x = 0;
            y = 0;
            weight = 0;
            color = PieceType.NONE;
        }
    }
}
