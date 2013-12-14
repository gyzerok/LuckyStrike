using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public enum Position
    {
        EARLY = 1,
        MIDDLE,
        LATE,
        BLIND
    }

    public enum Decision
    {
        UNKNOWN = 1,
        BLIND,
        BET,
        FOLD,
        CALL,
        RAISE,
    }

    public enum Street
    {
        PREFLOP = 1,
        FLOP,
        TURN,
        RIVER
    }

    public enum CardSuit
    {
        D = 1, //Diamonds
        H,     //Hearts
        S,     //Spades
        C,     //Clubs
    }

    public enum CardValue
    {
        _2 = 2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _T,
        _J,
        _Q,
        _K,
        _A,
    }
}
