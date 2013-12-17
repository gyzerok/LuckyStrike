using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Input
{
    public class ScreenGrabber : AbstractGrabber
    {
        private BitmapExt snapshot;

        private readonly Color readyColor = Color.FromArgb(255, 178, 195, 205);
        private readonly Color _sitOutColor = Color.FromArgb(255, 192, 192, 192);
        private readonly Point readyPoint = new Point(729, 672);

        private readonly List<Rectangle> _nameRectangles = new List<Rectangle>()
        {
            new Rectangle(468,457,112,19),
            new Rectangle(199,378,112,19),
            new Rectangle(194,143,112,19),
            new Rectangle(373,73,112,19),
            new Rectangle(891,73,112,19),
            new Rectangle(1065,143,112,19),
            new Rectangle(1055,378,112,19),
            new Rectangle(795,457,112,19),
        };

        private readonly List<Rectangle> _betsRectangles = new List<Rectangle>()
        {
            new Rectangle(614,391,105,65),
            new Rectangle(455,400,105,65),
            new Rectangle(379,316,105,65),
            new Rectangle(402,230,105,65),
            new Rectangle(527,194,105,65),
            new Rectangle(749,190,105,65),
            new Rectangle(831,234,105,65),
            new Rectangle(877,307,105,65),
            new Rectangle(763,395,105,65),
        };

        private readonly List<Point> _sittingOutPixels = new List<Point>()
        {
            new Point(497,482),
            new Point(228,404),
            new Point(223,169),
            new Point(403,100),
            new Point(920,101),
            new Point(1093,169),
            new Point(1085,402),
            new Point(824,482),
        };
       
        private readonly List<Rectangle> _cardsRectangles = new List<Rectangle>()
        {
            new Rectangle(650, 460, 15, 40), //hand1
            new Rectangle(671, 465, 15, 40), //hand2
            new Rectangle(533, 227, 15, 40), //flop1
            new Rectangle(598, 227, 15, 40), //flop2
            new Rectangle(663, 227, 15, 40), //flop3
            new Rectangle(728, 227, 15, 40), //turn
            new Rectangle(793, 227, 15, 40), //river
        };

        private readonly List<Rectangle> _playersRectangles = new List<Rectangle>()
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

        private readonly List<Rectangle> _dealerRectangles = new List<Rectangle>()
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

        public override AbstractData Grab()
        {
            return new ScreenData(
                this.GrabHandsRectangles(), 
                this.GrabCardsRectangles(), 
                this.GrabDealerRectangles(),
                this.GrabBetsRectangles(), 
                this.GrubActivePlayers()
            );
        }

        public bool IsReady()
        {
            this.snapshot = BitmapExt.FromScreen();

            if (snapshot.GetPixel(this.readyPoint.X, this.readyPoint.Y) == this.readyColor)
                return true;

            return false;
        }

        private List<BitmapExt> GrabBetsRectangles()
        {
            var result = new List<BitmapExt>();

            foreach (var rectangle in _betsRectangles)
            {
                // Getting apoximate bet rect
                var croppedBmp = this.snapshot.Crop(rectangle);
                // Geting accurate bet text rect
                croppedBmp = croppedBmp.Crop(Color.FromArgb(255, 255, 246, 207));

                if (croppedBmp != null)
                    result.Add(croppedBmp);   
            }

            return result;
        }

        private List<BitmapExt> GrabHandsRectangles()
        {
            return this.snapshot.Crop(this._playersRectangles);
        }

        private List<BitmapExt> GrabDealerRectangles()
        {
            return this.snapshot.Crop(this._dealerRectangles);
        }

        private List<BitmapExt> GrabCardsRectangles()
        {
            return this.snapshot.Crop(this._cardsRectangles);
        }

        public List<int> GrubActivePlayers()
        {
            var result = new List<int>();

            for (var i = 0; i < this._nameRectangles.Count; i++)
            {
                var bmp = this.snapshot.Crop(this._nameRectangles[i]);
                var hasName = bmp.HasColor(Color.FromArgb(255, 192, 192, 192)); ;

                if (hasName)
                {
                    var point = this._sittingOutPixels[i];
                    if (this.snapshot.GetPixel(point.X, point.Y) != this._sitOutColor)
                        result.Add(i + 1);
                }
            }
            return result;
        }
    }
}
