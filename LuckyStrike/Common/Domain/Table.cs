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

        public Table(int id)
        {
            this.id = id;
        }
    }
}
