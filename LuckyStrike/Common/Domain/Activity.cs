using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Activity
    {
        public Decision Decision { get; private set; }
        public double Bet { get; private set; }

        public Activity(Decision decision, double bet = 0.0)
        {
            this.Decision = decision;
            this.Bet = bet;
        }
    }
}
