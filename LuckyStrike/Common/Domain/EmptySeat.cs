﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class EmptySeat : AbstractSeat
    {
        public EmptySeat(AbstractSeat left, AbstractSeat right)
            : base(left, right)
        {
            
        }
    }
}
