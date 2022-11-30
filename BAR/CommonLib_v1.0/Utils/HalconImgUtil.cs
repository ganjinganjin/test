using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using BAR.Commonlib.Utils;
using BAR.Commonlib.Components;
using HalconDotNet;

namespace BAR.Commonlib.Utils
{
    public class HalconImgUtil
    {
        private static HalconImgUtil _instance = null;
        public      int     IImgHeight;
        public      int     IImgWidth;
        public      int     IImgCenterX;
        public      int     IImgCenterY;
        public      int[]   ArrBoxW;
        public      int[]   ArrBoxH;
        public      int     IBoxC;


        HObject cross;

        private static readonly object _objPadLock = new object();


        public delegate void ShowMsgDelegate(string strMsg);
        public ShowMsgDelegate ShowMsg;

        public HalconImgUtil()
        {
            ArrBoxW = new int[2];
            ArrBoxH = new int[2];
        }
        public static HalconImgUtil GetInstance()
        {
            if (_instance == null)
            {
                lock (_objPadLock)
                {
                    if (_instance == null)
                    {
                        _instance = new HalconImgUtil();
                    }
                }
            }
            return _instance;
        }
        public bool CreateImgDispRect(HWindow hWnd, int wndW, int wndH,out HTuple hWndID)
        {
            try
            {
                HOperatorSet.SetSystem("width", wndW);
                HOperatorSet.SetSystem("height", wndH);
                HOperatorSet.OpenWindow(-1, -1, wndW, wndH, hWnd, "visible", "", out hWndID);
                HDevWindowStack.Push(hWndID);
                //HOperatorSet.ReadImage(out img, "pic2");
                //HOperatorSet.ClearWindow(hWndID);
                //img.Dispose();
                if (hWndID == null) return false;
                HOperatorSet.SetColor(hWndID, "red");
                return true;
            }
            catch(HalconException hEx)
            {
                MessageBox.Show(hEx.Message, "CreateImgDispRect");
            }
            hWndID = null;
            return false;

        }
        public void LoadImgAndDisp(HTuple wndID, out HObject sourceImg, String path)
        {
            try
            {
                if (File.Exists(path))//判断文件是否存在
                {
                    //读取图象
                    HObject img;
                    HOperatorSet.ReadImage(out img, path);
                    sourceImg = img;
                    
                    //设置显示模式是margin(边缘)
                    HOperatorSet.SetDraw(wndID, "margin");
                    //获取图象宽度高度
                    HTuple L, T, W, H;
                    L = T = 0;
                    HOperatorSet.GetImageSize(sourceImg, out W, out H);
                    //窗口缩放
                    HOperatorSet.SetPart(wndID, L, T, H, W);
                    //显示图象
                    HOperatorSet.DispObj(sourceImg, wndID);
                    img.Dispose();
                    return;
                }
                sourceImg = null;
            }
            catch (HalconException hEx)
            {
                ShowMsg("LoadImgAndDisp：" + hEx.Message);
                MessageBox.Show(hEx.Message, "LoadImgAndDisp");
                sourceImg = null;
            }
        }
        public bool LoadModel(int ind, string path, ref HTuple model)
        {
            try
            {
                if (File.Exists(path))
                {
                    if (ind == GlobConstData.ST_MODELICPOS_NCC || ind == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_MODELICSTATACC
                        || ind == GlobConstData.ST_ModelBredeInPOS || ind == GlobConstData.ST_ModelBredeOutPOS || ind == GlobConstData.ST_ModelTrayPOS
                        /*|| ind == GlobConstData.ST_ModelTrayPOS || ind == GlobConstData.ST_ModelBredeInPOS || ind == GlobConstData.ST_ModelBredeOutPOS 
                        || ind == GlobConstData.ST_ModelTubeInPOS || ind == GlobConstData.ST_ModelTrayACC || ind == GlobConstData.ST_MODELPENACC 
                        || ind == GlobConstData.ST_ModelBredeInACC || ind == GlobConstData.ST_ModelBredeOutACC || ind == GlobConstData.ST_ModelTubeInACC*/)
                    {
                        HOperatorSet.ReadNccModel(path, out model);
                    }
                    else
                    {
                        HOperatorSet.ReadShapeModel(path, out model);
                    }
                    
                    return true;
                }
            }
            catch (HalconException hEx)
            {
                //ShowMsg("LoadModel：" + hEx.Message);
                //MessageBox.Show(hEx.Message, "LoadModel");
            }
            model = null;
            return false;
        }
        public void SaveImg(HObject sourceImg, string path, int y, int x, int len1, int len2, double angle)
        {
            try
            {
                HObject rectangle;
                HObject imgReduced;

                if (len1 != 0 && len2 != 0)
                {
                    HOperatorSet.GenRectangle2(out rectangle, y, x, angle, len1, len2);
                    HOperatorSet.ReduceDomain(sourceImg, rectangle, out imgReduced);

                    HOperatorSet.WriteImage(imgReduced, "png", 0, path);

                    rectangle.Dispose();
                }
                else
                {
                    imgReduced = sourceImg;
                    HOperatorSet.WriteImage(imgReduced, "png", 0, path);
                    imgReduced.Dispose();
                }
            }
            catch (HalconException hEx)
            {
                MessageBox.Show(hEx.Message, "SaveImg");
            }
        }
        public void DispImage(ImgOperater img, HTuple wndID, HObject sourceImg, bool isOperateImg, ref bool isHaveImg)
        {
            try
            {
                int top, left, right, bottom;
                HOperatorSet.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(wndID);
                HOperatorSet.SetSystem("flush_graphic", "true");

                HTuple width, height;
                HOperatorSet.GetImageSize(sourceImg, out width, out height);
                HOperatorSet.SetSystem("width", width);
                HOperatorSet.SetSystem("height", height);
                if( !isOperateImg )
                {
                    top = left = 0;
                    IImgHeight = right = (int)height;
                    IImgWidth = bottom = (int)width;
                    HOperatorSet.SetPart(wndID, top, left, right, bottom);
                    img.GetImage(sourceImg);
                }
                else
                {
                    HOperatorSet.SetPart(wndID, img.ITop, img.ILeft, img.IBottom, img.IRight);
                }
                IImgCenterX = width / 2;
                IImgCenterY = height / 2;

                HOperatorSet.DispObj(sourceImg, wndID);
                isHaveImg = true;

            }
            catch (HalconException hEx)
            {
                //ShowMsg("DispImage：" + hEx.Message);
            }
        }
        public void DisplayCross(HTuple wndID, double minScale, bool isScale)
        {
            try
            {
                if (wndID == null)
                {
                    return;
                }
                HOperatorSet.SetColor(wndID, "green");
                HOperatorSet.SetDraw(wndID, "margin");

                HOperatorSet.DispCircle(wndID, IImgCenterY, IImgCenterX, IBoxC);

                if (isScale)
                {
                    //画出刻度
                    DrawScale(wndID, this.IImgHeight, IImgWidth, minScale, 0.1, 0.1);
                }
                else
                {
                    HOperatorSet.DispLine(wndID, IImgHeight / 2, 0, IImgHeight / 2, IImgWidth );
                    HOperatorSet.DispLine(wndID, 0, IImgWidth / 2, IImgHeight, IImgWidth / 2);
                }
            }
            catch (HalconException hEx)
            {
                ShowMsg("DisplayCross：" + hEx.Message);
                MessageBox.Show(hEx.Message, "DisplayCross");
            }
        }
        public bool DarwICBox(HTuple wndID)
        {
            try
            {
                HOperatorSet.SetDraw(wndID, "margin");
                HOperatorSet.SetColor(wndID, "green");

                if (ArrBoxW[0] > 0 && ArrBoxH[0] > 0)
                {
                    HOperatorSet.DispRectangle1(wndID, IImgCenterY - ArrBoxH[0], IImgCenterX - ArrBoxW[0], IImgCenterY + ArrBoxH[0], IImgCenterX + ArrBoxW[0]);
                }
                HOperatorSet.SetColor(wndID, "blue");
                if (ArrBoxW[1] > 0 && ArrBoxH[1] > 0)
                {
                    HOperatorSet.DispRectangle1(wndID, IImgCenterY - ArrBoxH[1], IImgCenterX - ArrBoxW[1], IImgCenterY + ArrBoxH[1], IImgCenterX + ArrBoxW[1]);
                }
                return true;
            }
            catch (HalconException hEx)
            {
                ShowMsg("DarwICBox：" + hEx.Message);
            }
            return false;
        }
        private void DrawScale(HTuple wndID, int width2, int height2, double minScale, double resX, double resY)
        {
            try
            {
                Point cenPoint = new Point();
                cenPoint.X = width2 / 2;
                cenPoint.Y = height2 / 2;

                int x1, y1, x2, y2;
                x1 = 0;
                y1 = cenPoint.Y;
                x2 = width2;
                y2 = cenPoint.Y;
                HOperatorSet.DispLine( wndID, x1, y1, x2, y2 );

                x1 = cenPoint.X; y1 = 0;
                x2 = cenPoint.X; y2 = height2;
                HOperatorSet.DispLine( wndID, x1, y1, x2, y2 );

                double xpixSpan_Small = (1 / minScale) * resX * 50;
                double ypixSpan_Small = (1 / minScale) * resY * 50;
                int smallspanlinelen = 5;
                int midspanlinelen = 5;
                //int bigspanlinelen = 8;


                //bool btext = false;
                for (int i = 1; i < width2 / xpixSpan_Small; i++)
                {
                    int lineLen = smallspanlinelen;
                    if (i % 10 == 0)
                    {
                        lineLen += midspanlinelen;
                    }
                    else if (i % 5 == 0)
                        lineLen += midspanlinelen;

                    int x_left = (int)(cenPoint.X - i * xpixSpan_Small);
                    int x_right = (int)(cenPoint.X + i * xpixSpan_Small);

                    if(x_left >= 0)
                    {
                        HOperatorSet.DispLine( wndID, x_left, cenPoint.Y - smallspanlinelen,
                            x_left, cenPoint.Y - smallspanlinelen - lineLen + 1);
                    }

                    if (x_right < width2)
                    {
                        HOperatorSet.DispLine( wndID, x_right, cenPoint.Y - smallspanlinelen,
                            x_right, cenPoint.Y - smallspanlinelen + lineLen + 1);
                    }

                    int y_top = (int)(cenPoint.Y - i * ypixSpan_Small);
                    int y_bottom = (int)(cenPoint.Y + i * ypixSpan_Small);
                    if (y_top >= 0 )
                    {
                        HOperatorSet.DispLine( wndID, cenPoint.X + smallspanlinelen, y_top, 
                            cenPoint.X + smallspanlinelen - lineLen - 1, y_top);
                    }
                    if (y_bottom < height2)
                    {
                        HOperatorSet.DispLine(wndID, cenPoint.X + smallspanlinelen, y_bottom,
                            cenPoint.X + smallspanlinelen - lineLen - 1, y_bottom);
                    }
                }

            }
            catch(HalconException hEx)
            {
                ShowMsg("DrawScale：" + hEx.Message);
            }
        }

