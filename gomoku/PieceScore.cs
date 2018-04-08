using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gomoku
{
    class PieceScore
    {
        private int[] _scores = new int[8];
        private static readonly int numberOfDirs = 8;
        private static readonly PieceType ComputerPlayer = PieceType.White;
        private static readonly PieceType HumanPlayer = PieceType.Black;
        // 評估函式 -------------------------------------------------------------------------
        public int GetPieceWeight(int x, int y, PieceType[,] piecePlace, PieceType currentPlayer)
        {
            int weight = 0;
            // 水平方向 檢查[-4,4] ---------------------------------------------------            
            for (int i = -4; i <= 0; i++)
            {
                int continuous = 0;
                for (int j = 0; j < 5; j++)
                {
                    int targetX = x + i + j;
                    int targetY = y;
                    if (targetX > numberOfDirs || targetX < 0)
                        continue;
                    if (targetX == x && targetY == y)
                        continue;
                    if (piecePlace[targetX, targetY] == currentPlayer)
                    {
                        continuous++;
                    }
                    else if (piecePlace[targetX, targetY] != PieceType.NONE && piecePlace[targetX, targetY] != currentPlayer)
                    {
                        continuous = 0;
                    }
                }
                weight += continuous * continuous;
            }
            // 垂直方向 檢查[-4,4] ---------------------------------------------------            
            for (int i = -4; i <= 0; i++)
            {
                int continuous = 0;
                for (int j = 0; j < 5; j++)
                {
                    int targetX = x;
                    int targetY = y + i + j;
                    if (targetY > numberOfDirs || targetY < 0)
                        continue;
                    if (targetX == x && targetY == y)
                        continue;
                    if (piecePlace[targetX, targetY] == currentPlayer)
                    {
                        continuous++;
                    }
                    else if (piecePlace[targetX, targetY] != PieceType.NONE && piecePlace[targetX, targetY] != currentPlayer)
                    {
                        continuous = 0;
                    }
                }
                weight += continuous * continuous;
            }
            // 左上到右下 檢查[-4,4] ---------------------------------------------------            
            for (int i = -4; i <= 0; i++)
            {
                int continuous = 0;
                for (int j = 0; j < 5; j++)
                {
                    int targetX = x + i + j;
                    int targetY = y + i + j;
                    if (targetX > numberOfDirs || targetY > numberOfDirs || targetX < 0 || targetY < 0)
                        continue;
                    if (targetX == x && targetY == y)
                        continue;
                    if (piecePlace[targetX, targetY] == currentPlayer)
                    {
                        continuous++;
                    }
                    else if (piecePlace[targetX, targetY] != PieceType.NONE && piecePlace[targetX, targetY] != currentPlayer)
                    {
                        continuous = 0;
                    }
                }
                weight += continuous * continuous;
            }
            // 左下到右上 檢查[-4,4] ---------------------------------------------------            
            for (int i = -4; i <= 0; i++)
            {
                int continuous = 0;
                for (int j = 0; j < 5; j++)
                {
                    int targetX = x + i + j;
                    int targetY = y - i - j;
                    if (targetX > numberOfDirs || targetY > numberOfDirs || targetX < 0 || targetY < 0)
                        continue;
                    if (targetX == x && targetY == y)
                        continue;
                    if (piecePlace[targetX, targetY] == currentPlayer)
                    {
                        continuous++;
                    }
                    else if (piecePlace[targetX, targetY] != PieceType.NONE && piecePlace[targetX, targetY] != currentPlayer)
                    {
                        continuous = 0;
                    }
                }
                weight += continuous * continuous;
            }
            if (currentPlayer == HumanPlayer)
                weight *= -1;
            return weight;
        }

        // 是否剩下一步 ---------------------------------------------------------------------
        private bool theLastStep(PieceType [,] PiecePlace)
        {
            bool finished = true;
            for (int i = 0; i < numberOfDirs + 1; i++)
            {
                for (int j = 0; j < numberOfDirs + 1; j++)
                {
                    if (PiecePlace[i, j] != PieceType.NONE)
                    {
                        finished = false;
                        break;
                    }
                }
            }
            return finished;
        }

        public int minimax(int x, int y, PieceType currentPlayer, int depth, PieceType[,] PiecePlace, bool bMax)
        {
            // 此步得分 --------------------------------------------------------
            int CurrentPoint = GetPieceWeight(x, y, PiecePlace, currentPlayer);
            // 深度為0 或 剩下一步 ---------------------------------------------
            if (depth == 0 || theLastStep(PiecePlace))
            {
                return CurrentPoint;
            }
            // maximizing player -----------------------------------------------
            if (bMax)
            {
                int max_value = -10000;
                int tmp_value = 0;
                for (int i = 0; i < numberOfDirs + 1; i++)
                {
                    for (int j = 0;j < numberOfDirs+1; j++)
                    {
                        if (PiecePlace[i,j] == PieceType.NONE)
                        {
                            // 電腦下棋 ---------------------------------------------------------------------
                            PiecePlace[i, j] = ComputerPlayer;
                            tmp_value = minimax(i, j, ComputerPlayer, depth - 1, PiecePlace, false);
                            PiecePlace[i, j] = PieceType.NONE;
                            // 電腦下棋 ---------------------------------------------------------------------
                            if (tmp_value > max_value)
                            {
                                max_value = tmp_value;
                            }
                        }
                    }
                }
                return CurrentPoint + max_value;
            }
            // minimizing player -------------------------------------------------
            else if (!bMax)
            {
                int min_value = 10000;
                int tmp_value = 0;
                for (int i = 0; i < numberOfDirs + 1; i++)
                {
                    for (int j = 0; j < numberOfDirs + 1; j++)
                    {
                        if (PiecePlace[i, j] == PieceType.NONE)
                        {
                            // 人類下棋 ---------------------------------------------------------------------
                            PiecePlace[i, j] = HumanPlayer;
                            tmp_value = minimax(i, j, HumanPlayer, depth - 1, PiecePlace, true);
                            PiecePlace[i, j] = PieceType.NONE;
                            // 人類下棋 ---------------------------------------------------------------------
                            if (tmp_value < min_value)
                            {
                                min_value = tmp_value;
                            }
                        }
                    }
                }
                return CurrentPoint + min_value;
            }
            return -1;
        }
    }
}
