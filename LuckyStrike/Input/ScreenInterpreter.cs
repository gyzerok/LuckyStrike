using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
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
        private Dictionary<string, string> cards = new Dictionary<string, string>();
        private int previousDealerPosition = 100;
        private Hand previousHand = null;
        private Table currentTable;
        private ScreenGrabber grabber;

        public ScreenInterpreter()
        {
            this.LoadCards();

            this.grabber = new ScreenGrabber();
        }

        public override void Interprete()
        {
            var data = (ScreenData) grabber.Grab();
            var newDealerPosition = this.GetDealerPosition(data.GetDealersBitmaps());
            var newHand = this.GetHeroHand(data.GetCardsBitmaps());

            Game game;
            if (this.IsNewHand(newDealerPosition, newHand))
                game = new Game();
            else
                game = this.currentTable.ActiveGame;

            var boardCards = this.GetBoardCards(data.GetCardsBitmaps());
            game.NextStreet(boardCards);

            

            var activePlayers = grubber.GrubActivePlayers();
            activePlayers.Insert(0,0);

            //New hand
            if (this.IsNewHand(newDealerPosition, newHand))
            {
                var i = newDealerPosition;
                var currentPlayer = 0;
                while (i < currentTable.Seats.Count)
                {
                    if ()   
                }
            }
            else
            {
                
            }
        }

        private bool PlayerHasCards(List<int>)
        {
            return true;
        }

        private bool RectangleHasColor(Color color, Bitmap bmp)
        {
            for (var i = 0; i < bmp.Height; i++)
                for (var j = 0; j < bmp.Width; j++)
                    if (bmp.GetPixel(i, j) == color)
                        return true;
         
            return false;
        }

        private int GetNextPlayerIndex(int currentIndex, int tableSize)
        {
            return (currentIndex + 1) % tableSize;
        }

        private bool IsNewHand(int newDealerPosition, Hand currentHand)
        {
            return (currentHand != this.previousHand && newDealerPosition != this.previousDealerPosition);
        }

        private int GetDealerPosition(List<BitmapExt> dealerBitmaps)
        {
            var dealerColor = Color.FromArgb(255, 169, 23, 13);

            for (var i = 0; i < dealerBitmaps.Count; i++)
                if (dealerBitmaps[i].HasColor(dealerColor))
                    return i;
        }

        private Hand GetHeroHand(List<BitmapExt> bmps)
        {
            return new Hand(
                Card.FromString(cards[bmps[0].Hash]), 
                Card.FromString(cards[bmps[1].Hash])
            );
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

        private List<Card> GetBoardCards(List<BitmapExt> bmps)
        {
            var ret = new List<Card>();

            var i = 2;
            while (i < bmps.Count)
            {
                if (! this.cards.ContainsKey(bmps[i].Hash))
                    return ret;

                ret.Add(Card.FromString(bmps[i].Hash));
                i++;
            }

            return ret;
        }
    }
}

