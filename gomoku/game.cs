using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gomoku
{
    class game
    {
        private board m_board = new board();
        PieceType currentPlayer = PieceType.Black;
        // 棋盤的長/寬 ----------------------------------------------------------------------
        private int numberOfNode = 8;
        // 贏家 -----------------------------------------------------------------------------
        private PieceType winner = PieceType.NONE;
        public PieceType Winner { get { return winner; }}
        // 放置棋子 -------------------------------------------------------------------------
        public Piece PlaceAPiece(int x, int y)
        {
            Piece piece = m_board.PlaceAPiece(x, y, currentPlayer);
            if (piece != null)
            {
                // 確認贏家 ---------------------------------------------------------
                CheckWinner(m_board.LatestPiece.X, m_board.LatestPiece.Y);
                // 下一輪 -----------------------------------------------------------
                if (currentPlayer == PieceType.Black)
                {
                    currentPlayer = PieceType.White;
                }
                else if (currentPlayer == PieceType.White)
                {
                    currentPlayer = PieceType.Black;
                }
            }
            return piece;
        }
        public bool CanBePlaced(int x, int y)
        {
            return m_board.CanBePlaced(x, y);
        }

        public void CheckWinner(int x, int y)
        {
            for (int xDir = -1; xDir <= 1; xDir++)
            {
                for (int yDir = -1; yDir <= 1; yDir++)
                {
                    if (xDir == 0 && yDir == 0)
                        continue;
                    int count = 1;
                    int targetX = x + xDir;
                    int targetY = y + yDir;
                    while (count < 5)
                    {                        
                        if (targetX > numberOfNode)
                            break;
                        // 檢查是否同顏色 --------------------------------------------------------
                        if (m_board.GetPieceType(targetX, targetY) != currentPlayer)
                            break;
                        // 檢查下一顆 ------------------------------------------------------------
                        targetX += xDir;
                        targetY += yDir;
                        count++;
                    }
                    if (count == 5)
                        winner = currentPlayer;
                }
            }
        }
    }
}
