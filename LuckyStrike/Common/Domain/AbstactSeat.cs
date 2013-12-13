using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class AbstractSeat
    {
        public Table Table { get; private set; }
        public AbstractSeat Left { get; set; }
        public AbstractSeat Right { get; set; }

        public AbstractSeat(Table table, AbstractSeat left, AbstractSeat right)
        {
            this.Table = table;
            this.Left = left;
            this.Right = Right;
        }
    }
}
