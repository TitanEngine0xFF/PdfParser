using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commonlib.Utilities
{
    public class WaterMarkMaker
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="destImg"></param>
    /// <param name="watermarkFilename"></param>
    /// <param name="watermarkTransparency"></param>
    /// <param name="colNum">n/10 表示宽度的十分之一 ,那么水印就有10列 ,10行</param>
        public  static void AddWaterMark(Image destImg, Image watermarkFilename, int watermarkTransparency, double colNum)
        {
            //这样算才正常
            colNum *= 10;

                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float transparency = 0.5F;
                if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                    transparency = (watermarkTransparency / 10.0F);


                float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //如果原图片是索引像素格式之列的，则需要转换
                if (IsPixelFormatIndexed(destImg.PixelFormat))
                {
                    Bitmap bmp = new Bitmap(destImg.Width, destImg.Height, PixelFormat.Format32bppArgb);
                    using (Graphics gg = Graphics.FromImage(bmp))
                    {
                        gg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        gg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        gg.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        gg.DrawImage(destImg, 0, 0);
                    }
                    FillWaterMark(watermarkFilename, colNum, imageAttributes, bmp);

                }
                else
                {
                    FillWaterMark(watermarkFilename, colNum, imageAttributes, (Bitmap) destImg);
                }

        }

        private static void FillWaterMark(Image watermarkFilename, double colNum, ImageAttributes imageAttributes, Bitmap bmp)
        {
            double ws = bmp.Width * 10/ colNum;
            double hs = bmp.Height * 10/ colNum;
            var wimg = GetThumbNailImage(watermarkFilename, int.Parse(Math.Round(ws, 0).ToString()), int.Parse(Math.Round(hs, 0).ToString())); ;

            using (var g = Graphics.FromImage(bmp))
            {

                int w = (int)Math.Round(bmp.Width * 10/ colNum, 0);
                int h = (int)Math.Round(bmp.Height * 10/ colNum, 0);
                for (var x = 0; x < bmp.Width; x += w)
                {
                    for (var y = 0; y < bmp.Height; y += h)
                    {
                        g.DrawImage(wimg, new Rectangle(x, y, wimg.Width, wimg.Height), 0, 0, wimg.Width, wimg.Height, GraphicsUnit.Pixel, imageAttributes);
                        //g.DrawImage(wimg, x, y); 
                    }
                }
                g.Save();
            }
        }

        private static  void ImgagesAddwatermark(string imgPath, string saveDir, string root, string watermarkFilename, int watermarkTransparency, double bl)
        {
            try
            {
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float transparency = 0.5F;
                if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                    transparency = (watermarkTransparency / 10.0F);


                float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                using (Image img = Image.FromFile(imgPath))
                {
                    //如果原图片是索引像素格式之列的，则需要转换
                    if (IsPixelFormatIndexed(img.PixelFormat))
                    {
                        Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                        using (Graphics gg = Graphics.FromImage(bmp))
                        {
                            gg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            gg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            gg.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            gg.DrawImage(img, 0, 0);
                        }
                        double ws = bmp.Width * (bl / 10);
                        double hs = bmp.Height * (bl / 10);
                        var wimg = GetThumbNailImage(watermarkFilename, int.Parse(Math.Round(ws, 0).ToString()), int.Parse(Math.Round(hs, 0).ToString())); ;
                        var bg = wimg;
                        var g = Graphics.FromImage(bmp);
                        int w = int.Parse((Math.Round(bmp.Width * ((bl) / 10), 0)).ToString());
                        int h = int.Parse((Math.Round(bmp.Height * ((bl) / 10), 0)).ToString());
                        for (var x = 0; x < bmp.Width; x += w)
                        {
                            for (var y = 0; y < bmp.Height; y += h)
                            {
                                g.DrawImage(wimg, new Rectangle(x, y, wimg.Width, wimg.Height), 0, 0, wimg.Width, wimg.Height, GraphicsUnit.Pixel, imageAttributes);
                                //g.DrawImage(wimg, x, y); 
                            }
                        }
                        //if (System.IO.Directory.GetParent(imgPath).FullName != root)
                        //    saveDir = saveDir + "\\" + imgPath.Replace(root, "").Replace(System.IO.Path.GetFileName(imgPath), "");
                        //if (!System.IO.Directory.Exists(saveDir))
                        //    System.IO.Directory.CreateDirectory(saveDir);
                        //saveDir += "\\" + System.IO.Path.GetFileName(imgPath);
                        //if (System.IO.File.Exists(saveDir))
                        //    System.IO.File.Delete(saveDir);  //这句代码就是搞笑的。。已存在的会自动覆盖掉 ,根本没必要先删除后写入.
                        bmp.Save(saveDir);
                        g.Dispose();
                        bmp.Dispose();

                    }
                    else
                    {
                        double ws = img.Width * (bl / 10);
                        double hs = img.Height * (bl / 10);
                        var wimg = GetThumbNailImage(watermarkFilename, int.Parse(Math.Round(ws, 0).ToString()), int.Parse(Math.Round(hs, 0).ToString())); ;
                        var bg = wimg;
                        var g = Graphics.FromImage(img);
                        int w = int.Parse((Math.Round(img.Width * ((bl) / 10), 0)).ToString());
                        int h = int.Parse((Math.Round(img.Height * ((bl) / 10), 0)).ToString());
                        for (var x = 0; x < img.Width; x += w)
                        {
                            for (var y = 0; y < img.Height; y += h)
                            {
                                g.DrawImage(wimg, new Rectangle(x, y, wimg.Width, wimg.Height), 0, 0, wimg.Width, wimg.Height, GraphicsUnit.Pixel, imageAttributes);
                                //g.DrawImage(wimg, x, y); 
                            }
                        }
                        if (System.IO.Directory.GetParent(imgPath).FullName != root)
                            saveDir = saveDir + "\\" + imgPath.Replace(root, "").Replace(System.IO.Path.GetFileName(imgPath), "");
                        if (!System.IO.Directory.Exists(saveDir))
                            System.IO.Directory.CreateDirectory(saveDir);
                        saveDir += "\\" + System.IO.Path.GetFileName(imgPath);
                        if (System.IO.File.Exists(saveDir))
                            System.IO.File.Delete(saveDir);
                        img.Save(saveDir);
                        g.Dispose();
                        img.Dispose();
                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("[{0}]{1}", imgPath, ex.Message));
            }

        }
        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare,
PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,
PixelFormat.Format8bppIndexed
    };
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }


        public static System.Drawing.Image GetThumbNailImage(string imageFile, int thumMaxWidth, int thumMaxHeight)
        {
            System.Drawing.Image originalImage = null;
            System.Drawing.Image newImage = null;

            try
            {
                originalImage = System.Drawing.Image.FromFile(imageFile);
                newImage = GetThumbNailImage(originalImage, thumMaxWidth, thumMaxHeight);
            }
            catch { }
            finally
            {
                if (originalImage != null)
                {
                    originalImage.Dispose();
                    originalImage = null;
                }
            }

            return newImage;
        }

        public static  System.Drawing.Image GetThumbNailImage(System.Drawing.Image originalImage, int thumMaxWidth, int thumMaxHeight)
        {
            Size thumRealSize = Size.Empty;
            System.Drawing.Image newImage = originalImage;

            
            Graphics graphics = null;

            try
            {
                thumRealSize = GetNewSize(thumMaxWidth, thumMaxHeight, originalImage.Width, originalImage.Height);
                newImage = new Bitmap(thumRealSize.Width, thumRealSize.Height);
                graphics = Graphics.FromImage(newImage);

                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.Clear(Color.Transparent);

                graphics.DrawImage(originalImage, new Rectangle(0, 0, thumRealSize.Width, thumRealSize.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
            }
            catch { }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                    graphics = null;
                }
            }

            return newImage;
        }

        private static Size GetNewSize(int maxWidth, int maxHeight, int imageOriginalWidth, int imageOriginalHeight)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(imageOriginalWidth);
            double sh = Convert.ToDouble(imageOriginalHeight);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);

            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }

            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }
    }
}
