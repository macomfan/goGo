using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace goGo.DataEngine
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

        private bool DetectAndAddDianToBlock(GoDian dian, GoDian nextDian)
        {
            if (nextDian == null || nextDian.Type == GoDianType.EMPTY)
            {
                return false;
            }
            else if (nextDian.Block == null || nextDian.Block.DianNumber == 0)
            {
                //Error need refresh
                throw new Exception("ERROR: The Block of Zi is NULL or EMPTY");
            }
            else if (nextDian.Block.Type != dian.Type)
            {
                nextDian.Block.CleanQi();
                return false;
            }
            else if (dian.Block != nextDian.Block)
            {
                dian.Block.Merge(nextDian.Block);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckTiZi(GoDian dian)
        {
            if (dian.UP != null && dian.UP.Qi == 0)
            {
                RemoveBlock(dian.UP.Block);
            }
            if (dian.DOWN != null && dian.DOWN.Qi == 0)
            {
                RemoveBlock(dian.DOWN.Block);
            }
            if (dian.LEFT != null && dian.LEFT.Qi == 0)
            {
                RemoveBlock(dian.LEFT.Block);
            }
            if (dian.RIGHT != null && dian.RIGHT.Qi == 0)
            {
                RemoveBlock(dian.RIGHT.Block);
            }
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
            if (block.DianNumber != 0)
            {
                //recalculate qi
                dian.Block.CleanQi();
            }
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
            // Add dian into EMPTY
            DetectAndAddDianToBlock(dian, dian.UP);
            DetectAndAddDianToBlock(dian, dian.RIGHT);
            DetectAndAddDianToBlock(dian, dian.DOWN);
            DetectAndAddDianToBlock(dian, dian.LEFT);
            dian.Block.CleanQi();

            if (autoTake_)
            {
                CheckTiZi(dian);
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
