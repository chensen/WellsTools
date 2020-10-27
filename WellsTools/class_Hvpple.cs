using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hvppleDotNet;

namespace Wells
{
    public static class class_Hvpple
    {
        public static HTuple getGrayHisto(HImage image, HTuple rectangle1 = null)
        {
            if (image != null)
            {
                try
                {
                    HTuple hv_AbsoluteHisto, hv_RelativeHisto;

                    HTuple channel = image.CountChannels();
                    HImage imgTmp = null;
                    if (channel == 3)
                    {
                        imgTmp = image.Rgb1ToGray();
                    }
                    else
                    {
                        imgTmp = image.Clone();
                    }
                    HRegion region = new HRegion();
                    if (rectangle1 == null)
                    {
                        HTuple col, row;
                        imgTmp.GetImageSize(out col, out row);
                        region.GenRectangle1(0, 0, row - 1, col - 1);
                    }
                    else
                    {
                        region.GenRectangle1(rectangle1[0].D, rectangle1[1], rectangle1[2], rectangle1[3]);
                    }
                    hv_AbsoluteHisto = imgTmp.GrayHisto(region, out hv_RelativeHisto);
                    return hv_AbsoluteHisto;
                }
                catch (Exception exc)
                {
                    Wells.FrmType.frm_Log.Log("获取灰度直方图出错：" + exc.Message, 2);
                }
            }
            return null;
        }

        public static HTuple getGrayHisto(HObject image, HTuple rectangle1 = null)
        {
            if (image != null)
            {
                try
                {
                    HTuple hv_AbsoluteHisto, hv_RelativeHisto;

                    HOperatorSet.CountChannels(image, out HTuple channel);

                    HObject imgTmp = null;
                    if (channel == 3)
                    {
                        HOperatorSet.Rgb1ToGray(image, out imgTmp);
                    }
                    else
                    {
                        imgTmp = image;
                    }
                    HRegion region = new HRegion();
                    if (rectangle1 == null)
                    {
                        HTuple col, row;
                        HOperatorSet.GetImageSize(imgTmp, out col, out row);
                        region.GenRectangle1(0, 0, row - 1, col - 1);
                    }
                    else
                    {
                        region.GenRectangle1(rectangle1[0].D, rectangle1[1], rectangle1[2], rectangle1[3]);
                    }
                    HOperatorSet.GrayHisto(region, imgTmp, out hv_AbsoluteHisto, out hv_RelativeHisto);
                    return hv_AbsoluteHisto;
                }
                catch (Exception exc)
                {
                    Wells.FrmType.frm_Log.Log("获取灰度直方图出错：" + exc.Message, 2);
                }
            }
            return null;
        }

