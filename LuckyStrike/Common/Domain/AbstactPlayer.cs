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

        public AbstractPlayer()
        {
            this.Seats = new List<NonEmptySeat>();
        }

        public abstract void Act(Activity activity = null);
    }
}
