using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Table
    {
        private int id;
        public List<AbstractSeat> Seats { get; private set; }
        public List<Game> Games { get; private set; }

        public Game ActiveGame
        {
            get
            {
                return this.Games.Last();
            }
        }

        public Table(int id, int size)
        {
            this.id = id;
            this.Seats = new List<AbstractSeat>();

            AbstractSeat prevSeat = new EmptySeat(null, null);
            for (int i = 1; i < size - 1; i++)
            {
                this.Seats.Add(prevSeat);
                prevSeat = new EmptySeat(null, prevSeat);
            }

            while (prevSeat.Right.Left == null)
            {
                prevSeat.Right.Left = prevSeat;
                prevSeat = prevSeat.Right;
            }
        }

        public void SeatPlayer(int id, AbstractPlayer player)
        {
            var seat = new NonEmptySeat(this.Seats[id].Left, this.Seats[id].Right, player);

            this.Seats[id] = seat;
        }
    }
}
