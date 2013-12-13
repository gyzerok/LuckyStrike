using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Hand
    {
        public List<Card> Cards { get; private set; }

        public Hand(Card card1, Card card2)
        {
            this.Cards = new List<Card>();
            this.Cards.Add(card1);
            this.Cards.Add(card2);
        }

        public override string ToString()
        {
            string ret = "";

            if ((int)Cards[0].Value < (int)Cards[1].Value)
            {
                ret = this.Cards[1].ToString() + this.Cards[0].ToString();
            }
            else
            {
                ret = this.Cards[0].ToString() + this.Cards[1].ToString();
            }

            if (this.Cards[0].Suit == this.Cards[1].Suit)
                ret += "s";
            else
                ret += "o";

            return ret;
        }
    }
}
