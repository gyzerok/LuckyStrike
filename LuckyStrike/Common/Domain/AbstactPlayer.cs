using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class AbstractPlayer
    {
        // TODO make dictionary with table id
        public List<NonEmptySeat> Seats;
        public List<Activity> History { get; private set; }

        public Activity Activity
        {
            get
            {
                return this.History.Last();
            }
        }

        public AbstractPlayer()
        {
            this.Seats = new List<NonEmptySeat>();
        }

        public abstract void Act(int tableId, Activity activity = null);
    }
}
