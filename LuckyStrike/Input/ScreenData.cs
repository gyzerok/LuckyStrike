using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Input
{
    public class ScreenData : AbstractData
    {
        private List<Bitmap> handsBitmaps;
        private List<Bitmap> cardsBitmaps;
        private List<Bitmap> dealersBitmaps;
        private List<Bitmap> betsBitmaps;
 
        public ScreenData(List<Bitmap> hands, List<Bitmap> cards, List<Bitmap> dealers, List<Bitmap> bets)
        {
            this.handsBitmaps = hands;
            this.cardsBitmaps = cards;
            this.dealersBitmaps = dealers;
            this.betsBitmaps = bets;
        }

        public List<Bitmap> GetHandsBitmaps()
        {
            return this.handsBitmaps;
        }

        public List<Bitmap> GetCardsBitmaps()
        {
            return this.cardsBitmaps;
        }

        public List<Bitmap> GetDealersBitmaps()
        {
            return this.dealersBitmaps;
        }

        public List<Bitmap> GetBetssBitmaps()
        {
            return this.betsBitmaps;
        }

        public override void LoadConfig()
        {
           
        }
    }
}