//private ScreenGrabber grubber;
        //private Dictionary<string, string> cards = new Dictionary<string, string>();
        //private Table currenTable;
        //private int prevDealerPosition = 100;
        //private Timer timer; 

        //public ScreenInterpreter()
        //{
        //    grubber = new ScreenGrabber();
        //    this.LoadCards();
        //    this.currenTable = new Table(0,9);
        //    this.Timer_tick(this);
        //    //this.timer = new Timer(this.Timer_tick, null, 100, 100);
        //}

        //public void Timer_tick(Object sender)
        //{
        //    if (grubber.IsReady())
        //        this.Interprete();
        //}

        //public override void Interprete()
        //{
        
        //}

        //private void SeatPlayers(Dictionary<int, AbstractPlayer> players, int dealerPosition)
        //{
        //    var ho = new HIDOutput();
        //    players.Add(0, new ArtificialPlayer(BSStrategy.Instance, ho));
        //    foreach (var player in players)
        //    {
        //        this.currenTable.SeatPlayer(player.Key, player.Value);
        //    }
            
        //    //Dealer issue resolving
        //    if (this.currenTable.Seats[dealerPosition] is EmptySeat)
        //    {
        //        this.currenTable.SeatPlayer(dealerPosition, new OnlinePlayer());
        //        var curSeat = (NonEmptySeat)this.currenTable.Seats[dealerPosition];
        //        curSeat.Player.Act(0, new Activity(Decision.FOLD));
        //    }
        //}

        //private void formListOfActivities(int dealerPosition, Dictionary<int, double> bets)
        //{
        //    var ret = new Dictionary<int, Activity>();

        //    var curSeat = (NonEmptySeat)this.currenTable.Seats[dealerPosition];
        //    curSeat.Player.Act(0, new Activity(Decision.BLIND, 0.01));
        //    curSeat = curSeat.LeftNonEmpty;
        //    curSeat.Player.Act(0, new Activity(Decision.BLIND, 0.02));
        //    curSeat = curSeat.LeftNonEmpty;

        //    int counter = 2;
            
        //    while (counter <= bets.Count)
        //    {
        //        if (!bets.Keys.Contains(this.currenTable.Seats.IndexOf(curSeat)))
        //        {
        //            curSeat.Player.Act(0, new Activity(Decision.FOLD));    
        //        }

        //        if (curSeat.RightNonEmpty.Player.Activity.Bet < bets[this.currenTable.Seats.IndexOf(curSeat)])
        //        {
        //            curSeat.Player.Act(0, new Activity(Decision.RAISE, bets[this.currenTable.Seats.IndexOf(curSeat)]));
        //        }
        //        else
        //        {
        //            curSeat.Player.Act(0, new Activity(Decision.CALL));
        //        }

        //        curSeat = curSeat.LeftNonEmpty;
        //        counter++;
        //    }

        //}

        
        //}

        //private Dictionary<int, AbstractPlayer> IntepreteHands(ScreenData data)
        //{
        //    var activePlayers = new Dictionary<int,AbstractPlayer>();
        //    for (var i = 0; i < data.GetHandsBitmaps().Count; i++)
        //    {
        //        var flag = false;
        //        var targetBitmap = data.GetHandsBitmaps()[i];
        //        for (var j = 0; j < targetBitmap.Height; j++)
        //        {
        //            for (var k = 0; k < targetBitmap.Width; k++)
        //            {
        //                if (targetBitmap.GetPixel(j,k) == Color.FromArgb(255, 160, 73, 70))
        //                {
        //                    activePlayers.Add(i+1, new OnlinePlayer());
        //                    flag = true;
        //                    break;
        //                }
        //            }
        //            if (flag)
        //                break;
        //        }
        //    }
        //    return activePlayers;
        //}

        //private Dictionary<int, double> InterpreteBets(ScreenData data)
        //{
        //    var playerBets = new Dictionary<int, double>();
        //    for (var i = 0; i < data.GetBetssBitmaps().Count; i++)
        //    {
        //        var targetBitmap = data.GetBetssBitmaps()[i];
        //        if (targetBitmap != null)
        //        {
        //            playerBets.Add(i, DigitOcr.Recognize(targetBitmap));
        //        }
        //    }
        //    return playerBets;
        //}

        //private int InterpeteDealer(ScreenData data)
        //{
        //    var dealerColor = Color.FromArgb(255, 169, 23, 13);

        //    var ret = 0;
        //    for (var i = 0; i < data.GetDealersBitmaps().Count; i++)
        //    {
        //        var flag = false;
        //        var targetBitmap = data.GetDealersBitmaps()[i];
        //        for (int j = 0; j < targetBitmap.Width; j++)
        //        {
        //            for (int k = 0; k < targetBitmap.Height; k++)
        //            {
        //                if (targetBitmap.GetPixel(j, k) == dealerColor)
        //                {
        //                    ret = i;
        //                    flag = true;
        //                    break;
        //                }
        //            }
        //            if (flag) break;
        //        }
        //        //if (flag) break;
        //    }
        //    return ret;
        //}

        //private void InterpreteCards(ScreenData data)
        //{
        //    //Getting hand
        //    var hand = new Hand(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[0])]), Card.FromString(cards[GetHash(data.GetCardsBitmaps()[1])]));
        //    var hero = (ArtificialPlayer)currenTable.ActiveGame.Players[0];
        //    hero.Hand = hand;
        //    //Getting flop
        //    var boardCards = new List<Card>();
        //    if (cards.ContainsKey(GetHash(data.GetCardsBitmaps()[2])))
        //    {
        //        boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[2])]));
        //        boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[3])]));
        //        boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[4])]));
        //    }
        //    //Getting postflop
        //    if (cards.ContainsKey(GetHash(data.GetCardsBitmaps()[5])))
        //    {
        //        boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[5])]));
        //    }
        //    if (cards.ContainsKey(GetHash(data.GetCardsBitmaps()[6])))
        //    {
        //        boardCards.Add(Card.FromString(cards[GetHash(data.GetCardsBitmaps()[6])]));
        //    }

        //    //Board processing
        //    switch (boardCards.Count)
        //    {
        //        case 0:
        //        case 3:
        //            currenTable.ActiveGame.NextStreet(boardCards);
        //            break;
        //        case 4:
        //            currenTable.ActiveGame.NextStreet(boardCards[3]);
        //            break;
        //        default:
        //            currenTable.ActiveGame.NextStreet(boardCards[5]);
        //            break;
        //    }



