using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoBlock
    {
        static int mainid = 0;
        private int ID = 0; //For Test
        private int size_ = 0;
        private GoPointType type_ = GoPointType.EMPTY;
        private Dictionary<int, GoPoint> dianMap_ = new Dictionary<int, GoPoint>();
        private int qi_ = -1;
        private List<GoPoint> visitedList_ = new List<GoPoint>();

        public GoBlock(int size, GoPoint dian)
        {
            size_ = size;
            type_ = dian.Type;
            ID = ++mainid;
            if (dian.Block != null)
            {
                GoException.Throw("Attempt add a Dian to double Block");
            }
            int index = dian.Coord.GetIndex(size_);
            dianMap_.Add(index, dian);
            dian.Block = this;
            CleanQi();
            DetectNeighborBlock(dian, dian.UP);
            DetectNeighborBlock(dian, dian.DOWN);
            DetectNeighborBlock(dian, dian.LEFT);
            DetectNeighborBlock(dian, dian.RIGHT);
        }

        private bool DetectNeighborBlock(GoPoint dian, GoPoint nextDian)
        {
            if (nextDian == null || nextDian.Type == GoPointType.EMPTY)
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

        private void Merge(GoBlock block)
        {
            if (block.Type != type_)
            {
                throw new Exception("ERROR: Attempt merge two block under different type");
            }
            foreach (KeyValuePair<int, GoPoint> pair in block.dianMap_)
            {
                if (!dianMap_.ContainsKey(pair.Key))
                {
                    dianMap_.Add(pair.Key, pair.Value);
                    pair.Value.Block = this;
                }
                else
                {
                    throw new Exception("ERROR: Found different blocks have the same dian");
                }
            }
            block.dianMap_.Clear();
        }

        private void ClearQiOfBlock(GoPoint dian)
        {
            if (dian == null || dian.Block == null)
            {
                return;
            }
            dian.Block.CleanQi();
        }

        public void Remove()
        {
            foreach(KeyValuePair<int, GoPoint> dian in dianMap_)
            {
                ClearQiOfBlock(dian.Value.UP);
                ClearQiOfBlock(dian.Value.DOWN);
                ClearQiOfBlock(dian.Value.LEFT);
                ClearQiOfBlock(dian.Value.RIGHT);
            }
            foreach (KeyValuePair<int, GoPoint> dian in dianMap_)
            {
                dian.Value.Block = null;
                dian.Value.Type = GoPointType.EMPTY;
            }
            dianMap_.Clear();
        }

        public bool RemoveDian(GoPoint dian)
        {
            if (dian.Type != type_)
            {
                return false;
            }
            int index = dian.Coord.GetIndex(size_);
            if (!dianMap_.ContainsKey(index))
            {
                return false;
            }
            // remove the block;
            dian.Block = null;
            dianMap_.Remove(index);
            ClearQiOfBlock(dian.UP);
            ClearQiOfBlock(dian.DOWN);
            ClearQiOfBlock(dian.LEFT);
            ClearQiOfBlock(dian.RIGHT);
            if (DianNumber != 0)
            {
                CleanQi();
            }
            return true;
        }

        public int Qi
        {
            get
            {
                if (qi_ == -1)
                {
                    CalculateQi();
                }
                return qi_;
            }
        }

        private int CalculateNextQi(GoPoint dian, GoPoint newDian)
        {
            int qi = 0;
            if (newDian == null || newDian.IsVisited())
            {
                return 0;
            }
            else if (newDian.Type == GoPointType.EMPTY)
            {
                newDian.SetStatusAsVisited();
                visitedList_.Add(newDian);
                qi += 1;
            }
            else if (newDian.Type == dian.Type)
            {
                qi += CalculateCurrentQi(newDian);
            }
            return qi;
        }

        private int CalculateCurrentQi(GoPoint dian)
        {
            if (dian == null)
            {
                return 0;
            }
            dian.SetStatusAsVisited();
            visitedList_.Add(dian);
            int qi = 0;
            qi += CalculateNextQi(dian, dian.UP);
            qi += CalculateNextQi(dian, dian.DOWN);
            qi += CalculateNextQi(dian, dian.LEFT);
            qi += CalculateNextQi(dian, dian.RIGHT);
            return qi;
        }

        private void CleanQi()
        {
            qi_ = -1;
        }

        private void CalculateQi()
        {
            if (dianMap_.Count == 0)
            {
                throw new Exception("ERROR: Calculate Qi from a empty block");
            }
            if (qi_ != -1)
            {
                return;
            }
            GoPoint dian = dianMap_.Values.First();
            qi_ = CalculateCurrentQi(dian);
            foreach (GoPoint visited in visitedList_)
            {
                visited.ResetStatus();
            }
            visitedList_.Clear();
        }

        public int DianNumber
        {
            get { return dianMap_.Count; }
        }

        public GoPointType Type
        {
            get { return type_; }
        }
    }
}
