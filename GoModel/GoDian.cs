using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoDian
    {
        private GoCoord coord_ = null;
        private GoDianType type_ = GoDianType.EMPTY;
        private GoMarkType mark_ = GoMarkType.NONE;
        private GoLayout parentLayout_ = null;
        private GoBlock block_ = null;
        private GoDianVisitor vistior_ = null;
        private byte status_ = 0;

        public GoCoord Coord
        {
            get { return coord_; }
        }

        public GoDian UP
        {
            get { return vistior_.GetDianByDirection(this, GoDianVisitor.Direction.UP); }
        }

        public GoDian DOWN
        {
            get { return vistior_.GetDianByDirection(this, GoDianVisitor.Direction.DOWN); }
        }

        public GoDian LEFT
        {
            get { return vistior_.GetDianByDirection(this, GoDianVisitor.Direction.LEFT); }
        }

        public GoDian RIGHT
        {
            get { return vistior_.GetDianByDirection(this, GoDianVisitor.Direction.RIGHT); }
        }

        public GoDian(GoCoord coord, GoDianVisitor vistior)
        {
            coord_ = coord;
            vistior_ = vistior;
            type_ = GoDianType.EMPTY;
        }

        public GoDianType Type
        {
            get { return type_; }
            set { type_ = value; }
        }

        public GoMarkType Make
        {
            get { return mark_; }
            set { mark_ = value; }
        }

        public GoBlock Block
        {
            get { return block_; }
            set
            {
                //TODO
                block_ = value;
            }
        }

        public void ResetStatus()
        {
            status_ = 0;
        }

        public int Qi
        {
            get
            {
                if (type_ == GoDianType.EMPTY)
                {
                    return -1;
                }
                if (block_ == null)
                {
                    throw new Exception("ERROR: Block is NULL for a Dian");
                }
                return block_.Qi;
            }
        }

        public void SetStatusAsVisited()
        {
            status_ = 1;
        }

        public bool IsVisited()
        {
            if (status_ == 1)
            {
                return true;
            }
            return false;
        }
    }
}
