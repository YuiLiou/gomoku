using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace gomoku
{
    class game
    {
        // 盤面 -----------------------------------------------------------------------------
        private board m_board = new board();
        // 現在玩家 -------------------------------------------------------------------------
        public PieceType currentPlayer = PieceType.Black;
        // 電腦 -----------------------------------------------------------------------------
        public PieceType ComputerPlayer = PieceType.White;
        // 棋盤的長/寬 ----------------------------------------------------------------------
        private static readonly int numberOfNode = 8;
        // 贏家 -----------------------------------------------------------------------------
        private PieceType winner = PieceType.NONE;
        public PieceType Winner { get { return winner; }}
        // 得分 -----------------------------------------------------------------------------
        private PieceScore m_pieceScore = new PieceScore();

        public Piece ComputerRun()
        {
            if (currentPlayer == ComputerPlayer)
            {
                // 尋找最佳解 ---------------------------------------------------------------
                Step m_bestPiece = searchTheBestPiece();
                Point nodeID = new Point(m_bestPiece.x, m_bestPiece.y);
                Point form_node = m_board.convertToFormPosition(nodeID);
                return PlaceAPiece(form_node.X, form_node.Y);
            }
            return null;
        }

        // 放置棋子 -------------------------------------------------------------------------
        public Piece PlaceAPiece(int x, int y)
        {
            Piece piece = m_board.PlaceAPiece(x, y, currentPlayer);
            if (piece != null)
            {              
                // 確認贏家 ---------------------------------------------------------
                CheckWinner(m_board.LatestPiece.X, m_board.LatestPiece.Y);
                // 交換選手 ---------------------------------------------------------
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

        public Step searchTheBestPiece()
        {
            Step m_bestPiece = new Step();
            m_bestPiece.weight = -10000;
            for (int i=0;i<numberOfNode+1;i++)
            {
                for (int j=0;j<numberOfNode+1;j++)
                {
                    if (m_board.GetPieceType(i, j) == PieceType.NONE)
                    {
                        // 電腦下棋 ----------------------------------------------------------------------------------
                        m_board.PiecePlace[i, j] = ComputerPlayer;
                        int tmp_weight = m_pieceScore.minimax(i, j, ComputerPlayer, 2, m_board.PiecePlace, false);
                        m_board.PiecePlace[i, j] = PieceType.NONE;
                        // 電腦下棋 ----------------------------------------------------------------------------------
                        if (m_bestPiece.weight < tmp_weight)
                        {
                            m_bestPiece.x = i;
                            m_bestPiece.y = j;
                            m_bestPiece.weight = tmp_weight;
                            //MessageBox.Show("weight:" + i + " " + j + " " + tmp_weight);
                        }
                    }
                }
            }
            return m_bestPiece;
        }           
        
        public void CheckWinner(int x, int y)
        {
            for (int xDir = -1; xDir <= 1; xDir++)
            {
                for (int yDir = -1; yDir <= 1; yDir++)
                {
                    if (xDir == 0 && yDir == 0)
                        continue;
                    int count = 0;
                    int targetX = x + xDir;
                    int targetY = y + yDir;
                    while (count < 4)
                    {                
                        // 邊界檢查
                        if (targetX > numberOfNode || targetY > numberOfNode ||
                            targetX < 0 || targetY < 0)
                            break;
                        // 檢查是否同顏色 --------------------------------------------------------
                        if (m_board.GetPieceType(targetX, targetY) != currentPlayer)
                            break;
                        // 檢查下一顆 ------------------------------------------------------------
                        targetX += xDir;
                        targetY += yDir;
                        count++;
                    }
                    if (count == 4)
                        winner = currentPlayer;
                }
            }
        }
    }
}
