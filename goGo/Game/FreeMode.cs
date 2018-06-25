using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using goGo.DataEngine;
using goGo.Painter;

namespace goGo.Game
{
    public class FreeMode : IViewController
    {
        ChessBoard cb_ = null;

        public void BindChessBoard(ChessBoard cb)
        {
            cb_ = cb;
        }

        public void OnSelectDian(GoCoord coord)
        {

        }
        public void OnClickDian(GoCoord coord)
        {
            //layout_.SetDian(coord.Row, coord.Col, GoDianType.BLACK);
        }

        public bool OnMouseClick(MouseEventArgs e)
        {
            if (cb_.Layout == null)
            {
                return false;
            }
            GoCoord coord = cb_.hitTest(e.X, e.Y, 10);
            if (coord != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    cb_.Layout.SetDian(coord.Row, coord.Col, GoDianType.BLACK);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    cb_.Layout.SetDian(coord.Row, coord.Col, GoDianType.WHITE);
                }
            }
            return true;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            return false;
        }
    }
}
