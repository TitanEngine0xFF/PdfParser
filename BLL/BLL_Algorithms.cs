
using Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class BLL_Algorithms
    {

        /// <summary>
        /// 测试成功 , 取截图中红,蓝点
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rv"></param>
        /// <param name="bv"></param>
        /// <param name="Yoffset">Y轴补偿</param>
        /// <returns></returns>
        public static Tuple<Point[], Vector3RGB[]> GetCurve2Points(Bitmap bitmap ,int rv ,int bv, int Yoffset=3)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int length = height * 3 * width;
            byte[] RGBArr = new byte[length];
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            System.IntPtr Scan0 = data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(Scan0, RGBArr, 0, length);

            Point[] point2 = new Point[2];
            Vector3RGB[] vrgb = new Vector3RGB[2];
            int rowc = 0;
            int firstPointRowIndx = 0;
            bool redFound = false;
            bool blueFound = false;

            for (int PixelIdx = 0; PixelIdx < RGBArr.Length / 3; PixelIdx += 1)
            {
                //事实证明RGB值在数组中是反过来的。。3个小时 的调试，测试。。。
                if (PixelIdx * 3 >= RGBArr.Length)
                    break;
                if (PixelIdx * 3 + 1 >= RGBArr.Length)
                    break;
                if (PixelIdx * 3 + 2 >= RGBArr.Length)
                    break;

                var b = RGBArr[PixelIdx * 3];
                var g = RGBArr[PixelIdx * 3 + 1];

                var r = RGBArr[PixelIdx * 3 + 2];

                //Vector3RGB isRedCurvePoint = Vector3RGB.EmptyVal();
                //Vector3RGB isBlueCurvePoint = Vector3RGB.EmptyVal();

                //假如 相除 后 小于零 那就是 第一行的 第(余数)颗像素 
                //余数 就是x轴
                //请勿修改 ,2020年9月22号 测试通过
                int x = PixelIdx % (width);
                int y = PixelIdx / (width);

                //找像素 证明对了...
                //if (r == 237 && g == 28 && b == 36)
                //{
                //    point2[0] = new Point(x, y);
                //    goto End;
                //}

                if (true)//用于调试方便 切换用的。。
                {
                    //找红点 , redFound 变量 防止第一个红色坐标被后续的坐标覆盖了。

                    if (!redFound && y>rowc&&r > 240 && g < rv && b < rv)
                    {

                        vrgb[0] = new Vector3RGB(r, g, b);
                        point2[0] = new Point(x, y+ Yoffset);
                        //goto End;
                        rowc = y;
                        redFound = true;    //找饱和度最高的
                    }
                    else if (!blueFound && y>rowc&& b > 240 && r < bv && g < bv)
                    {

                        vrgb[1] = new Vector3RGB(r, g, b);
                        point2[1] = new Point(x, y+ Yoffset);
                        rowc = y;
                        blueFound = true;

                    }


                    //又掉坑了。。哎，半小时。
                    //得先确定 redFound=true ，再确认 redFound是否 跟 blueFound 相等。
                    // redFound==blueFound==true 这是错误的写法，可能是从左边开始判断的。。
                    //最佳写法是 redFound==true==blueFound
                    if (blueFound == true && redFound == blueFound)
                    {
                        goto End;
                    }

                }

            }

        End:
            //读取而已,无需再次写入
            //System.Runtime.InteropServices.Marshal.Copy(RGB, 0, Scan0, length);
            bitmap.UnlockBits(data);
            return new Tuple<Point[], Vector3RGB[]>(point2, vrgb);
        }



        /// <summary>
        /// 测试成功 , 取截图中红,蓝点
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rv"></param>
        /// <param name="bv"></param>
        /// <param name="Yoffset">Y轴补偿</param>
        /// <returns></returns>
        public static Tuple<Point[], Vector3RGB[]> GetCurve2PointsBigClip(Bitmap bitmap, int rv, int bv, int Yoffset = 3)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int length = height * 3 * width;
            byte[] RGBArr = new byte[length];
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            System.IntPtr Scan0 = data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(Scan0, RGBArr, 0, length);

            //p[0] 是红色 , p[1] 是蓝色
            Point[] OutPoint = new Point[2];
            Vector3RGB[] vrgb = new Vector3RGB[2];

            int RedMinX = -1;
            int BlueMinX = -1;
            int RedY = 0;
            int BlueY = 0;
            bool RedFirstFound = true;
            bool BlueFirstFound = true;

            //必须能被3整除否则,格式肯定是有问题或不完整的
            if (RGBArr.Length % 3 == 0)
            {
                for (int PixelIdx = 0; PixelIdx < RGBArr.Length / 3; PixelIdx++)
                {
                    if (PixelIdx == RGBArr.Length / 3 - 1)
                    {
                        break;
                    }

                    var b = RGBArr[PixelIdx * 3];
                    var g = RGBArr[PixelIdx * 3 + 1];
                    var r = RGBArr[PixelIdx * 3 + 2];

                    //Vector3RGB isRedCurvePoint = Vector3RGB.EmptyVal();
                    //Vector3RGB isBlueCurvePoint = Vector3RGB.EmptyVal();

                    //假如 相除 后 小于零 那就是 第一行的 第(余数)颗像素 
                    //余数 就是x轴
                    //请勿修改 ,2020年9月22号 测试通过
                    int x = PixelIdx % (width);
                    int y = PixelIdx / (width);

                    //找像素 证明对了...
                    //if (r == 237 && g == 28 && b == 36)
                    //{
                    //    point2[0] = new Point(x, y);
                    //    goto End;
                    //}




                    if (true)//用于调试方便 切换用的。。
                    {
                        if (r > 240 && g < rv && b < rv)
                        {
                            if (x > 0)
                            {

                                //这个是为了 初始化 最小值 ,得到最小值
                                if (RedFirstFound)
                                {
                                    RedFirstFound = false;
                                    RedMinX = x;  //因为第一次也可能是 X轴最小的红点
                                    RedY = y;
                                    vrgb[0] = new Vector3RGB(r, g, b);
                                }
                                //找到X轴最小的红点像素
                                if (x < RedMinX)
                                {
                                    RedMinX = x;
                                    RedY = y;
                                    vrgb[0] = new Vector3RGB(r, g, b);
                                }

                            }
                        }
                        else if (b > 240 && r < bv && g < bv)
                        {
                            if (x > 0)
                            {
                                //这个是为了 初始化 最小值 ,得到最小值
                                if (BlueFirstFound) //这叫常开门 , 第一次后就自锁了
                                {
                                    BlueFirstFound = false;
                                    BlueMinX = x;   //因为第一次也可能是 X轴最小的蓝点
                                    BlueY = y;
                                    vrgb[1] = new Vector3RGB(r, g, b);
                                }
                                else if (x < BlueMinX)
                                {
                                    BlueMinX = x;
                                    BlueY = y;
                                    vrgb[1] = new Vector3RGB(r, g, b);
                                }
                            }
                        }
                    }
                }
            }



        Error:
            if (RedMinX == -1 || BlueMinX == -1)
            {
                OutPoint[0] = new Point(-1, -1);
                OutPoint[1] = new Point(-1, -1);
                bitmap.UnlockBits(data);
                return new Tuple<Point[], Vector3RGB[]>(OutPoint, vrgb);
            }
            else
            {
                OutPoint[0] = new Point(RedMinX, RedY);
                OutPoint[1] = new Point(BlueMinX, BlueY);
            }
            //读取而已,无需再次写入
            //System.Runtime.InteropServices.Marshal.Copy(RGB, 0, Scan0, length);
            bitmap.UnlockBits(data);
            return new Tuple<Point[], Vector3RGB[]>(OutPoint, vrgb);
        }
    }
}