        public static void dispMessage(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
     HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_GenParamName = null, hv_GenParamValue = null;
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_CoordSystem_COPY_INP_TMP = hv_CoordSystem.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Column: The column coordinate of the desired text position
            //   A tuple of values is allowed to display text at different
            //   positions.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically...
            //   - if |Row| == |Column| == 1: for each new textline
            //   = else for each text position.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow
            //       otherwise -> use given string as color string for the shadow color
            //
            //It is possible to display multiple text strings in a single call.
            //In this case, some restrictions apply:
            //- Multiple text positions can be defined by specifying a tuple
            //  with multiple Row and/or Column coordinates, i.e.:
            //  - |Row| == n, |Column| == n
            //  - |Row| == n, |Column| == 1
            //  - |Row| == 1, |Column| == n
            //- If |Row| == |Column| == 1,
            //  each element of String is display in a new textline.
            //- If multiple positions or specified, the number of Strings
            //  must match the number of positions, i.e.:
            //  - Either |String| == n (each string is displayed at the
            //                          corresponding position),
            //  - or     |String| == 1 (The string is displayed n times).
            //
            //
            //Convert the parameters for disp_text.
            try
            {
                if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                  new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
                {

                    return;
                }
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP = 12;
                }
                //
                //Convert the parameter Box to generic parameters.
                hv_GenParamName = new HTuple();
                hv_GenParamValue = new HTuple();
                if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                    {
                        //Display no box
                        hv_GenParamName = hv_GenParamName.TupleConcat("box");
                        hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                    }
                    else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual("true"))) != 0)
                    {
                        //Set a color other than the default.
                        hv_GenParamName = hv_GenParamName.TupleConcat("box_color");
                        hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(0));
                    }
                }
                if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                    {
                        //Display no shadow.
                        hv_GenParamName = hv_GenParamName.TupleConcat("shadow");
                        hv_GenParamValue = hv_GenParamValue.TupleConcat("false");
                    }
                    else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual("true"))) != 0)
                    {
                        //Set a shadow color other than the default.
                        hv_GenParamName = hv_GenParamName.TupleConcat("shadow_color");
                        hv_GenParamValue = hv_GenParamValue.TupleConcat(hv_Box.TupleSelect(1));
                    }
                }
                //Restore default CoordSystem behavior.
                if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
                {
                    hv_CoordSystem_COPY_INP_TMP = "image";
                }
                //
                if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
                {
                    //disp_text does not accept an empty string for Color.
                    hv_Color_COPY_INP_TMP = new HTuple();
                }
                //
                HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP,
                    hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName,
                    hv_GenParamValue);
            }
            catch(Exception exc)
            {
                Wells.FrmType.frm_Log.Log(exc.Message, 2);
            }

            return;
        }
        
        public static void setDisplayFont(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
     HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_Fonts = new HTuple();
            HTuple hv_Style = null, hv_Exception = new HTuple(), hv_AvailableFonts = null;
            HTuple hv_Fdx = null, hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            try
            {
                if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                {
                    hv_Size_COPY_INP_TMP = 16;
                }
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    //Restore previous behaviour
                    hv_Size_COPY_INP_TMP = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt();
                    if (hv_Size_COPY_INP_TMP < 1)
                        hv_Size_COPY_INP_TMP = 1;
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Courier";
                    hv_Fonts[1] = "Courier 10 Pitch";
                    hv_Fonts[2] = "Courier New";
                    hv_Fonts[3] = "CourierNew";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Consolas";
                    hv_Fonts[1] = "Menlo";
                    hv_Fonts[2] = "Courier";
                    hv_Fonts[3] = "Courier 10 Pitch";
                    hv_Fonts[4] = "FreeMono";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Luxi Sans";
                    hv_Fonts[1] = "DejaVu Sans";
                    hv_Fonts[2] = "FreeSans";
                    hv_Fonts[3] = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Times New Roman";
                    hv_Fonts[1] = "Luxi Serif";
                    hv_Fonts[2] = "DejaVu Serif";
                    hv_Fonts[3] = "FreeSerif";
                    hv_Fonts[4] = "Utopia";
                }
                else
                {
                    hv_Fonts = hv_Font_COPY_INP_TMP.Clone();
                }
                hv_Style = "";
                if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
                {
                    hv_Style = hv_Style + "Bold";
                }
                else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new hvppleException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
                {
                    hv_Style = hv_Style + "Italic";
                }
                else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new hvppleException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
                {
                    hv_Style = "Normal";
                }
                HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
                hv_Font_COPY_INP_TMP = "";
                for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
                {
                    hv_Indices = hv_AvailableFonts.TupleFind(hv_Fonts.TupleSelect(hv_Fdx));
                    if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(0))) != 0)
                    {
                        if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                        {
                            hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_Fdx);
                            break;
                        }
                    }
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
                {
                    throw new hvppleException("Wrong value of control parameter Font");
                }
                hv_Font_COPY_INP_TMP = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
                HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);
                // dev_set_preferences(...); only in hdevelop
            }
            catch(Exception exc)
            {
                Wells.FrmType.frm_Log.Log(exc.Message, 2);
            }

            return;
        }

        public static bool isImage(HObject obj)
        {
            #region 判断是否是真正的图像
            bool ret = false;
            if (obj == null) return ret;
            if (!obj.IsInitialized()) return ret;
            HTuple width, height;
            try
            {
                HOperatorSet.GetImageSize(obj, out width, out height);
                if (width.TupleLength() > 0 && height.TupleLength() > 0)
                    ret = true;
            }
            catch (Exception exc)
            {
                ret = false;
            }
            return ret;
            #endregion
        }

        public static bool isRegion(HObject obj)
        {
            #region 判断是否是真正的region
            bool ret = false;
            if (obj == null) return ret;
            if (!obj.IsInitialized()) return ret;
            HTuple area, col, row;
            try
            {
                HOperatorSet.AreaCenter(obj, out area, out row, out col);
                if (area.TupleLength() > 0 && row.TupleLength() > 0 && col.TupleLength() > 0)
                    ret = true;
            }
            catch (Exception exc)
            {
                ret = false;
            }
            return ret;
            #endregion
        }
    }
}
