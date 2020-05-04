using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.Controls.ImageView
{
    public class ConstData
    {
        public const int Board_Scale = 5;
        public const int Scale_Num = 15;

        public const int View_Gap = 500;
        public const int LeftDown = 0;//左下角原点
        public const int LeftUp = 1;//左上角原点
        public const int RightDown = 2;//右下角原点
        public const int RightUp = 3;//右上角原点

        public const int Scale_Min = 5;
        public const int Scale_Max = 60;
        public const int Scale_Delta = 5;

        public const decimal dScale_Min = 0.1M;
        public const decimal dScale_Max = 10.0M;
        
        public const int View_Area = 0;//显示area视图
        public const int View_Camera = 1;//显示相机视图

        public const int Show_Normal = 0;//cameraview正常显示
        public const int Show_Grid = 1;//cameraview显示网格

        public const int Tool_None = 0;//
        public const int Tool_Mesure = 1;//

        public const int Mode_Normal = 0;//检测模式，显示概图，不能进行放大缩小测量等操作
        public const int Mode_Edit = 1;//编辑模式
    }

    public class LockedSign
    {
        public static long l2ShowIsCreatingImage = 0;
    }
}
