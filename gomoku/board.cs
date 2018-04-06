using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace gomoku
{
    class board
    {
        private static readonly Point NOT_EXISTS_NODE = new Point(-1, -1);
        private static readonly int OFFSET = 75;
        private static readonly int NODE_DISTANCE = 75;
        private static readonly int NODE_RADIUS = 10;
        private Piece[,] pieces = new Piece [9,9];

        // 最後一手 ---------------------------------------------------------------------------
        private Point latestPiece = NOT_EXISTS_NODE;
        public Point LatestPiece { get { return latestPiece; } }

        public bool CanBePlaced(int x, int y)
        {
            // 找最鄰近的點 ---------------------------------------------------
            Point nodeID = FindTheClosestNode(x, y);

            // 找不到 ---------------------------------------------------------
            if (nodeID == NOT_EXISTS_NODE)
                return false;

            // 已經有棋子 -----------------------------------------------------
            if (pieces[nodeID.X, nodeID.Y] != null)
                return false;

            return true;
        }       

        // 得到棋子顏色 ------------------------------------------------------- 
        public PieceType GetPieceType(int x, int y)
        {           
            if (pieces[x, y] != null)
                return pieces[x, y].GetPieceType();
            else
                return PieceType.NONE;
        }
        public Piece PlaceAPiece(int x, int y, PieceType type)
        {
            // 找最鄰近的點 ---------------------------------------------------
            Point nodeID = FindTheClosestNode(x, y);

            // 找不到 ---------------------------------------------------------
            if (nodeID == NOT_EXISTS_NODE)
                return null;

            // 已經有棋子 -----------------------------------------------------
            if (pieces[nodeID.X, nodeID.Y] != null)
                return null;
            else
            {
                Point formNodeID = convertToFormPosition(nodeID);
                // 紀錄最近一顆 --------------------------------------------------------------
                latestPiece = nodeID;
                // 下子 ----------------------------------------------------------------------
                if (type == PieceType.Black)
                    pieces[nodeID.X, nodeID.Y] = new BlackPiece(formNodeID.X, formNodeID.Y);
                else if (type == PieceType.White)
                    pieces[nodeID.X, nodeID.Y] = new WhitePiece(formNodeID.X, formNodeID.Y);
            }
            return pieces[nodeID.X, nodeID.Y];
        }

        public Point convertToFormPosition(Point nodeID)
        {
            Point formPosition = new Point();
            formPosition.X = nodeID.X * NODE_DISTANCE + OFFSET;
            formPosition.Y = nodeID.Y * NODE_DISTANCE + OFFSET;
            return formPosition;
        }

        private Point FindTheClosestNode(int x, int y)
        {
            // 找X座標 ---------------------------------------------------------
            int indexX = FindTheClosestNode(x);
            if (indexX == -1)
                return NOT_EXISTS_NODE;

            // 找Y座標 ---------------------------------------------------------
            int indexY = FindTheClosestNode(y);
            if (indexY == -1)
                return NOT_EXISTS_NODE;

            return new Point(indexX, indexY);            
        }
        private int FindTheClosestNode(int z)
        {
            // 小於邊界 --------------------------------------------
            if (z < OFFSET - NODE_RADIUS)
                return -1;
            // 大於邊界 --------------------------------------------
            if (z > OFFSET + NODE_DISTANCE * 8 + NODE_RADIUS)
                return -1;
            // 位移 ------------------------------------------------
            z -= OFFSET;
            int index = z / NODE_DISTANCE;
            int distance = z % NODE_DISTANCE;
            // 鄰近於左節點 ----------------------------------------
            if (distance <= NODE_RADIUS)
            {
                return index;
            }
            // 鄰近於右節點 ----------------------------------------
            if ((NODE_DISTANCE - distance) < NODE_RADIUS)
            {
                return index + 1;
            }
            // 都沒有 ----------------------------------------------
            return -1;
        }        
    }
}
