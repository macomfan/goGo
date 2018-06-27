using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace goGo.Painter
{
    public class Ruler
    {
        public static List<String> horizontal_ = new List<String>();
        public static List<String> vertical_ = new List<String>();
        public static Font font_ = null;
        private int size_ = 0;
        public Ruler(int size)
        {
            size_ = size;
            for (int i = 0; i < size; i++ )
            {
                horizontal_.Add(String.Format("{0}", (char)(65+i)));
                vertical_.Add(String.Format("{0}", i + 1));
            }
        }



        public void Draw(Graphics g, float step, float range, float margin)
        {
            float horizontalStart = range + margin;
            float verticalStart = range + margin;
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            font_ = new Font("Verdana", range * 0.4f, GraphicsUnit.Pixel);

            g.DrawLine(new Pen(new SolidBrush(Color.Red)), 0, margin, 200, margin);
            g.DrawLine(new Pen(new SolidBrush(Color.Red)), 0, margin + range, 200, margin + range);

            float halfRange = range / 2.0f;
            for (int i = 0; i < size_; i++)
            {
                {
                    float x = horizontalStart + i * step;
                    float y = margin;
                    SizeF size = g.MeasureString(horizontal_[i], font_);
                    g.DrawString(horizontal_[i], font_, new SolidBrush(Color.Black), x - size.Width / 2.0f, y - size.Height / 2.0f);
                    //g.FillEllipse(blackBrush, x - 2, y - 2, 4, 4);
                }
                {
                    float x = halfRange + margin;
                    float y = verticalStart + i * step;
                    SizeF size = g.MeasureString(vertical_[i], font_);
                    g.DrawString(vertical_[i], font_, new SolidBrush(Color.Black), x - size.Width / 2.0f, y - size.Height / 2.0f);
                }
            }
        }
    }
}
