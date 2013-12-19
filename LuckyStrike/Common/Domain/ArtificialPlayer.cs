using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class ArtificialPlayer : AbstractPlayer
    {
        private AbstractOutput output;
        private AbstractStrategy strategy;

        public ArtificialPlayer(AbstractStrategy strategy, AbstractOutput output)
        {
            this.strategy = strategy;
            this.output = output;
        }

        public override void Act(NonEmptySeat seat, Activity activity = null)
        {
            if (seat.Table.Street == Street.PREFLOP)
            {
                var act = this.strategy.Process(seat);

                this.output.Emulate(act);
            }
        }
    }
}
