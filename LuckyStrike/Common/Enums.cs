using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
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
        BET = 1,
        CALL,
        RAISE,
        FOLD
    }

    public enum Street
    {
        PREFLOP = 1,
        FLOP,
        TURN,
        RIVER
    }
}
