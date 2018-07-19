using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using GoModel;
using goGo.Painter;
using goGo.Game;

namespace goGo
{
    public partial class GoView : Control
    {
        private float xOffset_ = 0.0f;
        private float yOffset_ = 0.0f;

        ChessBoard cb = null;
        GoLayout layout = new GoLayout();
        FreeMode mode = new FreeMode();

        public GoView()
        {
            cb = new ChessBoard();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
            cb.BindLayout(layout);
            mode.BindChessBoard(cb);
//             layout.SetDian(1, 0, GoDianType.BLACK);
//             layout.SetDian(0, 1, GoDianType.BLACK);
//             layout.SetDian(2, 1, GoDianType.BLACK);
//             layout.SetDian(1, 2, GoDianType.BLACK);
//             layout.SetDian(0, 2, GoDianType.WHITE);
//             layout.SetDian(1, 3, GoDianType.WHITE);
//             layout.SetDian(2, 2, GoDianType.WHITE);

            layout.SetupPoint(new GoCoord(1, 2), GoPointType.BLACK);
            layout.SetupPoint(new GoCoord(0, 1), GoPointType.BLACK);
            layout.SetupPoint(new GoCoord(2, 1), GoPointType.BLACK);
            layout.SetupPoint(new GoCoord(1, 1), GoPointType.WHITE);
        }

        int x_print;
        int y_print;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

//             Font font = new Font("Verdana", 15);
//             SizeF size = e.Graphics.MeasureString("A", font);
//             Bitmap a = new Bitmap((int)size.Width+10, (int)size.Height+10);
//             Graphics bg = Graphics.FromImage(a);
//             bg.DrawString("A", font, new SolidBrush(Color.Red), 0.0f, 0.0f);
//             e.Graphics.DrawImage(a, 0.0f, 0.0f, size.Width+100.0f, size.Height+100.0f);
//             Matrix matrix = new Matrix();
//             matrix.Scale(5.0f, 5.0f);
//             Graphics g = e.Graphics;
//             g.Transform = matrix;
//             g.DrawString("AAA", this.Font, Brushes.Black, 50,50);
//             g.ResetTransform();
//             e.Graphics.DrawString("A", new Font("Verdana", 12), new SolidBrush(Color.Red), 250, 250);
            
            Font font = new Font("Verdana", 12, GraphicsUnit.Pixel);
            e.Graphics.DrawString(string.Format("({0},{1})", x_print, y_print), font, new SolidBrush(Color.Red), new PointF(0.0f, 0.0f));
            //e.Graphics.DrawString(string.Format("({0},{1})", x_print, y_print), font, new SolidBrush(Color.Red), new Rectangle(0, 0,100,100));

            e.Graphics.TranslateTransform(xOffset_, yOffset_);
            cb.DrawBoard(e.Graphics);
            cb.DrawSelected(e.Graphics);
            cb.DrawLayout(e.Graphics);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            point.Offset(-(int)xOffset_, -(int)yOffset_);
            GoCoord coord = cb.hitTest(point, 10);
            if (coord != null)
            {
                x_print = coord.Row;
                y_print = coord.Col;
                cb.SelectedCoord = coord;
                int qi = cb.Layout.GetQi(coord.Row, coord.Col);
                this.Invalidate();
                //mode.o;
            }
            else
            {
                if (cb.SelectedCoord != null)
                {
                    cb.SelectedCoord = null;
                    this.Invalidate();
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Point point = e.Location;
            point.Offset(-(int)xOffset_, -(int)yOffset_);
            MouseEventArgs newArgs = new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta);
            if (mode.OnMouseClick(newArgs))
            {
                this.Invalidate();
            }
        }

        private void GoView_Resize(object sender, EventArgs e)
        {
            int maxSize = 0;
            if (this.Height > this.Width)
            {
                maxSize = this.Width;
                xOffset_ = 0.0f;
                yOffset_ = (this.Height - maxSize) / 2.0f;

            }
            else if (this.Width > this.Height)
            {
                maxSize = this.Height;
                xOffset_ = (this.Width - maxSize) / 2.0f;
                yOffset_ = 0.0f;
            }
            else
            {
                maxSize = this.Height;
                xOffset_ = 0.0f;
                yOffset_ = 0.0f;
            }
            if (maxSize != cb.Size)
            {
//                 System.Diagnostics.Debug.WriteLine("Resize: size: {0} on H: {1}, W: {2}, XOFF: {3}, YOFF: {4}",
//                     maxSize, Height, Width, (int)xOffset_, (int)yOffset_);
                cb.Size = maxSize;
                this.Invalidate();
            }
        }
    }
}
