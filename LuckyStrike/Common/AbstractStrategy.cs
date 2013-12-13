using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;

namespace Common
{
    public abstract class AbstractStrategy
    {
        public abstract Activity Process(NonEmptySeat seat);
    }
}
