using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Common.Domain;

namespace AI
{
    public class BSStrategy : AbstractStrategy
    {
        private Dictionary<List<string>, List<List<int>>> preflopTable;
        private static BSStrategy instance = null;

        public static BSStrategy Instance
        {
            get
            {
                if (instance == null) instance = new BSStrategy();

                return instance;
            }
        }

        private BSStrategy()
        {
            
        }

        public override Activity Process(NonEmptySeat seat)
        {
            var player = (seat.Player as ArtificialPlayer);

            if (seat.Table.ActiveGame.Street == Street.PREFLOP)
            {
                var subtable = this.GetSubtable(player.Hand);

                var previousDecision = this.GetPreviousDecision(seat);
                var position = this.GetPosition(seat);

                var decision = subtable[(int) previousDecision][(int) position];

                return new Activity((Decision)decision);
            }

            return null;
        }

        private List<List<int>> GetSubtable(Hand hand)
        {
            foreach (var element in preflopTable)
            {
                if (element.Key.Contains(hand.ToString()))
                {
                    return element.Value;
                }
            }

            return null;
        }

        private Decision GetPreviousDecision(NonEmptySeat seat)
        {
            var previousSeat = seat.RightNonEmpty;

            if (previousSeat.Player.Activity.Decision == Decision.BLIND)
            {
                return Decision.FOLD;
            }

            if (seat == previousSeat)
            {
                throw new Exception("Table is empty");
            }

            return previousSeat.Player.Activity.Decision;
        }

        private Position GetPosition(NonEmptySeat seat)
        {
            if (seat.LeftNonEmpty.Player.Activity.Decision == Decision.BLIND ||
                seat.RightNonEmpty.Player.Activity.Decision == Decision.BLIND)
            {
                return Position.BLIND;
            }

            var positions = new List<int>();
            positions.Add(2);
            positions.Add(3);
            positions.Add(2);

            int minus = seat.Table.Seats.Count - seat.Table.ActiveGame.Players.Count;
            int i = 0;
            while (minus > 0)
            {
                positions[i]--;
                minus--;

                if (positions[i] == 0) i++;
            }

            var checkingSeat = seat;
            if (checkingSeat.Player == seat.Player) return Position.LATE;

            int j = positions.Count - 1;
            while (j > 0)
            {
                if (checkingSeat.Player == seat.Player)
                    return (Position) j + 1;

                positions[j]--;
                if (positions[j] == 0) j--;

                while (!seat.Table.ActiveGame.Players.Contains(checkingSeat.RightNonEmpty.Player))
                {
                    checkingSeat = checkingSeat.RightNonEmpty;
                }
            }

            throw new Exception("Position indetification fail");
        }
    }
}
