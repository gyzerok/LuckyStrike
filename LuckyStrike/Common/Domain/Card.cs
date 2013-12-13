using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Card
    {
        public CardValue Value { get; private set; }
        public CardSuit Suit { get; private set; }

        public Card(CardValue value, CardSuit suit)
        {
            this.Value = value;
            this.Suit = suit;
        }
    }
}
