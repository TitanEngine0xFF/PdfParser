using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commonlib.DrawingExts
{
    public static class BitmapExt
    {
        public static Bitmap CopyBitmap(this Bitmap src)
        {
            Bitmap dest = new Bitmap(src.Width, src.Height);
            using (var g1 = Graphics.FromImage(dest))
            {
                g1.DrawImage(src, 0, 0,src.Width,src.Height);
                g1.Save();
            }
            return dest;
        }
    }
}
