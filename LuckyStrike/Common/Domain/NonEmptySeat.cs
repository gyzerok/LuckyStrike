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

        public NonEmptySeat LeftNonEmpty
        {
            get
            {
                AbstractSeat checkingSeat = this.Left;
                while (checkingSeat is EmptySeat)
                {
                    checkingSeat = checkingSeat.Left;
                }

                return (NonEmptySeat)checkingSeat;
            }
        }

        public NonEmptySeat RightNonEmpty
        {
            get
            {
                AbstractSeat checkingSeat = this.Right;
                while (checkingSeat is EmptySeat)
                {
                    checkingSeat = checkingSeat.Right;
                }

                return (NonEmptySeat)checkingSeat;
            }
        }

        public NonEmptySeat(Table table, AbstractSeat left, AbstractSeat right, AbstractPlayer player)
            : base(table, left, right)
        {
            this.Player = player;
        }
    }
}
