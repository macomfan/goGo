using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoPoint
    {
        private GoCoord coord_ = null;
        private GoPointType type_ = GoPointType.EMPTY;
        private GoMarkType mark_ = GoMarkType.NONE;
        private GoBlock block_ = null;
        private GoPointVisitor vistior_ = null;
        private byte status_ = 0;

        public GoCoord Coord
        {
            get { return coord_; }
        }

        public GoPoint UP
        {
            get { return vistior_.GetPointByDirection(this, GoPointVisitor.Direction.UP); }
        }

        public GoPoint DOWN
        {
            get { return vistior_.GetPointByDirection(this, GoPointVisitor.Direction.DOWN); }
        }

        public GoPoint LEFT
        {
            get { return vistior_.GetPointByDirection(this, GoPointVisitor.Direction.LEFT); }
        }

        public GoPoint RIGHT
        {
            get { return vistior_.GetPointByDirection(this, GoPointVisitor.Direction.RIGHT); }
        }

        public GoPoint(GoCoord coord, GoPointVisitor vistior)
        {
            coord_ = coord;
            vistior_ = vistior;
            type_ = GoPointType.EMPTY;
        }

        public GoPointType Type
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
                if (type_ == GoPointType.EMPTY)
                {
                    return -1;
                }
                if (block_ == null)
                {
                    throw new Exception("ERROR: Block is NULL for a Point");
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
