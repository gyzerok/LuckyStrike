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
        private List<Bitmap> playersBitmaps;
        private List<Bitmap> cardsBitmaps;
        private List<Bitmap> dealersBitmaps;
        private List<Bitmap> betsBitmaps;
        private List<int> activePlayers; 
 
        public ScreenData(List<Bitmap> players, List<Bitmap> cards, List<Bitmap> dealers, List<Bitmap> bets, List<int> activePlayers )
        {
            this.playersBitmaps = players;
            this.cardsBitmaps = cards;
            this.dealersBitmaps = dealers;
            this.betsBitmaps = bets;
            this.activePlayers = activePlayers;
        }

        public List<int> GetActivePlayers()
        {
            return this.activePlayers;
        }

        public List<Bitmap> GetPlayersBitmaps()
        {
            return this.playersBitmaps;
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
