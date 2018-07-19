using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using GoModel;

namespace goGo.Painter
{
    public class ChessBoard
    {
        private float borderPenWidth_ = 3.0f;
        private float inLinePenWidth_ = 1.0f;
        private float margin_ = 20.0f;
        private float starWidth_ = 4.0f;
        private float rulerWidth_ = 10.0f;
        private PointF borderTopLeft = new PointF(0.0f, 0.0f);
        private PointF borderBottomRight = new PointF(0.0f, 0.0f);

        private int size_ = 300;
        private float step_ = 0.0f;

        private Bitmap canvas_ = null;
        private List<Pieces> piecesList_ = new List<Pieces>();
        private GoLayout layout_ = null;
        private Ruler ruler_ = null;

        private GoCoord selected_ = null;
        public ChessBoard()
        {

        }

        public GoLayout Layout
        {
            get { return layout_; }
        }

        public void BindLayout(GoLayout layout)
        {
            if (layout_ == layout || layout == null)
            {
                return;
            }
            if (layout_ != null)
            {
                layout_.PointChanged -= new GoLayout.PointChangedHandler(Layout_DianChanged);
            }
            layout_ = layout;
            layout_.PointChanged += new GoLayout.PointChangedHandler(Layout_DianChanged);
            ruler_ = new Ruler(layout.Size);
            Layout_DianChanged();
        }

        void Layout_DianChanged()
        {
            piecesList_.Clear();
            if (layout_ == null)
            {
                return;
            }
            for (int i = 0; i < GoLayout.SIZE; i++)
            {
                for (int j = 0; j < GoLayout.SIZE; j++)
                {
                    GoPoint dian = layout_.GetPoint(i, j);
                    if (dian.Type != GoPointType.EMPTY)
                    {
                        piecesList_.Add(new Pieces(i, j, dian.Type));
                    }
                }
            }
        }

        public int Size
        {
            get { return size_; }
            set {
                if (size_ != value)
                {
                    size_ = value;
                    canvas_ = null;
                }
            }
        }

        private void ReCalculateBorder()
        {
            margin_ = size_ * 0.01f;
            if (margin_ < 10.0f)
            {
                margin_ = 10.0f;
            }
            else if (margin_ > 20.0f)
            {
                margin_ = 20.0f;
            }

            rulerWidth_ = size_ * 0.05f;
            if (rulerWidth_ < 15.0f)
            {
                rulerWidth_ = 15.0f;
            }
            else if (margin_ > 40.0f)
            {
                rulerWidth_ = 40.0f;
            }

            borderTopLeft = new PointF(margin_ + rulerWidth_, margin_ + rulerWidth_);
            borderBottomRight = new PointF(size_ - margin_, size_- margin_);
            step_ = (borderBottomRight.X - borderTopLeft.X) / (float)(GoLayout.SIZE - 1);
        }

        private void ReDraw()
        {
            if (canvas_ != null)
            {
                return;
            }
            ReCalculateBorder();
            // Recalculate margin
            canvas_ = new Bitmap(size_, size_);

            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Pen inLinePen = new Pen(blackBrush);
            inLinePen.Width = inLinePenWidth_;
            Pen borderPen = new Pen(blackBrush);
            borderPen.Width = borderPenWidth_;

            Graphics bufferGraphics = Graphics.FromImage(canvas_);
            float xLeft = borderTopLeft.X;
            float xRight = borderBottomRight.X;
            float yTop = borderTopLeft.Y;
            float yBottom = borderBottomRight.Y;

            float xIndex = borderTopLeft.X;
            float yIndex = borderTopLeft.Y;
            for (int j = 0; j < GoLayout.SIZE; j++)
            {
                if (j != 0 && j != GoLayout.SIZE - 1)
                {
                    bufferGraphics.DrawLine(inLinePen, xIndex, yTop, xIndex, yBottom);
                }
                xIndex += step_;
            }
            for (int i = 0; i < GoLayout.SIZE; i++)
            {
                if (i != 0 && i != GoLayout.SIZE - 1)
                {
                    bufferGraphics.DrawLine(inLinePen, xLeft, yIndex, xRight, yIndex);
                }
                yIndex += step_;
            }
            bufferGraphics.DrawRectangle(borderPen, xLeft, yTop, xRight - xLeft, yBottom - yTop);

//             GoStar[] stars = GoLayout.GetStars();
//             float starOffset = starWidth_ / 2.0f;
//             foreach (GoStar star in stars)
//             {
//                 float x = xLeft + star.Row * step_;
//                 float y = yTop + star.Col * step_;
//                 bufferGraphics.FillEllipse(blackBrush, x - starOffset, y - starOffset, starWidth_, starWidth_);
//             }
        }

