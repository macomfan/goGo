using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoPointVisitor
    {
        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
        }

        private GoLayout layout_ = null;


        public GoPointVisitor(GoLayout layout)
        {
            layout_ = layout;
        }

        public GoPoint GetPointByDirection(GoPoint dian, Direction direction)
        {
            int newRow = dian.Coord.Row;
            int newCol = dian.Coord.Col;
            switch (direction)
            {
                case Direction.UP:
                    newRow--;
                    break;
                case Direction.DOWN:
                    newRow++;
                    break;
                case Direction.LEFT:
                    newCol--;
                    break;
                case Direction.RIGHT:
                    newCol++;
                    break;
            }
            return layout_.GetPoint(new GoCoord(newRow, newCol));
        }
    }
}
