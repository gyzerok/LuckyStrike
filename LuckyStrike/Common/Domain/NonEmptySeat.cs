using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class NonEmptySeat : AbstractSeat
    {
        public AbstractPlayer Player { get; private set; }

        public NonEmptySeat(AbstractSeat left, AbstractSeat right, AbstractPlayer player)
            : base(left, right)
        {
            this.Player = player;
        }
    }
}
