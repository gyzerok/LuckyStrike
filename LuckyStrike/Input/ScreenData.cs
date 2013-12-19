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
        private List<BitmapExt> playersBitmaps;
        private List<BitmapExt> cardsBitmaps;
        private List<BitmapExt> dealersBitmaps;
        private List<BitmapExt> betsBitmaps;
        private List<int> activePlayers;

        public ScreenData(List<BitmapExt> players, List<BitmapExt> cards, List<BitmapExt> dealers, List<BitmapExt> bets, List<int> activePlayers)
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

        public List<BitmapExt> GetPlayersBitmaps()
        {
            return this.playersBitmaps;
        }

        public List<BitmapExt> GetCardsBitmaps()
        {
            return this.cardsBitmaps;
        }

        public List<BitmapExt> GetDealersBitmaps()
        {
            return this.dealersBitmaps;
        }

        public List<BitmapExt> GetBetsBitmaps()
        {
            return this.betsBitmaps;
        }

        public override void LoadConfig()
        {
           
        }
    }
}
