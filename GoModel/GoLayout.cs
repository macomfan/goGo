using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoLayout
    {
        public delegate void DianChangedHandler();
        public event DianChangedHandler DianChanged;

        public static int SIZE = 19;
        public static int STAR_NUM = 9;

        private GoDian[] layout_ = new GoDian[SIZE * SIZE];
        private static GoStar[] stars_ = new GoStar[STAR_NUM];
        private GoDianVisitor visitor_ = null;
        private bool autoTake_ = true;
        private bool allowChangeZi_ = false;

        private Stack<GoStep> steps_ = new Stack<GoStep>();

        static GoLayout()
        {
            stars_[0] = new GoStar(3, 3);
            stars_[1] = new GoStar(3, 15);
            stars_[2] = new GoStar(15, 3);
            stars_[3] = new GoStar(15, 15);
            stars_[4] = new GoStar(9, 9);
            stars_[5] = new GoStar(9, 3);
            stars_[6] = new GoStar(3, 9);
            stars_[7] = new GoStar(9, 15);
            stars_[8] = new GoStar(15, 9);
        }

        public int Size
        {
            get { return SIZE; }
        }

        public bool AutoTake
        {
            get { return autoTake_; }
            set { autoTake_ = value; }
        }

        public bool AllowChangeZi
        {
            get { return allowChangeZi_; }
            set { allowChangeZi_ = value; }
        }

        public void ResetAllStatus()
        {
            for (int i = 0; i < SIZE * SIZE; i++)
            {
                layout_[i].ResetStatus();
            }
        }

        public GoLayout()
        {
            visitor_ = new GoDianVisitor(this);
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    GoDian dian = new GoDian(new GoCoord(i, j), visitor_);
                    layout_[i * SIZE + j] = dian;
                }
            }
        }

        public void PushStep(GoStep step)
        {
            steps_.Push(step);
            SetDian(step.Coord, step.Type);
        }

        public bool PopStep()
        {
            GoStep step = steps_.Pop();
            SetDian(step.Coord, GoDianType.EMPTY);
            return false;
        }

        public static GoStar[] GetStars()
        {
            return stars_;
        }

        public GoDian GetDian(int row, int col)
        {
            return GetDian(new GoCoord(row, col));
        }

        public GoDian GetDian(GoCoord coord)
        {
            if (!coord.IsVaild(SIZE))
            {
                return null;
            }
            return layout_[coord.GetIndex(SIZE)];
        }

        private bool CheckTiZi(GoDian dian)
        {
            bool TiZi = false;
            if (dian.UP != null && dian.UP.Qi == 0)
            {
                TiZi = true;
                RemoveBlock(dian.UP.Block);
            }
            if (dian.DOWN != null && dian.DOWN.Qi == 0)
            {
                TiZi = true;
                RemoveBlock(dian.DOWN.Block);
            }
            if (dian.LEFT != null && dian.LEFT.Qi == 0)
            {
                TiZi = true;
                RemoveBlock(dian.LEFT.Block);
            }
            if (dian.RIGHT != null && dian.RIGHT.Qi == 0)
            {
                TiZi = true;
                RemoveBlock(dian.RIGHT.Block);
            }
            return TiZi;
        }

        private void RemoveBlock(GoBlock block)
        {
            if (block == null || block.DianNumber == 0)
            {
                GoException.Throw("Attempt remove a NULL or EMPTY Block");
            }
            block.Remove();
        }

        private void RemoveZi(GoDian dian)
        {
            if (dian.Block == null || dian.Block.DianNumber == 0)
            {
                GoException.Throw("Found an unexpected Dian which Block is NULL or EMPTY");
            }
            GoBlock block = dian.Block;
            block.RemoveDian(dian);
            dian.Type = GoDianType.EMPTY;
        }


        public bool SetDian(GoCoord coord, GoDianType type)
        {
            GoDian dian = GetDian(coord);
            if (dian == null)
            {
                GoException.Throw("The Coord is out of bounds");
            }
            else if (dian.Type == type)
            {
                // no change
                return true;
            }
            else if (type == GoDianType.EMPTY)
            {
                // Remove Zi
                RemoveZi(dian);
                return true;
            }
            else if (dian.Type != GoDianType.EMPTY)
            {
                if (!allowChangeZi_)
                {
                    //Error need refresh
                    GoException.Throw("Cannot change chess, should remove it firstly");
                }
                RemoveZi(dian);
            }
            dian.Type = type;
            dian.Block = new GoBlock(SIZE, dian);
            if (autoTake_)
            {
                if (dian.Qi == 0 && !CheckTiZi(dian))
                {
                    RemoveZi(dian);
                    return false;
                }
                else
                {
                    CheckTiZi(dian);
                }
            }

            if (DianChanged != null)
            {
                DianChanged();
            }

            return true;
        }

        public bool SetDian(int row, int col, GoDianType type)
        {
            return SetDian(new GoCoord(row, col), type);
        }

        public int GetQi(int row, int col)
        {
            GoDian dian = GetDian(row, col);
            if (dian != null && dian.Type != GoDianType.EMPTY)
            {
                return dian.Qi;
            }
            return -1;
        }
    }
}
