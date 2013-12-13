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
        public Hand Hand { get; set; }

        public ArtificialPlayer(AbstractStrategy strategy, AbstractOutput output)
        {
            this.strategy = strategy;
            this.output = output;
        }

        public override void Act(int tableId, Activity activity = null)
        {
            var act = this.strategy.Process(this.Seats[tableId]);

            this.History.Add(act);

            this.output.Emulate(act);
        }
    }
}
