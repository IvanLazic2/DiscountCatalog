using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Processors
{
    public static class ImageProcessor
    {
        public static byte[] CreateThumbnail(byte[] byteArray)
        {
            //using (var ms = new MemoryStream(byteArray))
            //using (var srcImage = Image.FromStream(ms))
            //using (var newImage = new Bitmap(40, 40))
            //using (var graphics = Graphics.FromImage(newImage))
            //using (var stream = new MemoryStream())
            //{
            //    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //    graphics.DrawImage(srcImage, new Rectangle(0, 0, 40, 40));
            //    newImage.Save(stream, ImageFormat.Png);

            //    byte[] thumbnail = stream.ToArray();

            //    return thumbnail;

            //}


            if (byteArray != null && byteArray.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                using (Image thumbnail = Image.FromStream(new MemoryStream(byteArray)).GetThumbnailImage(50, 50, null, new IntPtr()))
                {
                    thumbnail.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }

            return new byte[0];
        }

        public static byte[] SetDefault(string entityName)
        {
            string root = AppContext.BaseDirectory;

            string target = root + "/Content/DefaultImages";

            byte[] image = File.ReadAllBytes($"{target}/{entityName}-dark.png");

            return image;
        }

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