        public void DispRectangle1(HTuple wndID, HTuple row2, HTuple column2,  HTuple len11, HTuple len22, String color = "blue")
        {
            HOperatorSet.SetColor(wndID, color);
            HOperatorSet.DispRectangle1(wndID, row2, column2, len11, len22);
        }

        public bool RepDarwBoxNoPhi(HTuple wndID,ref HTuple row2, ref HTuple column2, ref HTuple len11, ref HTuple len22, String color = "blue", String disp = "", bool IsOffset = false)
        {
            HObject rectangle = null;
            HTuple row = new HTuple(), column = new HTuple(), len1 = new HTuple(), len2 = new HTuple(), rectangleArea = new HTuple(), rectangleX = new HTuple(), rectangleY = new HTuple();
            try
            {

                DrawTxt( wndID, 20, 20, disp, "red");

                HOperatorSet.SetColor(wndID, color);
                HOperatorSet.DrawRectangle1Mod(wndID, row2, column2, len11, len22, out row, out column, out len1, out len2);
                if (row.D == 0)
                {
                    row = row2;
                    column = column2;
                    len1 = len11;
                    len2 = len22;
                }
                HOperatorSet.GenRectangle1(out rectangle, row, column, len1, len2);
                HOperatorSet.AreaCenter(rectangle, out rectangleArea, out rectangleX, out rectangleY);//IImgCenterX
                rectangle.Dispose();
                //HOperatorSet.MoveRegion(rectangle, out rectangleMove, row + (IImgCenterX - rectangleX), column + (IImgCenterY - rectangleY));
                if (IsOffset)
                {
                    row2 = row + (IImgCenterY - rectangleX);
                    column2 = column + (IImgCenterX - rectangleY);
                    len11 = len1 + (IImgCenterY - rectangleX);
                    len22 = len2 + (IImgCenterX - rectangleY);
                }
                else
                {
                    row2 = row;
                    column2 = column;
                    len11 = len1;
                    len22 = len2;
                }
                rectangle?.Dispose();
                row?.Dispose();
                column?.Dispose();
                len1?.Dispose();
                len2?.Dispose();
                rectangleArea?.Dispose();
                rectangleX?.Dispose();
                rectangleY?.Dispose();
                return true;
            }
            catch (HalconException hEx)
            {
                rectangle?.Dispose();
                row?.Dispose();
                column?.Dispose();
                len1?.Dispose();
                len2?.Dispose();
                rectangleArea?.Dispose();
                rectangleX?.Dispose();
                rectangleY?.Dispose();
                ShowMsg("RepDarwBoxNoPhi：" + hEx.Message);
            }
            return false;
        }
        public void DrawTxt(HTuple wndID, int posY, int posX, string txt, string color)
        {
            try
            {
                HOperatorSet.SetColor( wndID, color);
                HOperatorSet.SetTposition(wndID, posY, posX);
                HOperatorSet.WriteString(wndID, txt);
            }
            catch (HalconException hEx)
            {
                MessageBox.Show(hEx.Message, "DrawTxt");
            }
        }
        public void DrawCross(HTuple wndID, int row, int colum, double ang, int len, string color)
        {
            try
            {
                HOperatorSet.SetColor(wndID, color);
                HOperatorSet.SetDraw(wndID, "margin");
                for (int i = 0; i < row; i++)
                {
                    HOperatorSet.GenCrossContourXld(out cross, row, colum, len, ang);
                    HOperatorSet.DispObj(cross, wndID);
                }
            }
            catch (HalconException ex)
            {
                MessageBox.Show(ex.Message, "DrawCross");
            }
        }
        public void LoadImgAndDisp(HTuple wndID, HObject sourceImg, String path)
        {
            try
            {

            }
            catch (HalconException hEx)
            {
                MessageBox.Show(hEx.Message, "LoadImgAndDisp");
            }

        }
        public bool FindModel(bool isDisp, HTuple wndID, HObject sourceImg, HTuple modelID, HTuple row2, HTuple colum2, HTuple phi2, HTuple len1, HTuple len2, HTuple certaintyThreshold, HTuple modecount, out HTuple row1, out HTuple cloum1, out HTuple angle, out HTuple score)
        {
            HObject rectangle = null;
            try
            {
                HOperatorSet.GenRectangle1(out rectangle, row2, colum2, len1, len2);
                bool ret = FindModel(isDisp, wndID, sourceImg, modelID, rectangle, certaintyThreshold, modecount, out row1, out cloum1, out angle, out score);
                rectangle?.Dispose();
                if (ret)
                {
                    //HTuple y = row1;
                    //HTuple x = cloum1;
                    //HTuple ang = angle;
                    //HTuple score2 = score;

                    String str = String.Format("Y:{0:f1},X:{1:f1},A:{2:f1},Score:{3:f1}", row1.D, cloum1.D, angle.D, score.D);
                    DrawTxt(wndID, 50, 50, str, "green");
                }

                return ret;
            }
            catch (HalconException hEx)
            {
                rectangle?.Dispose();
                MessageBox.Show(hEx.Message, "FindModel");
            }
            row1 = cloum1 = angle = score = null;
            return false;
        }
        public bool FindModel(bool isDisp, HTuple wndID, HObject sourceImg, HTuple modelID, HObject rectangle, HTuple certaintyThreshold, HTuple modelcount, out HTuple row1, out HTuple cloumn1, out HTuple angle, out HTuple score)
        {
            HObject imageReduced = null;
            HTuple Angle2 = new HTuple();
            HTuple angStar = (new HTuple(-30)).TupleRad();
            HTuple angExt = (new HTuple(60)).TupleRad();
            try
            {
                HOperatorSet.ReduceDomain(sourceImg, rectangle, out imageReduced);

                HOperatorSet.FindShapeModel(imageReduced, modelID, angStar, angExt,
                                            certaintyThreshold, modelcount, 0.2, "least_squares", 0, 0.3, out row1, out cloumn1, out Angle2, out score);
                imageReduced.Dispose();
                int numMatches = row1.TupleLength();           
                if (numMatches > 0)
                {
                    if (isDisp)
                    {
                        Dev_Display_Shap_Mathching_Results(wndID, modelID, "green", row1, cloumn1, Angle2, 1, 1, 0);

                        HOperatorSet.GenCrossContourXld(out cross, row1, cloumn1, 30, (new HTuple(45)).TupleRad());
                        HOperatorSet.DispObj(cross, wndID);
                        cross.Dispose();
                    }
                    HOperatorSet.TupleDeg(Angle2, out angle);
                    Angle2?.Dispose();
                    if (angle < -90) angle = 180 + angle;
                    angle = -angle;
                    return true;
                }
                Angle2?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
            }
            catch (HalconException hEx)
            {
                imageReduced?.Dispose();
                Angle2?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
                if (hEx.GetErrorCode() == 9400)
                {
                    MessageBox.Show("查找目标出现异常5");
                }
                else
                {
                    MessageBox.Show("查找目标出现异常6");
                }
            }
            row1 = cloumn1 = angle = score = null;

            return false;
        }
        public bool FindModelPar(bool isDisp, HTuple wndID, HObject sourceImage, HTuple modelID, HTuple row2, HTuple column2, HTuple phi2, HTuple len11, HTuple len22, HTuple certaintyThreshold, HTuple angle11, HTuple maxOverLap, HTuple greediness, HTuple modelCount, out HTuple row1, out HTuple column1, out HTuple angle, out HTuple score)
        {
            HObject rect = null;
            try
            {
                //产生模板查找区域
                HOperatorSet.GenRectangle1(out rect, row2, column2, len11, len22);
                bool retBool = FindModelPar(isDisp, wndID, sourceImage, modelID, rect, certaintyThreshold, angle11, maxOverLap, greediness, modelCount, out row1, out column1, out angle, out score);
                
                if (retBool)
                {
                    //HTuple Y = row1;
                    //HTuple X = column1;
                    //HTuple A = angle;
                    //HTuple core = score; 
                    //String str = String.Format("Y:{0:f1},X:{1:f1},A:{2:f1},Score:{3:f1}", Y.D, X.D, A.D, core.D);
                    //DrawTxt(wndID, 50, 50, str, "green");
                }
                rect?.Dispose();
                //HOperatorSet.ClearAllShapeModels();
                return retBool;
            }
            catch (HalconException hEx)
            {
                rect?.Dispose();
                MessageBox.Show(hEx.Message, "FindModelPar");
                row1 = column1 = angle = score = null;
                return false;
            }
            
        }
        public bool FindModelPar(bool isDisp, HTuple wndID, HObject sourceImage, HTuple modelID, HObject rect, HTuple CertaintyThreshold, HTuple angle2, HTuple maxOverLap, HTuple greediness, HTuple modelCount, out HTuple row1, out HTuple column1, out HTuple angle, out HTuple score)
        {
            HObject imageReduced = null, ImageScaled = null;
            HTuple Min = new HTuple(), Max = new HTuple(), Range = new HTuple(), mult = new HTuple(), add = new HTuple(), temAngle = new HTuple();
            HTuple angStar = (new HTuple(-1 * angle2)).TupleRad();
            HTuple angExt = (new HTuple(2 * angle2)).TupleRad();
            try
            {
                HOperatorSet.MinMaxGray(rect, sourceImage, 2, out Min, out Max, out Range);
                mult = 255.0 / (Max - Min) + 0.3;
                add = (-mult) * Min - 60;
                HOperatorSet.ScaleImage(sourceImage, out ImageScaled, mult, add);
                HOperatorSet.ReduceDomain(ImageScaled, rect, out imageReduced);
                ImageScaled.Dispose();
                
                HOperatorSet.FindShapeModel(imageReduced, modelID, angStar, angExt,
                    CertaintyThreshold, modelCount, maxOverLap, "least_squares", 0, greediness, out row1, out column1, out temAngle, out score);
                
                int numMatches = row1.TupleLength();
                if (numMatches > 0)
                {
                    if (isDisp)
                    {
                        Dev_Display_Shap_Mathching_Results(wndID, modelID, "green", row1, column1, temAngle, 1, 1, 0);
                        HOperatorSet.GenCrossContourXld(out cross, row1, column1, 30, (new HTuple(45)).TupleRad());
                        HOperatorSet.DispObj(cross, wndID);
                    }
                    HOperatorSet.TupleDeg(temAngle, out angle);
                    if (angle < -90) angle = 180 + angle;
                    angle = -(angle);
                    
                    return true;
                }

                imageReduced?.Dispose();
                ImageScaled?.Dispose();
                Min?.Dispose();
                Max?.Dispose();
                Range?.Dispose();
                mult?.Dispose();
                add?.Dispose();
                temAngle?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
            }
            catch(HalconException hEx)
            {
                imageReduced?.Dispose();
                ImageScaled?.Dispose();
                Min?.Dispose();
                Max?.Dispose();
                Range?.Dispose();
                mult?.Dispose();
                add?.Dispose();
                temAngle?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
                ShowMsg("FindModelPar：" + hEx.Message);
                //if (hEx.GetErrorCode() == 9400)
                //{
                //    MessageBox.Show("查找目标出现异常5");
                //}
                //else
                //{
                //    MessageBox.Show("查找目标出现异常6");
                //}
            }
            row1 = column1 = angle = score = null;
            
            return false;
        }
        public void ModelSaveToFile( HObject sourceImg, HTuple row2, HTuple column2, HTuple len11, HTuple len22, String fileName, int ind, out HTuple modelID)
        {
            HObject rect = null;
            HObject imageReduced = null, imageReduced2 = null;
            HTuple imgW = new HTuple(), imgH = new HTuple(), tem_modelID = new HTuple();
            HTuple angStar = (new HTuple(-30)).TupleRad();
            HTuple angExt = (new HTuple(60)).TupleRad();
            try
            {
                HOperatorSet.GenRectangle1(out rect, row2, column2, len11, len22);
                HOperatorSet.ReduceDomain(sourceImg, rect, out imageReduced);
                rect.Dispose();

                HOperatorSet.CreateShapeModel(imageReduced, "auto", angStar, angExt, "auto", "auto", "use_polarity"/*'ignore_local_polarity */, "auto", "auto", out modelID);
                //modelID = tem_modelID;
                String path;
                path = fileName + "\\set_" + ind + ".shp";
                HOperatorSet.WriteShapeModel(modelID, path);
                path = fileName + "\\set_" + ind + ".jpg";
                HOperatorSet.CropDomain(imageReduced, out imageReduced2);
                HOperatorSet.WriteImage(imageReduced2, "jpeg", 0, path);
                
                rect?.Dispose();
                imageReduced?.Dispose();
                imageReduced2?.Dispose();
                imgW?.Dispose();
                imgH?.Dispose();
                tem_modelID?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
            }
            catch (HalconException hEx)
            {
                rect?.Dispose();
                imageReduced?.Dispose();
                imageReduced2?.Dispose();
                imgW?.Dispose();
                imgH?.Dispose();
                tem_modelID?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
                MessageBox.Show(hEx.Message, "ModelSaveToFile");
                modelID = new HTuple();
            }
        }
        public void ModelSaveToFilePar(HObject sourceImg, HTuple row2, HTuple column2, HTuple len11, HTuple len22, HTuple angle, HTuple contrast, HTuple minContrast, String fileName, int ind, ref HTuple modelID_1, ref HTuple modelID_2, ref HTuple modelID_3, ref HTuple modelID_4)
        {
            short flag = 0;
            int panAngle = 0;
            HObject rectAngle = null, ImageScaled = null;
            HObject ImageAffineTrans = null, RegionAffineTrans = null, imgReduced = null, imgReduced2 = null;
            HTuple temModelID = new HTuple(), HomMat2D = new HTuple(), HomMat2DRotate = new HTuple();
            HTuple Area = new HTuple(), Row = new HTuple(), Column = new HTuple();
            HTuple Min = new HTuple(), Max = new HTuple(), Range = new HTuple(), mult = new HTuple(), add = new HTuple();
            HTuple angStar = (new HTuple(-1 * angle)).TupleRad();
            HTuple angExt = (new HTuple(2 * angle)).TupleRad();
            try
            {
                

                HOperatorSet.GenRectangle1(out rectAngle, row2, column2, len11, len22);
                HOperatorSet.MinMaxGray(rectAngle, sourceImg, 2, out Min, out Max, out Range);
                mult = 255.0 / (Max - Min) + 0.3;
                add = (-mult) * Min - 60;
                HOperatorSet.ScaleImage(sourceImg, out ImageScaled, mult, add);
                HOperatorSet.ReduceDomain(ImageScaled, rectAngle, out imgReduced);

                HOperatorSet.AreaCenter(rectAngle, out Area, out Row, out Column);
                HOperatorSet.HomMat2dIdentity(out HomMat2D);

                //创建图像识别模板
                __CreateShapeModel:
                HOperatorSet.CreateShapeModel(imgReduced, "auto", angStar, angExt, "auto", "auto",
                    "use_polarity", contrast, minContrast, out temModelID);

                string path;
                path = fileName + "\\set_" + ind + ".shp";
                HOperatorSet.WriteShapeModel(temModelID, path);
                path = fileName + "\\set_" + ind + ".jpg";
                HOperatorSet.CropDomain(imgReduced, out imgReduced2);
                HOperatorSet.WriteImage(imgReduced2, "jpeg", 0, path);
                
                if (flag == 0)
                {
                    panAngle = 90;
                    ind++;
                    flag = 1;
                    modelID_1 = temModelID;
                    goto __BackModel;
                }
                else if (flag == 1)
                {
                    panAngle = -90;
                    ind++;
                    flag = 2;
                    modelID_2 = temModelID;
                    goto __BackModel;
                }
                else if (flag == 2)
                {
                    panAngle = 180;
                    ind++;
                    flag = 3;
                    modelID_3 = temModelID;
                    goto __BackModel;
                }
                else if (flag == 3)
                {
                    modelID_4 = temModelID;
                    goto __End;
                }

            //后拍模板
            __BackModel:
                HOperatorSet.HomMat2dRotate(HomMat2D, (new HTuple(panAngle)).TupleRad(), Row, Column, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(ImageScaled, out ImageAffineTrans, HomMat2DRotate, "constant", "false");
                HOperatorSet.AffineTransRegion(rectAngle, out RegionAffineTrans, HomMat2DRotate, "nearest_neighbor");
                HOperatorSet.ReduceDomain(ImageAffineTrans, RegionAffineTrans, out imgReduced);
                ImageAffineTrans.Dispose();
                RegionAffineTrans.Dispose();
                goto __CreateShapeModel;

                __End:
                rectAngle?.Dispose();
                ImageScaled?.Dispose();
                ImageAffineTrans?.Dispose();
                RegionAffineTrans?.Dispose();
                imgReduced?.Dispose();
                imgReduced2?.Dispose();
                temModelID?.Dispose();
                HomMat2D?.Dispose();
                HomMat2DRotate?.Dispose();
                Area?.Dispose();
                Row?.Dispose();
                Column?.Dispose();
                Min?.Dispose();
                Max?.Dispose();
                Range?.Dispose();
                mult?.Dispose();
                add?.Dispose();
                HOperatorSet.ClearAllShapeModels();
            }
            catch (HalconException hEx)
            {
                MessageBox.Show(hEx.Message, "ModelSaveToFilePar_2");
                modelID_1 = null;
                modelID_2 = null;
                modelID_3 = null;
                rectAngle?.Dispose();
                ImageScaled?.Dispose();
                ImageAffineTrans?.Dispose();
                RegionAffineTrans?.Dispose();
                imgReduced?.Dispose();
                imgReduced2?.Dispose();
                temModelID?.Dispose();
                HomMat2D?.Dispose();
                HomMat2DRotate?.Dispose();
                Area?.Dispose();
                Row?.Dispose();
                Column?.Dispose();
                Min?.Dispose();
                Max?.Dispose();
                Range?.Dispose();
                mult?.Dispose();
                add?.Dispose();
            }

        }

        /// <summary>
        /// 查找NCC模板
        /// </summary>
        public bool NCCModelPar(bool isDisp, HTuple wndID, HObject sourceImage, HTuple modelID, HTuple row2, HTuple column2, HTuple phi2, HTuple len11, HTuple len22, HTuple certaintyThreshold, HTuple angle11, HTuple maxOverLap, HTuple modelCount, out HTuple row1, out HTuple column1, out HTuple angle, out HTuple score)
        {
            HObject rect = null;
            try
            {
                //产生模板查找区域
                //HOperatorSet.SetSystem("cudnn_deterministic", "true");
                HOperatorSet.GenRectangle1(out rect, row2, column2, len11, len22);
                bool retBool = NCCModelPar(isDisp, wndID, sourceImage, modelID, rect, certaintyThreshold, angle11, maxOverLap, modelCount, out row1, out column1, out angle, out score);
                rect?.Dispose();
                if (retBool)
                {
                    //HTuple Y = row1;
                    //HTuple X = column1;
                    //HTuple A = angle;
                    //HTuple core = score;
                    //String str = String.Format("Y:{0:f1},X:{1:f1},A:{2:f1},Score:{3:f1}", Y.D, X.D, A.D, core.D);
                    //DrawTxt(wndID, 50, 50, str, "green");
                }
                return retBool;
            }
            catch (HalconException hEx)
            {
                rect?.Dispose();
                MessageBox.Show(hEx.Message, "NCCModelPar");
                row1 = column1 = angle = score = null;
                return false;
            }
            
        }
        private bool NCCModelPar(bool isDisp, HTuple wndID, HObject sourceImage, HTuple modelID, HObject rect, HTuple CertaintyThreshold, HTuple angle2, HTuple maxOverLap, HTuple modelCount, out HTuple row1, out HTuple column1, out HTuple angle, out HTuple score)
        {
            HObject imageReduced = null;
            HTuple temAngle = new HTuple();
            HTuple angStar = (new HTuple(-1 * angle2)).TupleRad();
            HTuple angExt = (new HTuple(2 * angle2)).TupleRad();
            try
            {
                //HOperatorSet.MinMaxGray(rect, sourceImage, 2, out Min, out Max, out Range);
                //mult = 255.0 / (Max - Min) + 0.3;
                //add = (-mult) * Min - 60;
                //HOperatorSet.ScaleImage(sourceImage, out ImageScaled, mult, add);
                //ImageScaled.Dispose();
                HOperatorSet.ReduceDomain(sourceImage, rect, out imageReduced);

                HOperatorSet.FindNccModel(imageReduced, modelID, angStar, angExt,
                    CertaintyThreshold, modelCount, maxOverLap, "true", 0, out row1, out column1, out temAngle, out score);
                imageReduced.Dispose();
                int numMatches = row1.TupleLength();
                if (numMatches > 0)
                {
                    if (isDisp)
                    {
                        Dev_Display_NCC_Mathching_Results(wndID, modelID, "green", row1, column1, temAngle, 1, 1, 0);
                        HOperatorSet.DispArrow(wndID, row1, column1, row1 - temAngle.TupleSin() * 200, column1 + temAngle.TupleCos() * 100, 5);
                        HOperatorSet.GenCrossContourXld(out cross, row1, column1, 30, (new HTuple(45)).TupleRad());
                        HOperatorSet.DispObj(cross, wndID);
                    }
                    HOperatorSet.TupleDeg(temAngle, out angle);
                    temAngle.Dispose();
                    if (angle < -90) angle = 180 + angle;
                    angle = -(angle);
                    return true;
                }

                imageReduced?.Dispose();
                temAngle?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
            }
            catch (HalconException hEx)
            {
                imageReduced?.Dispose();
                temAngle?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
                ShowMsg("NCCModelPar：" + hEx.Message);
                //if (hEx.GetErrorCode() == 9400)
                //{
                //    MessageBox.Show("查找目标出现异常5");
                //}
                //else
                //{
                //    MessageBox.Show("查找目标出现异常6");
                //}
            }
            row1 = column1 = angle = score = null;

            return false;
        }

        public void NCCModelSaveToFile(HObject sourceImg, HTuple row2, HTuple column2, HTuple len11, HTuple len22, HTuple row1, HTuple column1, HTuple len13, HTuple len14, HTuple angle, String fileName, int ind, ref HTuple modelID)
        {
            HObject rect1 = null, rect2 = null, rgM = null;
            HObject imageReduced = null, imageReduced2 = null;
            HTuple tem_modelID = new HTuple();
            HTuple angStar = (new HTuple(-1 * angle)).TupleRad();
            HTuple angExt = (new HTuple(2 * angle)).TupleRad();
            try
            {
                HOperatorSet.GenRectangle1(out rect1, row2, column2, len11, len22);
                HOperatorSet.GenRectangle1(out rect2, row1, column1, len13, len14);
                HOperatorSet.Difference(rect1, rect2, out rgM);
                HOperatorSet.ReduceDomain(sourceImg, rgM, out imageReduced);
                rgM.Dispose();

                
                HOperatorSet.CreateNccModel(imageReduced, "auto", angStar, angExt, "auto", "use_polarity", out modelID);
                //modelID = tem_modelID;
                //tem_modelID = null;
                String path;
                path = fileName + "\\set_" + ind + ".shp";
                HOperatorSet.WriteNccModel(modelID, path);
                path = fileName + "\\set_" + ind + ".jpg";
                HOperatorSet.CropDomain(imageReduced, out imageReduced2);
                HOperatorSet.WriteImage(imageReduced2, "jpeg", 0, path);
                
                rect1?.Dispose();
                rect2?.Dispose();
                rgM?.Dispose();
                imageReduced?.Dispose();
                imageReduced2?.Dispose();
                tem_modelID?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
                return;
            }
            catch (HalconException hEx)
            {
                rect1?.Dispose();
                rect2?.Dispose();
                rgM?.Dispose();
                imageReduced?.Dispose();
                imageReduced2?.Dispose();
                tem_modelID?.Dispose();
                angStar?.Dispose();
                angExt?.Dispose();
                MessageBox.Show(hEx.Message, "NCCModelSaveToFile");
                //modelID = new HTuple();
            }
        }

        private void Dev_Display_Shap_Mathching_Results(HTuple wndID, HTuple modelID, HTuple color, HTuple row, HTuple column, HTuple angle, HTuple scaleR, HTuple scaleC, HTuple model)
        {
            HObject modelContours = null, contoursAffinTrans = null;
            int numMatches, ind, match;
            HTuple homMat2DIdentity = new HTuple(), homMat2DScale = new HTuple(), homMat2DRotate = new HTuple(), HomMat2DTranslate = new HTuple();
            try
            {
                numMatches = row.TupleLength();
                if (numMatches > 0)
                {
                    if (scaleR.TupleNumber() == 1)
                    {
                        HOperatorSet.TupleGenConst(numMatches, scaleR, out scaleR);
                    }
                    if (scaleC.TupleNumber() == 1)
                    {
                        HOperatorSet.TupleGenConst(numMatches, scaleC, out scaleC);
                    }
                    else if (model.TupleNumber() == 0)
                    {
                        HOperatorSet.TupleGenConst(numMatches, 0, out model);
                    }
                    for (ind = 0; ind <= modelID.TupleLength() - 1; ind++)
                    {
                        HOperatorSet.GetShapeModelContours(out modelContours, (HTuple)modelID[ind], 1);
                        //设定画的颜色
                        HOperatorSet.SetColor(wndID, color);

                        for (match = 0; match <= numMatches - 1; match++)
                        {
                            if (ind == ((HTuple)(model[match])).TupleNumber())
                            {
                                HOperatorSet.HomMat2dIdentity(out homMat2DIdentity);

                                HOperatorSet.HomMat2dScale(homMat2DIdentity, (HTuple)(scaleR[match]), (HTuple)(scaleC[match]), 0, 0, out homMat2DScale);

                                HOperatorSet.HomMat2dRotate(homMat2DScale, (HTuple)(angle[match]), 0, 0, out homMat2DRotate);

                                HOperatorSet.HomMat2dTranslate(homMat2DRotate, (HTuple)(row[match]), (HTuple)(column[match]), out HomMat2DTranslate);

                                HOperatorSet.AffineTransContourXld(modelContours, out contoursAffinTrans, HomMat2DTranslate);

                                HOperatorSet.DispObj(contoursAffinTrans, wndID);
                                
                            }
                        }
                    }
                }
                modelContours?.Dispose();
                contoursAffinTrans?.Dispose();
                homMat2DIdentity?.Dispose();
                homMat2DScale?.Dispose();
                homMat2DRotate?.Dispose();
                HomMat2DTranslate?.Dispose();
            }
            catch (HalconException hEx)
            {
                modelContours?.Dispose();
                contoursAffinTrans?.Dispose();
                homMat2DIdentity?.Dispose();
                homMat2DScale?.Dispose();
                homMat2DRotate?.Dispose();
                HomMat2DTranslate?.Dispose();
                MessageBox.Show(hEx.Message, "Dev_Display_Shap_Mathching_Results");
            }
        }

        private void Dev_Display_NCC_Mathching_Results(HTuple wndID, HTuple modelID, HTuple color, HTuple row, HTuple column, HTuple angle, HTuple scaleR, HTuple scaleC, HTuple model)
        {
            HObject modelContours = null, contoursAffinTrans = null;
            int numMatches, ind, match;
            HTuple homMat2DIdentity = new HTuple(), homMat2DScale = new HTuple(), homMat2DRotate = new HTuple(), HomMat2DTranslate = new HTuple();
            try
            {
                //HObject modelContours, contoursAffinTrans;
                //int numMatches, ind, match;
                //HTuple homMat2DIdentity, homMat2DScale, homMat2DRotate, HomMat2DTranslate;

                numMatches = row.TupleLength();
                if (numMatches > 0)
                {
                    if (scaleR.TupleNumber() == 1)
                    {
                        HOperatorSet.TupleGenConst(numMatches, scaleR, out scaleR);
                    }
                    if (scaleC.TupleNumber() == 1)
                    {
                        HOperatorSet.TupleGenConst(numMatches, scaleC, out scaleC);
                    }
                    else if (model.TupleNumber() == 0)
                    {
                        HOperatorSet.TupleGenConst(numMatches, 0, out model);
                    }
                    for (ind = 0; ind <= modelID.TupleLength() - 1; ind++)
                    {
                        HOperatorSet.GetNccModelRegion(out modelContours, (HTuple)modelID[ind]);
                        //设定画的颜色
                        HOperatorSet.SetColor(wndID, color);

                        for (match = 0; match <= numMatches - 1; match++)
                        {
                            if (ind == ((HTuple)(model[match])).TupleNumber())
                            {
                                HOperatorSet.HomMat2dIdentity(out homMat2DIdentity);

                                HOperatorSet.HomMat2dScale(homMat2DIdentity, (HTuple)(scaleR[match]), (HTuple)(scaleC[match]), 0, 0, out homMat2DScale);

                                HOperatorSet.HomMat2dRotate(homMat2DScale, (HTuple)(angle[match]), 0, 0, out homMat2DRotate);

                                HOperatorSet.HomMat2dTranslate(homMat2DRotate, (HTuple)(row[match]), (HTuple)(column[match]), out HomMat2DTranslate);

                                HOperatorSet.AffineTransRegion(modelContours, out contoursAffinTrans, HomMat2DTranslate, "constant");//nearest_neighbor

                                HOperatorSet.DispObj(contoursAffinTrans, wndID);

                                CCD.ImageXld[3] = contoursAffinTrans;

                                contoursAffinTrans.Dispose();
                            }
                        }
                    }
                }
                modelContours?.Dispose();
                contoursAffinTrans?.Dispose();
                homMat2DIdentity?.Dispose();
                homMat2DScale?.Dispose();
                homMat2DRotate?.Dispose();
                HomMat2DTranslate?.Dispose();
            }
            catch (HalconException hEx)
            {
                modelContours?.Dispose();
                contoursAffinTrans?.Dispose();
                homMat2DIdentity?.Dispose();
                homMat2DScale?.Dispose();
                homMat2DRotate?.Dispose();
                HomMat2DTranslate?.Dispose();
                MessageBox.Show(hEx.Message, "Dev_Display_NCC_Mathching_Results");
            }
        }
    }
}
