﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Input
{
    public class ScreenGrubber : AbstractGrubber
    {
        private Bitmap image;

        private Color readyColor = Color.FromArgb(255, 178, 195, 205);
        private Point readyPoint = new Point(729, 672);

        private readonly List<Rectangle> cardsRects = new List<Rectangle>()
        {
            new Rectangle(650, 460, 15, 40), //hand1
            new Rectangle(671, 465, 15, 40), //hand2
            new Rectangle(533, 227, 15, 40), //flop1
            new Rectangle(598, 227, 15, 40), //flop2
            new Rectangle(663, 227, 15, 40), //flop3
            new Rectangle(728, 227, 15, 40), //turn
            new Rectangle(793, 227, 15, 40), //river
        };

        private readonly List<Rectangle> betsRects = new List<Rectangle>()
        {
            new Rectangle(614,391,105,35),
            new Rectangle(455,400,105,35),
            new Rectangle(379,316,105,35),
            new Rectangle(402,230,105,35),
            new Rectangle(527,194,105,35),
            new Rectangle(749,190,105,35),
            new Rectangle(831,234,105,35),
            new Rectangle(877,307,105,35),
            new Rectangle(763,395,105,35),
        };

        private readonly List<Rectangle> handsRects = new List<Rectangle>()
        {
            new Rectangle(398,379,30,30),
            new Rectangle(310,286,30,30),
            new Rectangle(406,174,30,30),
            new Rectangle(484,140,30,30),
            new Rectangle(858,138,30,30),
            new Rectangle(937,174,30,30),
            new Rectangle(1029,288,30,30),
            new Rectangle(923,380,30,30),
        };

        private readonly List<Rectangle> dealerRects = new List<Rectangle>()
        {
            new Rectangle(661,425,30,25),
            new Rectangle(475,427,30,25),
            new Rectangle(316,334,30,25),
            new Rectangle(340,212,30,25),
            new Rectangle(522,140,30,25),
            new Rectangle(826,140,30,25),
            new Rectangle(1002,213,30,25),
            new Rectangle(1022,332,30,25),
            new Rectangle(874,428,30,25),
        };

        public override AbstractData Grub()
        {
            this.image = ScreenGrubber.Snapshot();
            this.GrubBetsRectangles();
            return new ScreenData(this.GrubHandsRectangles(), this.GrubCardsRectangles(), this.GrubDealerRectangles(),
                this.GrubBetsRectangles());
        }

        public bool IsReady()
        {
            this.image = ScreenGrubber.Snapshot();
            if (image.GetPixel(this.readyPoint.X, this.readyPoint.Y) == this.readyColor)
                return true;
            return false;
        }

        public static Bitmap Snapshot()
        {
            var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            var g = Graphics.FromImage(image as Image);
            g.CopyFromScreen(0, 0, 0, 0, image.Size);
            return image;
        }

        public static Bitmap Crop(Bitmap source, Rectangle rect)
        {
            return source.Clone(rect, PixelFormat.DontCare);
        }

        public static Bitmap DetectBet(Bitmap image)
        {
            var minX = image.Width - 1;
            var maxX = 0;
            var minY = image.Height - 1;
            var maxY = 0;
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    if (image.GetPixel(j, i) == Color.FromArgb(255, 255, 246, 207))
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
                return image.Clone(new Rectangle(minX, minY, maxX - minX, maxY - minY), PixelFormat.DontCare);
            }
            return null;
        }

        private List<Bitmap> GrubBetsRectangles()
        {
            var result = new List<Bitmap>();
            foreach (var rectangle in betsRects)
            {
                result.Add(new Bitmap(ScreenGrubber.DetectBet(ScreenGrubber.Crop(image,rectangle))));   
            }
            return result;
        }

        private List<Bitmap> GrubHandsRectangles()
        {
            var result = new List<Bitmap>();
            foreach (var rectangle in handsRects)
            {
                result.Add(new Bitmap(ScreenGrubber.Crop(image,rectangle)));
            }
            return result;
        }

        private List<Bitmap> GrubDealerRectangles()
        {
            var result = new List<Bitmap>();
            foreach (var rectangle in dealerRects)
            {
                result.Add(new Bitmap(ScreenGrubber.Crop(image,rectangle)));
            }
            return result;
        }

        private List<Bitmap> GrubCardsRectangles()
        {
            var result = new List<Bitmap>();
            foreach (var rectangle in cardsRects)
            {
                result.Add(new Bitmap(ScreenGrubber.Crop(image,rectangle)));
            }
            return result;
        }
    }
}
