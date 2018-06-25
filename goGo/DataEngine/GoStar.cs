using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace goGo.DataEngine
{
    public class GoStar
    {
        private int row_ = 0;
        private int col_ = 0;

        public int Row
        {
            get { return row_; }
        }

        public int Col
        {
            get { return col_; }
        }

        public GoStar(int row, int col)
        {
            row_ = row;
            col_ = col;
        }
    }
}
