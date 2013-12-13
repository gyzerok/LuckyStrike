using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class EmptySeat : AbstractSeat
    {
        public EmptySeat(Table table, AbstractSeat left, AbstractSeat right)
            : base(table, left, right)
        {
            
        }
    }
}
