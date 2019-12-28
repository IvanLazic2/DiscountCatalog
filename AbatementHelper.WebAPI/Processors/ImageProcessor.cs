using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class ImageProcessor
    {
        public static byte[] CreateThumbnail(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            using (var srcImage = Image.FromStream(ms))
            using (var newImage = new Bitmap(100, 100))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, 100, 100));
                newImage.Save(stream, ImageFormat.Png);
                byte[] thumbnail = stream.ToArray();

                return thumbnail;

            }
        }

        //public static byte[] To1MB(byte[] byteArray)
        //{

        //}

        public static bool IsValid(byte[] byteArray)
        {
            try
            {
                using (var ms = new MemoryStream(byteArray))
                {
                    Image.FromStream(ms);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}