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

        public AbstractPlayer()
        {
            this.Seats = new List<NonEmptySeat>();
        }

        public abstract void Act(NonEmptySeat seat, Activity activity = null);
    }
}
