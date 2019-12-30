using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

namespace AbatementHelper.MVC.Processors
{
    public static class ImageProcessor
    {
        public static byte[] To1MB(byte[] image)
        {
            byte[] currentByteArrayImage = image;
            double scale = 1f;

            if (!IsValid(image))
            {
                return null;
            }

            using (var inputMS = new MemoryStream(image))
            {
                Image fullSizeImage = Image.FromStream(inputMS);

                while (currentByteArrayImage.Length > 1000000)
                {
                    Bitmap fullSizeBitmap = new Bitmap(fullSizeImage, new Size((int)(fullSizeImage.Width * scale), (int)(fullSizeImage.Height * scale)));
                    using (var outputMS = new MemoryStream())
                    {
                        fullSizeBitmap.Save(outputMS, fullSizeImage.RawFormat);

                        currentByteArrayImage = outputMS.ToArray();

                        scale -= 0.05f;
                    }
                }

                return currentByteArrayImage;
            }
            
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