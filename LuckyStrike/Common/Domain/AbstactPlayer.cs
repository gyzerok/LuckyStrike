using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public abstract class AbstractPlayer
    {
        public List<NonEmptySeat> Seats;
        public List<Activity> History { get; private set; }

        public Activity Activity
        {
            get
            {
                this.History.Last();
            }
        }

        public AbstractPlayer()
        {
            this.Seats = new List<NonEmptySeat>();
        }

        public abstract void Act(Activity activity = null);
    }
}
