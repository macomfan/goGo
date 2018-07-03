using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoStep
    {
        private GoCoord coord_ = null;
        private GoDianType type_ = GoDianType.EMPTY;

        public GoCoord Coord
        {
            get { return coord_; }
        }

        public GoDianType Type
        {
            get { return type_; }
        }

        public GoStep(GoCoord coord, GoDianType type)
        {
            coord_ = coord;
            type_ = type;
        }
    }
}
