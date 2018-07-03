using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GoModel;

namespace goGo.Game
{
    interface IViewController
    {
        void OnSelectDian(GoCoord coord);
        void OnClickDian(GoCoord coord);

        bool OnMouseClick(MouseEventArgs e);
        bool OnMouseMove(MouseEventArgs e);
    }
}
