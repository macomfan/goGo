using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoStep
    {
        private GoCoord coord_ = null;
        private GoPointType type_ = GoPointType.EMPTY;
        private int id_ = 0;

        public GoCoord Coord
        {
            get { return coord_; }
        }

        public GoPointType Type
        {
            get { return type_; }
        }

        public GoStep(GoCoord coord, GoPointType type)
        {
            coord_ = coord;
            type_ = type;
        }
    }
}
