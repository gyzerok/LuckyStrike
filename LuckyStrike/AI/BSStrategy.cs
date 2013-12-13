using System;
using System.Collections.Generic;
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
        private BSStrategy instance = null;

        public BSStrategy Instance
        {
            get
            {
                if (this.instance == null) this.instance = new BSStrategy();

                return this.instance;
            }
        }

        private BSStrategy()
        {
            
        }

        public override Activity Process(NonEmptySeat seat)
        {
            var game = seat.Table.ActiveGame;
            var player = (seat.Player as ArtificialPlayer);

            if (game.Street == Street.PREFLOP)
            {
                var subtable = this.GetSubtable(player.Hand);

                var previousDecision = this.GetPreviousDecision(seat);
                var position = this.GetPosition(game);

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

            i = this.state.Board.Dealer;
            while (positions[i] != 0)
            {
                if (i == 0) return (Position)(positions.Count - 1);

                positions[positions.Count - 1]--;
                if (positions[positions.Count - 1] == 0) positions.RemoveAt(positions.Count - 1);
                i = this.state.Board.RightOf(i);
            }

            return Position.Early;
        }
    }
}
