using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAR.Commonlib
{
    public class PointD
    {
        public Double X { set; get; }
        public Double Y { set; get; }
        public PointD()
        {

        }
        public  PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
