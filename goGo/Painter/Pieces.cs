using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using goGo.DataEngine;

namespace goGo.Painter
{
    public class Pieces
    {
        public static Bitmap black_ = new Bitmap(128, 128);
        public static Bitmap white_ = new Bitmap(128, 128);
        private int size_ = 10;
        private GoCoord coord_ = null;

        private GoDianType type_;

        static Pieces()
        {
            Graphics gblack = Graphics.FromImage(black_);
            gblack.FillEllipse(new SolidBrush(Color.Black), 2, 2, 126, 126);
            Graphics gwhite = Graphics.FromImage(white_);
            gwhite.FillEllipse(new SolidBrush(Color.White), 2, 2, 126, 126);
            Pen whitePen = new Pen(new SolidBrush(Color.Black));
            whitePen.Width = 10.0f;
            gwhite.DrawEllipse(whitePen, 5, 5, 120, 120);
        }


        public GoCoord Coord
        {
            get { return coord_; }
        }

        public Pieces(int row, int col, GoDianType type)
        {
            coord_ = new GoCoord(row, col);
            type_ = type;
        }
    }
}