        public void DrawBoard(Graphics g)
        {
            ReDraw();
            g.DrawImage(canvas_, 0, 0);

            if (ruler_ != null)
            {
                ruler_.Draw(g, step_, rulerWidth_, margin_);
            }
        }

        public void DrawLayout(Graphics g)
        {
            if (layout_ == null)
            {
                return;
            }
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Pen inLinePen = new Pen(blackBrush);
            foreach (Pieces pieces in piecesList_)
            {
                GoPoint dian = layout_.GetPoint(pieces.Coord.Row, pieces.Coord.Col);
                if (dian.Type == GoPointType.BLACK)
                {
                    PointF center = GetCenter(pieces.Coord);
                    g.DrawImage(Pieces.black_, center.X - 9, center.Y - 9, 18, 18);
                    //g.FillRectangle(blackBrush, center.X - 5, center.Y - 5, 10, 10);
                }
                else if (dian.Type == GoPointType.WHITE)
                {
                    PointF center = GetCenter(pieces.Coord);
                    g.DrawImage(Pieces.white_, center.X - 9, center.Y - 9, 18, 18);
                    //g.DrawRectangle(inLinePen, center.X - 5, center.Y - 5, 10, 10);
                }
            }
        }

        private PointF GetCenter(GoCoord coord)
        {
            return new PointF(borderTopLeft.X + coord.Col * step_, borderTopLeft.Y + coord.Row * step_);
        }

        public void DrawSelected(Graphics g)
        {
            if (selected_ == null)
            {
                return;
            }
            SolidBrush blackBrush = new SolidBrush(Color.Red);
            Pen inLinePen = new Pen(blackBrush);
            float x = borderTopLeft.X + selected_.Col * step_;
            float y = borderTopLeft.Y + selected_.Row * step_;
            float sizeHalf = 10.0f / 2.0f;
            g.DrawRectangle(inLinePen, x - sizeHalf, y - sizeHalf, 10.0f, 10.0f);
        }

        private GoCoord hitTest(int x, int y, int col, int row, int precision)
        {
            if (row < 0 || row > GoLayout.SIZE - 1 || col < 0 || col > GoLayout.SIZE - 1 || precision == 0)
            {
                return null;
            }
            int centerX = (int)(col * step_);
            int centerY = (int)(row * step_);
            //System.Diagnostics.Debug.WriteLine(String.Format("Rect ({0},{1}) on Row:{2}, Col:{3}", centerX, centerY, row, col));
            if (centerX - precision < x && x < centerX + precision)
            {
                if (centerY - precision < y && y < centerY + precision)
                {
                    return new GoCoord(row, col);
                }
            }
            return null;
        }

        public GoCoord SelectedCoord
        {
            get { return selected_; }
            set { selected_ = value; }
        }

        public GoCoord hitTest(int x, int y , int precision = 5)
        {
            if (precision <= 0 || canvas_ == null)
            {
                return null;
            }
            int col = x / (int)step_;
            int row = y / (int)step_;

            Point point = new Point(x, y);
            point.Offset(-(int)borderTopLeft.X, -(int)borderTopLeft.Y);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    GoCoord coord = hitTest(point.X, point.Y, col + j, row + i, precision);
                    if (coord != null)
                    {
                        return coord;
                    }
                }
            }
            return null;
        }

        public GoCoord hitTest(Point point, int precision = 5)
        {
            return hitTest(point.X, point.Y);
        }

        public Bitmap GetCanvas()
        {
            return canvas_;
        }
    }
}
