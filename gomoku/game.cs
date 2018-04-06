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
        private board m_board = new board();
        // 現在玩家 -------------------------------------------------------------------------
        public PieceType currentPlayer = PieceType.Black;
        // 電腦 -----------------------------------------------------------------------------
        public PieceType CommputerPlayer = PieceType.White;
        // 棋盤的長/寬 ----------------------------------------------------------------------
        private int numberOfNode = 8;
        // 贏家 -----------------------------------------------------------------------------
        private PieceType winner = PieceType.NONE;
        public PieceType Winner { get { return winner; }}
        // 得分 -----------------------------------------------------------------------------
        private PieceScore[,] m_pieceScore = new PieceScore[9,9];

        public Piece ComputerRun()
        {
            if (currentPlayer == CommputerPlayer)
            {
                // 尋找最佳解 ---------------------------------------------------------------
                BestPiece m_bestPiece = searchTheBestPiece();
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
                // 計算分數 ---------------------------------------------------------
                CheckWeight(m_board.LatestPiece.X, m_board.LatestPiece.Y);                
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

        public void CheckWeight(int x, int y)
        {
            int index = 0;
            int[] scores = new int[8];
            for (int yDir = -1; yDir <= 1; yDir++)
            {
                for (int xDir = -1; xDir <= 1; xDir++)
                {
                    if (xDir == 0 && yDir == 0)
                        continue;
                    int count = 0;
                    int targetX = x + xDir;
                    int targetY = y + yDir;
                    while(count < 4)
                    {
                        // 邊界檢查 ---------------------------------------------------------
                        if (targetX > numberOfNode || targetY > numberOfNode ||
                            targetX < 0 || targetY < 0)                        
                            break;
                        // 是否同顏色 -------------------------------------------------------
                        if (m_board.GetPieceType(targetX, targetY) != currentPlayer)
                            break;
                        targetX += xDir;
                        targetY += yDir;
                        count++;
                    }
                    // 每個方位的Score ------------------------------------------------------
                    scores[index] = count;
                    index++;
                }
            }
            // 局面分數存檔 -----------------------------------------------------------------
            SetPieceScore(x,y,scores);
        }

        public BestPiece searchTheBestPiece()
        {
            BestPiece m_bestPiece = new BestPiece();
            for (int i=0;i<numberOfNode+1;i++)
            {
                for (int j=0;j<numberOfNode+1;j++)
                {
                    // 如果已經有棋子
                    if (m_board.GetPieceType(i, j) != PieceType.NONE)
                        continue;
                    CheckWeight(i, j);
                    int tmp_weight = GetTargetWeight(i, j);
                    if (m_bestPiece.weight <= tmp_weight)
                    {
                        m_bestPiece.x = i;
                        m_bestPiece.y = j;
                        m_bestPiece.weight = tmp_weight;
                    }
                }
            }
            return m_bestPiece;
        }

        public void SetPieceScore(int x, int y, int[] scores)
        {
            PieceScore _pieceScore = new PieceScore();
            _pieceScore.Scores = scores;
            m_pieceScore[x, y] = _pieceScore;
        }

        public int[] GetPieceScore()
        {
            return m_pieceScore[m_board.LatestPiece.X, m_board.LatestPiece.Y].Scores;
        }
        public int GetPieceWeight()
        {
            return m_pieceScore[m_board.LatestPiece.X, m_board.LatestPiece.Y].GetPieceWeight();
        }

        private int GetTargetWeight(int x, int y)
        {
            return m_pieceScore[x, y].GetPieceWeight();
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
