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
            this.Games = new List<Game>();

            AbstractSeat prevSeat = new EmptySeat(this, null, null);
            for (int i = 1; i < size; i++)
            {
                this.Seats.Add(prevSeat);
                prevSeat = new EmptySeat(this, null, prevSeat);
            }
            this.Seats.Add(prevSeat);

            this.Seats[0].Right = prevSeat;

            while (prevSeat.Right.Left == null)
            {
                prevSeat.Right.Left = prevSeat;
                prevSeat = prevSeat.Right;
            }
        }

        public void SeatPlayer(int id, AbstractPlayer player)
        {
            var seat = new NonEmptySeat(this, this.Seats[id].Left, this.Seats[id].Right, player);

            player.Seats.Add(seat);

            this.Seats[id] = seat;
        }
    }
}
