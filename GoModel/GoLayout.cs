using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoLayout
    {
        public delegate void PointChangedHandler();
        public event PointChangedHandler PointChanged;

        public static int SIZE = 19;
        public static int STAR_NUM = 9;

        private GoPoint[] layout_ = new GoPoint[SIZE * SIZE];
        private static GoStar[] stars_ = new GoStar[STAR_NUM];
        private GoPointVisitor visitor_ = null;

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

        public GoLayout()
        {
            visitor_ = new GoPointVisitor(this);
            ResetPoint();
        }

        private void ResetPoint()
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    GoPoint point = new GoPoint(new GoCoord(i, j), visitor_);
                    layout_[i * SIZE + j] = point;
                }
            }
        }

        public void PushStep(GoStep step)
        {
            GoPoint point = OnAddChess(step.Coord, step.Type);
            OnCheckCapture(point);
            steps_.Push(step);
            OnChessChanged();
        }

        public bool PopStep()
        {
            GoStep step = steps_.Pop();

            return false;
        }

        public void ClearSteps()
        {
            if (steps_.Count != 0)
            {
                steps_.Clear();
                ResetPoint();
                OnChessChanged();
            }
        }

        public static GoStar[] GetStars()
        {
            return stars_;
        }

        public GoPoint GetPoint(int row, int col)
        {
            return GetPoint(new GoCoord(row, col));
        }

        public GoPoint GetPoint(GoCoord coord)
        {
            if (!coord.IsVaild(SIZE))
            {
                return null;
            }
            return layout_[coord.GetIndex(SIZE)];
        }

        private bool OnCheckCapture(GoPoint point)
        {
            bool needCapture = false;
            if (point.UP != null && point.UP.Qi == 0)
            {
                needCapture = true;
                RemoveBlock(point.UP.Block);
            }
            if (point.DOWN != null && point.DOWN.Qi == 0)
            {
                needCapture = true;
                RemoveBlock(point.DOWN.Block);
            }
            if (point.LEFT != null && point.LEFT.Qi == 0)
            {
                needCapture = true;
                RemoveBlock(point.LEFT.Block);
            }
            if (point.RIGHT != null && point.RIGHT.Qi == 0)
            {
                needCapture = true;
                RemoveBlock(point.RIGHT.Block);
            }
            return needCapture;
        }

        private void RemoveBlock(GoBlock block)
        {
            if (block == null || block.DianNumber == 0)
            {
                GoException.Throw("Attempt remove a NULL or EMPTY Block");
            }
            block.Remove();
        }

        private void OnRemoveChess(GoPoint point)
        {
            if (point.Block == null || point.Block.DianNumber == 0)
            {
                GoException.Throw("Found an unexpected Dian which Block is NULL or EMPTY");
            }
            GoBlock block = point.Block;
            block.RemoveDian(point);
            point.Type = GoPointType.EMPTY;
        }

        private GoPoint OnAddChess(GoCoord coord, GoPointType type)
        {
            GoPoint point = GetPoint(coord);
            if (point == null)
            {
                GoException.Throw("The Coord is out of bounds");
            }
            else if (point.Type == type)
            {
                // no change
                return point;
            }
            if (point.Type != GoPointType.EMPTY)
            {
                GoException.Throw("Should remove the Chess first");
            }
            point.Type = type;
            point.Block = new GoBlock(SIZE, point);
            return point;
        }

        private void OnChessChanged()
        {
            if (PointChanged != null)
            {
                PointChanged();
            }
        }

//         public bool SetDian(GoCoord coord, GoDianType type)
//         {
// 
//             else if (dian.Type == type)
//             {
//                 // no change
//                 return true;
//             }
//             else if (type == GoDianType.EMPTY)
//             {
//                 // Remove Zi
//                 RemoveChess(dian);
//                 return true;
//             }
//             else if (dian.Type != GoDianType.EMPTY)
//             {
// //                 if (!allowChangeZi_)
// //                 {
// //                     //Error need refresh
// //                     GoException.Throw("Cannot change chess, should remove it firstly");
// //                 }
//                 RemoveChess(dian);
//             }
//             dian.Type = type;
//             dian.Block = new GoBlock(SIZE, dian);
// //             if (autoTake_)
// //             {
//                 if (dian.Qi == 0 && !CheckTiZi(dian))
//                 {
//                     RemoveChess(dian);
//                     return false;
//                 }
//                 else
//                 {
//                     CheckTiZi(dian);
//                 }
// /*            }*/
// 
//             if (DianChanged != null)
//             {
//                 DianChanged();
//             }
// 
//             return true;
//         }

        public bool SetupPoint(GoCoord coord, GoPointType type)
        {
            GoPoint point = GetPoint(coord);
            if (point != null && point.Type != type)
            {
                OnRemoveChess(point);
                OnAddChess(coord, type);
            }
            return true;
        }

        public int GetQi(int row, int col)
        {
            GoPoint point = GetPoint(row, col);
            if (point != null && point.Type != GoPointType.EMPTY)
            {
                return point.Qi;
            }
            return -1;
        }
    }
}
