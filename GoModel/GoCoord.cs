using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoCoord
    {
        private int row_;
        private int col_;

        public GoCoord(int row, int col)
        {
            row_ = row;
            col_ = col;
        }

        public int Row
        {
            get { return row_; }
        }

        public int Col
        {
            get { return col_; }
        }

        public int GetIndex(int size)
        {
            return row_ * size + col_;
        }

        public bool IsVaild(int maxSize)
        {
            if (row_ < 0 || row_ >= maxSize || col_ < 0 || col_ >= maxSize)
            {
                return false;
            }
            return true;
        }
    }
}
