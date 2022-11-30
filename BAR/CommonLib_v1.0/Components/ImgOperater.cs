using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;


namespace BAR.Commonlib.Components
{
    public class ImgOperater
    {
        ///<主窗口
        public Form ObjParentWnd;

        public bool IsZoomImg;

        public bool IsImgMove;

        public double DTempTop, DTempLeft, DTempRight, DTempBottom;

        public int ITop, ILeft, IRight, IBottom;

        public double DZoomWndFactor;
        ///<判定是否显示轮廓
        public bool IsContours;
        ///<只显示原图
        public bool IsOrigImg;

        public bool IsLineFlag;

        public HObject ObjRegionImg;

        public int IImgWidth;

        public int IImgHeight;

        ///<Halcon窗口
        public HTuple ObjHWnd;
        ///<图像
        public HObject ObjSourceImg;
        ///<Halcon窗口宽
        public int IWndWidth;

        public int IWndHeight;
        ///<用于获取鼠标移动
        public HTuple ObjButton;
        ///<鼠标当前坐标
        public HTuple ObjMouseX, ObjMouseY;
        public HTuple ObjRow, ObjColumn;
        //public HTuple ObjMouseX, ObjMouseY;

        public void Initial(Form parentWnd, HTuple halconWnd)
        {
            this.ObjParentWnd = parentWnd;

            this.ObjHWnd = halconWnd;
        }
        public void On_Move()
        {
            ////////移动图像///////////////////
            HTuple Dx, Dy, Button1;
            try
            {
                if (ObjButton != 1)
                    HOperatorSet.GetMposition(ObjHWnd, out ObjRow, out ObjColumn, out ObjButton);
                if (ObjButton == 1)
                {
                    HOperatorSet.GetMposition(ObjHWnd, out ObjMouseY, out ObjMouseX, out Button1);

                    if (Button1 != 1)
                        ObjButton = -1;

                    Dx = (ObjMouseX - ObjColumn);
                    Dy = (ObjMouseY - ObjRow);

                    ITop = ITop - Dy;
                    ILeft = ILeft - Dx;
                    IRight = IRight - Dx;
                    IBottom = IBottom - Dy;
                    DispImg();
                }
            }
            catch (HalconException hEx)
            {
                //return ;
            }
        }

        public void GetImage(HObject image)
        {
            this.ObjSourceImg = image;

            HTuple width, height;

            try
            {
                HOperatorSet.GetImageSize(ObjSourceImg, out width, out height);
                ITop = 0;
                ILeft = 0;
                DTempRight = IImgWidth = IRight = (int)width[0].D;
                DTempRight = IBottom = (int)height[0].D;

                DTempTop = ITop;
                DTempLeft = ILeft;
                DTempRight = IRight;
                DTempBottom = IBottom;

                DZoomWndFactor = 1.0;
            }
            catch (HalconException ex)
            {

            }
        }
        public void DispImg()
        {
            try
            {
                if (IsOrigImg)
                {
                    DTempTop = ITop;
                    DTempLeft = ILeft;
                    DTempRight = IRight;
                    DTempBottom = IBottom;
                }
            }
            catch (HalconException ex)
            {

            }
        }
        private double Round(double r)
        {
            return (r > 0.0) ? Math.Floor( r + 0.5 ) : Math.Ceiling( r - 0.5 );
        }
        public void ZoomImage(double x, double y, double scale)
        {
            double lengthC, lengthR;
            double percentC, percentR;
            int lenC, lenR;

            percentC = (x - DTempLeft) / (DTempRight - DTempLeft);
            percentR = (y - DTempTop) / (DTempBottom - DTempTop);

            lengthC = (DTempRight - DTempLeft) * scale;
            lengthR = (DTempBottom - DTempTop) * scale;

            DTempLeft = x - lengthC * percentC;
            DTempRight = x + lengthC * (1 - percentC);

            DTempTop = y - lengthR * percentR;
            DTempBottom = y + lengthR * (1 - percentR);

            lenC = (int)(lengthC + 0.5);
            lenR = (int)(lengthR + 0.5);

            ILeft = (int)(Round(DTempLeft));
            ITop = (int)(Round(DTempTop));
            IRight = (lenC > 0) ? lenC : 1;
            IRight = IRight + ILeft;
            IBottom = (lenR > 0) ? lenR : 1;
            IBottom = IBottom + ITop;

            DispImg();
        }
    }
}
