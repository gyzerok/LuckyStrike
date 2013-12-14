using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

using Common.Domain;
using Common;
using AI;
using Output;

namespace Input
{
    public class ScreenInterpreter : AbstractInterpreter
    {
        private ScreenGrubber grubber;
        private Dictionary<string, string> cards = new Dictionary<string, string>();
        private Table currenTable;
        private int prevDealerPosition = 100;
        private Timer timer; 

        public ScreenInterpreter()
        {
            grubber = new ScreenGrubber();
            this.LoadCards();
            this.currenTable = new Table(0,9);
            this.Timer_tick(this);
            //this.timer = new Timer(this.Timer_tick, null, 100, 100);
        }

        public void Timer_tick(Object sender)
        {
            if (grubber.IsReady())
                this.Interprete();
        }

        public override void Interprete()
        {
            var screenData = (ScreenData)grubber.Grub();

            var newDealerPos = this.InterpeteDealer(screenData);

            if (newDealerPos != this.prevDealerPosition || prevDealerPosition == 100)
            {
                var activePlayers = this.IntepreteHands(screenData);   
                this.SeatPlayers(activePlayers);
                var debug = activePlayers.Values.ToList();
                this.currenTable.Games.Add(new Game(this.currenTable, debug, newDealerPos));
                prevDealerPosition = newDealerPos;
            }

            this.formListOfActivities(this.prevDealerPosition, this.InterpreteBets(screenData));
    
            (this.currenTable.Seats[0] as NonEmptySeat).Player.Act(0);
        }

        private void LoadCards()
        {
            var result = new Dictionary<string, string>();
            using (var fs = File.OpenRead("../../../cards.cfg"))
            using (var reader = new BinaryReader(fs))
            {
                // Get count.
                var count = reader.ReadInt32();
                // Read in all pairs.
                for (var i = 0; i < count; i++)
                {
                    var key = reader.ReadString();
                    var value = reader.ReadString();
                    result.Add(key, value);
                }
            }
            cards = result;
        }

        private void SeatPlayers(Dictionary<int, AbstractPlayer> players)
        {
            var ho = new HIDOutput();
            players.Add(0, new ArtificialPlayer(BSStrategy.Instance, ho));
            foreach (var player in players)
            {
                this.currenTable.SeatPlayer(player.Key, player.Value);
            }
        }

        private void formListOfActivities(int dealerPosition, Dictionary<int, double> bets)
        {
            var ret = new Dictionary<int, Activity>();

            var curSeat = (NonEmptySeat)this.currenTable.Seats[dealerPosition];
            curSeat.Player.Act(0, new Activity(Decision.BLIND, 0.01));
            curSeat = curSeat.LeftNonEmpty;
            curSeat.Player.Act(0, new Activity(Decision.BLIND, 0.02));
            curSeat = curSeat.LeftNonEmpty;

            int counter = 2;
            
            while (counter <= bets.Count)
            {
                if (!bets.Keys.Contains(this.currenTable.Seats.IndexOf(curSeat)))
                {
                    curSeat.Player.Act(0, new Activity(Decision.FOLD));    
                }

                if (curSeat.RightNonEmpty.Player.Activity.Bet < bets[this.currenTable.Seats.IndexOf(curSeat)])
                {
                    curSeat.Player.Act(0, new Activity(Decision.RAISE, bets[this.currenTable.Seats.IndexOf(curSeat)]));
                }
                else
                {
                    curSeat.Player.Act(0, new Activity(Decision.CALL));
                }

                curSeat = curSeat.LeftNonEmpty;
                counter++;
            }
        }

        private static string GetHash(Bitmap image)
        {
            // get the bytes from the image
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp); // gif for example
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

        private Dictionary<int, AbstractPlayer> IntepreteHands(ScreenData data)
        {
            var activePlayers = new Dictionary<int,AbstractPlayer>();
            for (var i = 0; i < data.GetHandsBitmaps().Count; i++)
            {
                var flag = false;
                var targetBitmap = data.GetHandsBitmaps()[i];
                for (var j = 0; j < targetBitmap.Height; j++)
                {
                    for (var k = 0; k < targetBitmap.Width; k++)
                    {
                        if (targetBitmap.GetPixel(j,k) == Color.FromArgb(255, 160, 73, 70))
                        {
                            activePlayers.Add(i+1, new OnlinePlayer());
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        break;
                }
            }
            return activePlayers;
        }

        private Dictionary<int, double> InterpreteBets(ScreenData data)
        {
            var playerBets = new Dictionary<int, double>();
            for (var i = 0; i < data.GetBetssBitmaps().Count; i++)
            {
                var targetBitmap = data.GetBetssBitmaps()[i];
                if (targetBitmap != null)
                {
                    playerBets.Add(i, DigitOcr.Recognize(targetBitmap));
                }
            }
            return playerBets;
        }

        private int InterpeteDealer(ScreenData data)
        {
            var dealerColor = Color.FromArgb(255, 169, 23, 13);

            var ret = 0;
            for (var i = 0; i < data.GetDealersBitmaps().Count; i++)
            {
                var flag = false;
                var targetBitmap = data.GetDealersBitmaps()[i];
                for (int j = 0; j < targetBitmap.Width; j++)
                {
                    for (int k = 0; k < targetBitmap.Height; k++)
                    {
                        if (targetBitmap.GetPixel(j, k) == dealerColor)
                        {
                            ret = i;
                            flag = true;
                            break;
                        }
                    }
                    if (flag) break;
                }
                //if (flag) break;
            }
            return ret;
        }

        private void InterpreteCards(ScreenData data)
        {
            //Getting hand
            var hand = new Hand(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[0])]), Card.FromString(cards[GetHash(data.GetCardsBitmaps()[1])]));
            var hero = (ArtificialPlayer)currenTable.ActiveGame.Players[0];
            hero.Hand = hand;
            //Getting flop
            var boardCards = new List<Card>();
            if (cards.ContainsKey(GetHash(data.GetCardsBitmaps()[2])))
            {
                boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[2])]));
                boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[3])]));
                boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[4])]));
            }
            //Getting postflop
            if (cards.ContainsKey(GetHash(data.GetCardsBitmaps()[5])))
            {
                boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[5])]));
            }
            if (cards.ContainsKey(GetHash(data.GetCardsBitmaps()[6])))
            {
                boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[6])]));
            }

            //Board processing
            switch (boardCards.Count)
            {
                case 0:
                case 3:
                    currenTable.ActiveGame.NextStreet(boardCards);
                    break;
                case 4:
                    currenTable.ActiveGame.NextStreet(boardCards[3]);
                    break;
                default:
                    currenTable.ActiveGame.NextStreet(boardCards[5]);
                    break;
            }

        }

    }
}
