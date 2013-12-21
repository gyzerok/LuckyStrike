using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using AI;
using Common.Domain;
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
        private int prevCardCount = 0;
        private double smallBlind = 0.01;
        private double betValue;
        private bool isPreflop;

        private List<int> previousPlayers;
        private List<int> foldedPlayers; 

        public ScreenInterpreter()
        {
            this.LoadCards();
            this.grabber = new ScreenGrabber();
            this.currentTable = new Table(0, 9, BSStrategy.Instance, new HIDOutput());
            this.previousPlayers = new List<int>(this.currentTable.Size);
            this.foldedPlayers = new List<int>(this.currentTable.Size);
        }

        public override void Interprete()
        {
            var data = (ScreenData)grabber.Grab();

            var newDealerPosition = this.GetDealerPosition(data.GetDealersBitmaps());

            var newHand = this.GetHeroHand(data.GetCardsBitmaps());

            if (IsNewHand(newDealerPosition, newHand))
            {
                //form.Clear();
                this.betValue = this.smallBlind;
                this.foldedPlayers.Clear();
                this.SitPlayers(this.previousPlayers, data.GetActivePlayers());
                currentTable.NewHand(newDealerPosition);
                (currentTable.Seats[0] as NonEmptySeat).Hand = newHand;

                this.prevCardCount = 0;

                //form.Show("Dealer position is: " + newDealerPosition.ToString());
                //form.Show("Hero hand: " + newHand[0] + ' ' + newHand[1]);
            }

            var newCards = this.GetBoardCards(data.GetCardsBitmaps());

            if (newCards.Count != prevCardCount)
            {
            //    var temp = "";
            //    foreach (var str in newCards)
            //    {
            //        temp += cards[str] + ' ';
            //    }
            //    form.Show("Board: " + temp);
                prevCardCount = newCards.Count;
            }

            this.currentTable.NextStreet(newCards);

            //Getting current state
            this.isPreflop = (this.currentTable.Street == Street.PREFLOP) ? true : false;

            var activePlayers = this.GetActivePlayers(data.GetActivePlayers());
            var startingIndex = 0;
            var dealerRelativePosition = activePlayers.IndexOf(newDealerPosition);
            var blind = (0 - dealerRelativePosition == -activePlayers.Count + 1 || 0 - dealerRelativePosition == -activePlayers.Count + 2)

                ? true
                : false;

            if ((!IsNewHand(newDealerPosition, newHand) && newCards.Count == prevCardCount) || (blind && isPreflop))
            {
                startingIndex = 1;
            }
            else
            {
                startingIndex = newDealerPosition+1;
            }

            //form.Show("Iterating from: " + startingIndex.ToString());

            while (startingIndex < currentTable.Size)
            {
                if (activePlayers.Contains(startingIndex))
                {
                    var bet = this.GetBet(data.GetBetsBitmaps()[startingIndex]);
                    var currentPlayer = (this.currentTable.Seats[startingIndex] as NonEmptySeat);

                    if (IsActivePlayer(data.GetPlayersBitmaps()[startingIndex - 1]))
                    {
                        var currentBet = Convert.ToDouble(bet);
                        
                        if (currentBet == smallBlind || betValue == smallBlind)
                        {
                            currentPlayer.Act(new Activity(Decision.BLIND));
                            this.betValue = currentBet;
                        }

                        else if (currentBet == betValue)
                        {
                            currentPlayer.Act(new Activity(Decision.CALL));
                            this.betValue = currentBet;
                        }

                        else if (currentBet > betValue)
                        {
                            currentPlayer.Act(new Activity(Decision.RAISE));
                            this.betValue = currentBet;    
                        }
                    }
                    else
                    {
                        currentPlayer.Act(new Activity(Decision.FOLD));
                        //form.Show("Player #" + startingIndex.ToString() + " folds");
                        this.foldedPlayers.Add(startingIndex);
                    }
                }
                startingIndex++;
            }

            (this.currentTable.Seats[0] as NonEmptySeat).Act(new Activity(Decision.UNKNOWN));

            previousDealerPosition = newDealerPosition;
            previousHand = newHand;
        }

        
        private int GetNextPlayerIndex(int currentIndex, int tableSize)
        {
            return (currentIndex + 1) % tableSize;
        }

        private bool IsNewHand(int newDealerPosition, Hand currentHand)
        {
            return (currentHand != this.previousHand && newDealerPosition != this.previousDealerPosition);
        }

        private void SitPlayers(List<int> previousPlayers, List<int> newPlayers)
        {
            for (var i = 1; i < previousPlayers.Count; i++)
            {
                if (!newPlayers.Contains(previousPlayers[i]))
                {
                    this.currentTable.SitOut(i);
                }
            }



            for (var i=0; i<newPlayers.Count; i++)
            {
                if (!previousPlayers.Contains(newPlayers[i]))
                {
                    this.currentTable.SitIn(i+1, new OnlinePlayer());
                }
            }
        }

        private string GetBet(BitmapExt bmp)
        {
            
            if (bmp == null)
                return "";
            else
            {
                var result = bmp.ToString();
                result = result.Replace('n', '0');
                result = result.Replace('.', ',');
                return result;
            }    
        }

        private bool IsActivePlayer(BitmapExt bmp)
        {
            if (bmp.HasColor(Color.FromArgb(255, 160, 73, 70)))
                return true;
            return false;
        }

        private int GetDealerPosition(List<BitmapExt> dealerBitmaps)
        {
            var dealerColor = Color.FromArgb(255, 203, 199, 56);

            for (var i = 0; i < dealerBitmaps.Count; i++)
                if (dealerBitmaps[i].HasColor(dealerColor))
                    return i;
            throw new Exception("Dealer has not been recognized");
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

        private List<int> GetActivePlayers(List<int> currentPlayers)
        {
            var result = new List<int>(currentPlayers);

            foreach (var foldedPlayer in foldedPlayers)
            {
                if (result.Contains(foldedPlayer))
                    result.Remove(foldedPlayer);
            }

            return result;
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



