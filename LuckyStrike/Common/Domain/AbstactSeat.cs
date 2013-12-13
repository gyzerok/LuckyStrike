using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class AbstractSeat
    {
        public AbstractSeat Left { get; set; }
        public AbstractSeat Right { get; set; }

        public AbstractSeat(AbstractSeat left, AbstractSeat right)
        {
            this.Left = left;
            this.Right = Right;
        }
    }
}
