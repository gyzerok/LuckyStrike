using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class OnlinePlayer : AbstractPlayer
    {
        public double Bet { get; private set; }

        public override void Act(int tableId, Activity activity = null)
        {
            
        }
    }
}
