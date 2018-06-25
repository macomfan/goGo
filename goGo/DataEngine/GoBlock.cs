using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace goGo.DataEngine
{
    public class GoBlock
    {
        static int mainid = 0;
        private int ID = 0; //For Test
        private int size_ = 0;
        private GoDianType type_ = GoDianType.EMPTY;
        private Dictionary<int, GoDian> dianMap_ = new Dictionary<int, GoDian>();
        private int qi_ = -1;
        private List<GoDian> visitedList_ = new List<GoDian>();

        public GoBlock(int size, GoDian dian)
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
        }

        public void Merge(GoBlock block)
        {
            if (block.Type != type_)
            {
                throw new Exception("ERROR: Attempt merge two block under different type");
            }
            foreach (KeyValuePair<int, GoDian> pair in block.dianMap_)
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

        private void ClearQiOfBlock(GoDian dian)
        {
            if (dian == null || dian.Block == null)
            {
                return;
            }
            dian.Block.CleanQi();
        }

        public void Remove()
        {
            foreach(KeyValuePair<int, GoDian> dian in dianMap_)
            {
                ClearQiOfBlock(dian.Value.UP);
                ClearQiOfBlock(dian.Value.DOWN);
                ClearQiOfBlock(dian.Value.LEFT);
                ClearQiOfBlock(dian.Value.RIGHT);
            }
            foreach (KeyValuePair<int, GoDian> dian in dianMap_)
            {
                dian.Value.Block = null;
                dian.Value.Type = GoDianType.EMPTY;
            }
            dianMap_.Clear();
        }

        public bool RemoveDian(GoDian dian)
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

        private int CalculateNextQi(GoDian dian, GoDian newDian)
        {
            int qi = 0;
            if (newDian == null || newDian.IsVisited())
            {
                return 0;
            }
            else if (newDian.Type == GoDianType.EMPTY)
            {
                newDian.MarkAsVisited();
                visitedList_.Add(newDian);
                //                 if (direction == Direction.UP)
                //                 {
                //                     System.Diagnostics.Debug.WriteLine(String.Format("Add Qi ({0},{1}) on UP", mu.Row, mu.Col));
                //                 }
                //                 else if (direction == Direction.DOWN)
                //                 {
                //                     System.Diagnostics.Debug.WriteLine(String.Format("Add Qi ({0},{1}) on DOWN", mu.Row, mu.Col));
                //                 }
                //                 else if (direction == Direction.LEFT)
                //                 {
                //                     System.Diagnostics.Debug.WriteLine(String.Format("Add Qi ({0},{1}) on LEFT", mu.Row, mu.Col));
                //                 }
                //                 else
                //                 {
                //                     System.Diagnostics.Debug.WriteLine(String.Format("Add Qi ({0},{1}) on RIGHT", mu.Row, mu.Col));
                //                 }
                qi += 1;
            }
            else if (newDian.Type == dian.Type)
            {
                qi += CalculateCurrentQi(newDian);
            }
            return qi;
        }

        private int CalculateCurrentQi(GoDian dian)
        {
            if (dian == null)
            {
                return 0;
            }
            dian.MarkAsVisited();
            visitedList_.Add(dian);
            int qi = 0;
            qi += CalculateNextQi(dian, dian.UP);
            qi += CalculateNextQi(dian, dian.DOWN);
            qi += CalculateNextQi(dian, dian.LEFT);
            qi += CalculateNextQi(dian, dian.RIGHT);
            return qi;
        }

        public void CleanQi()
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
            GoDian dian = dianMap_.Values.First();
            qi_ = CalculateCurrentQi(dian);
            foreach (GoDian visited in visitedList_)
            {
                visited.ResetStatus();
            }
            visitedList_.Clear();
        }

        public int DianNumber
        {
            get { return dianMap_.Count; }
        }

        public GoDianType Type
        {
            get { return type_; }
        }

        public GoDian GetDian()
        {
            return null;
        }
    }
}
