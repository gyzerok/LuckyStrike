using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class ArtificialPlayer : AbstractPlayer
    {
        private AbstractStrategy strategy;
        public Hand Hand { get; set; }

        public ArtificialPlayer(AbstractStrategy strategy)
        {
            this.strategy = strategy;
        }

        public override void Act(int tableId, Activity activity = null)
        {
            var act = this.strategy.Process(this.Seats[tableId].Table.ActiveGame);

            this.History.Add(act);
        }
    }
}
