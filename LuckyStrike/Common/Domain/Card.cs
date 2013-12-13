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

        public override string ToString()
        {
            switch (this.Value)
            {
                case Value._2:
                    return "2";
                case Value._3:
                    return "3";
                case Value._4:
                    return "4";
                case Value._5:
                    return "5";
                case Value._6:
                    return "6";
                case Value._7:
                    return "7";
                case Value._8:
                    return "8";
                case Value._9:
                    return "9";
                case Value._T:
                    return "T";
                case Value._J:
                    return "J";
                case Value._Q:
                    return "Q";
                case Value._K:
                    return "K";
                case Value._A:
                    return "A";
                default:
                    return "";
            }
        }

        public static Card FromString(string str)
        {
            var card = new Card();

            switch (str[0])
            {
                case '2':
                    card.Value = CardValue._2;
                    break;
                case '3':
                    card.Value = CardValue._3;
                    break;
                case '4':
                    card.Value = CardValue._4;
                    break;
                case '5':
                    card.Value = CardValue._5;
                    break;
                case '6':
                    card.Value = CardValue._6;
                    break;
                case '7':
                    card.Value = CardValue._7;
                    break;
                case '8':
                    card.Value = CardValue._8;
                    break;
                case '9':
                    card.Value = CardValue._9;
                    break;
                case 'T':
                    card.Value = CardValue._T;
                    break;
                case 'J':
                    card.Value = CardValue._J;
                    break;
                case 'Q':
                    card.Value = CardValue._Q;
                    break;
                case 'K':
                    card.Value = CardValue._K;
                    break;
                default:
                    card.Value = CardValue._A;
                    break;
            }

            switch (str[1])
            {
                case 's':
                    card.Suit = CardSuit.S;
                    break;
                case 'c':
                    card.Suit = CardSuit.C;
                    break;
                case 'd':
                    card.Suit = CardSuit.D;
                    break;
                default:
                    card.Suit = CardSuit.H;
                    break;
            }

            return card;
        }
    }
}
