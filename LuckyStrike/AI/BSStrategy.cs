using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Common.Abstract;
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
            this.LoadConfig();
        }

        private void LoadConfig()
        {
            var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            var sr = new StreamReader(baseDirectory+"/../../../preflop_table.txt");
            preflopTable = new Dictionary<List<string>, List<List<int>>>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var actions = new List<int>();
                var subTable = new List<List<int>>();
                string tmpLine;
                for (int i = 0; i < 3; i++)
                {
                    tmpLine = sr.ReadLine();
                    actions = tmpLine.Split(' ').Select(n => int.Parse(n)).ToList();
                    subTable.Add(actions);
                    this.preflopTable.Add(line.Split(' ').ToList(), subTable);
                }
            }
            sr.Close();
        }

        public override Activity Process(NonEmptySeat seat)
        {
            if (seat.Table.Street == Street.PREFLOP)
            {
                var subtable = this.GetSubtable(seat.Hand);

                if (subtable == null) 
                    return new Activity(Decision.FOLD);

                var previousDecision = this.GetPreviousDecision(seat);
                var position = this.GetPosition(seat);

                var decision = subtable[((int)previousDecision) - 4][(int) position-1];

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
            var previousSeat = seat;

            while (previousSeat.RightActive != seat)
            {
                previousSeat = previousSeat.RightActive;

                if (previousSeat.Activity.Decision == Decision.RAISE)
                {
                    return Decision.RAISE;
                }

                if (previousSeat.Activity.Decision == Decision.CALL)

                if (seat == previousSeat)
                {
                    throw new Exception("Table is empty");
                }
            }

            return Decision.FOLD;
        }

        private Position GetPosition(NonEmptySeat seat)
        {
            if (seat.LeftNonEmpty.Activity.Decision == Decision.BLIND ||
                seat.RightNonEmpty.Activity.Decision == Decision.BLIND)
            {
                return Position.BLIND;
            }

            var positions = this.CalcPositions(seat);

            var checkingSeat = seat;
            if (checkingSeat.Player == seat.Player) return Position.LATE;

            int j = positions.Count - 1;
            while (j > 0)
            {
                if (checkingSeat.Player == seat.Player)
                    return (Position) j + 1;

                positions[j]--;
                if (positions[j] == 0) j--;

                checkingSeat = checkingSeat.RightActive;
            }

            throw new Exception("Position identification fail");
        }

        private List<int> CalcPositions(NonEmptySeat seat)
        {
            var positions = new List<int>();
            positions.Add(2);
            positions.Add(3);
            positions.Add(2);

            var minus = seat.Table.Seats.Count - seat.Table.ActivePlayersCount;
            var i = 0;
            while (minus > 0)
            {
                positions[i]--;
                minus--;

                if (positions[i] == 0) i++;
            }

            return positions;
        }
    }
}
