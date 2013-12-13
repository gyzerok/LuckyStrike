using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Game
    {
        public Table Table { get; private set; }
        public List<Card> Board { get; private set; }
        public List<AbstractPlayer> Players { get; private set; }
        public AbstractPlayer Dealer { get; private set; }

        public double Bank
        {
            get
            {
                // TODO implement later
                return 0.0;
            }
        }

        public Street Street
        {
            get
            {
                switch (this.Board.Count)
                {
                    case 0:
                        return Street.PREFLOP;
                    case 3:
                        return Street.FLOP;
                    case 4:
                        return Street.TURN;
                    default: //case 5
                        return Street.RIVER;
                }
            }
        }

        public Game(Table table, List<AbstractPlayer> players, int dealer)
        {
            this.Table = table;
            this.Players = players;
            this.Dealer = this.Players[dealer];
        }

        public void NextStreet(Card card)
        {
            this.Board.Add(card);
        }

        public void NextStreet(List<Card> cards)
        {
            this.Board = cards;
        }
    }
}
