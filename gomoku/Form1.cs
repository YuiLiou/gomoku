﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gomoku
{
    public partial class Form1 : Form
    {

        game m_game = new game();
        public Form1()
        {            
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Piece piece = m_game.PlaceAPiece(e.X, e.Y);
            if (piece != null)
            {
                this.Controls.Add(piece);
                // 判定勝負 -------------------------------------------------------                
                if (m_game.Winner == PieceType.Black)
                {
                    MessageBox.Show("Black Win!");
                }                 
            }
            Piece c_piece = m_game.ComputerRun();
            if (c_piece != null)
            {
                this.Controls.Add(c_piece);
                // 判定勝負 -------------------------------------------------------                
                if (m_game.Winner == PieceType.White)
                {
                    MessageBox.Show("White Win!");
                }                    
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            bool bCanBePlace =  m_game.CanBePlaced(e.X, e.Y);
            if (bCanBePlace)
            {
                this.Cursor = Cursors.Hand;
            }
            else if (!bCanBePlace)
            {
                this.Cursor = Cursors.Default;
            }
        }       
    }
}
