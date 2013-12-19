using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Input
{
    public class BitmapExt
    {
        private Bitmap bmp;

        public BitmapExt(Bitmap bmp)
        {
            this.bmp = bmp;
        }

        public Color GetPixel(int i, int j)
        {
            return this.bmp.GetPixel(i, j);
        }

        public BitmapExt Crop(Rectangle rect)
        {
            return new BitmapExt(this.bmp.Clone(rect, PixelFormat.DontCare));
        }

        public BitmapExt Crop(Color color)
        {
            var minX = this.bmp.Width - 1;
            var minY = this.bmp.Height - 1;
            var maxX = 0;
            var maxY = 0;

            for (var i = 0; i < this.bmp.Height; i++)
            {
                for (var j = 0; j < this.bmp.Width; j++)
                {
                    if (this.bmp.GetPixel(j, i) == color)
                    {
                        minX = Math.Min(minX, j);
                        maxX = Math.Max(maxX, j);
                        minY = Math.Min(minY, i);
                        maxY = Math.Max(maxY, i);
                    }
                }
            }

            if (minX < maxX)
            {
                minX--;
                minY--;
                maxY++;
                maxX++;

                return new BitmapExt(this.bmp.Clone(new Rectangle(minX, minY, maxX - minX, maxY - minY), PixelFormat.DontCare));
            }

            return null;
        }

        public List<BitmapExt> Crop(List<Rectangle> rects)
        {
            var list = new List<BitmapExt>();

            foreach (var rect in rects)
            {
                list.Add(this.Crop(rect));
            }

            return list;
        }

        public bool HasColor(Color color)
        {
            for (int i = 0; i < this.bmp.Height; i++)
                for (int j = 0; j < this.bmp.Width; j++)
                    if (this.bmp.GetPixel(j, i) == color)
                        return true;

            return false;
        }

        public static BitmapExt FromScreen()
        {
            var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            var g = Graphics.FromImage(image as Image);
            g.CopyFromScreen(0, 0, 0, 0, image.Size);

            return new BitmapExt(image);
        }

        public override string ToString()
        {
            var ocr = new tessnet2.Tesseract();

            ocr.Init(null, "eng", false);
            ocr.SetVariable("tessedit_char_whitelist", "0123456789,$");

            var result = ocr.DoOCR(this.bmp, Rectangle.Empty);

            return result[0].Text;
        }

        public string Hash
        {
            get
            {
                byte[] bytes = null;
                using (var ms = new MemoryStream())
                {
                    this.bmp.Save(ms, ImageFormat.Bmp); // gif for example
                    bytes = ms.ToArray();
                }
                // hash the bytes
                var md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(bytes);
                var temp = "";
                foreach (byte value in hash)
                {
                    temp += value.ToString();
                }

                return temp;
            }
        }
    }
}
