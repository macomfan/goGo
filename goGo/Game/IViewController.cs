using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using goGo.DataEngine;

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
