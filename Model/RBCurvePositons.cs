using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class RBCurvePositons
    {
        public Point OriginRedPoint;
        public Point OriginBluePoint;

        //小图坐标
        public Point CaptureRedPoint;
        public Point CaptureBluePoint;
        public Vector3RGB[] RGBSET;

        public RBCurvePositons(Point originRedPoint, Point originBluePoint, Point captureRedPoint, Point captureBluePoint, Vector3RGB[] RGBdata)
        {
            OriginRedPoint = originRedPoint;
            OriginBluePoint = originBluePoint;
            CaptureRedPoint = captureRedPoint;
            CaptureBluePoint = captureBluePoint;
            this.RGBSET = RGBdata;
        }
    }

    /// <summary>
    /// 如果要 留空 ,请使用 EmptyVal() 方法 初始化 
    /// </summary>
    struct Vector3RGB
    {
        public int R, G, B;
        public bool isEmpty;

        /// <summary>
        /// 如果要 留空 ,请使用 EmptyVal() 方法 初始化 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Vector3RGB(int r, int g, int b)
        {
            isEmpty = false;
            R = r; G = g; B = b;
        }
        public static bool operator ==(Vector3RGB a, Vector3RGB b)
        {

            return a.R == b.R && a.G == b.G && a.B == b.B ? true : false;
        }
        public static bool operator !=(Vector3RGB a, Vector3RGB b)
        {

            return a.R == b.R && a.G == b.G && a.B == b.B ? false : true;
        }

        public static Vector3RGB EmptyVal()
        {

            return new Vector3RGB() { isEmpty = true };
        }

        public override string ToString()
        {

            return String.Format("rgb: {0},{1},{2}", R, G, B);
        }

    }
}
