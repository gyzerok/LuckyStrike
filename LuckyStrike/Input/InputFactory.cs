using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Input
{
    public abstract class InputFactory
    {
        public abstract AbstractGrabber CreateGrubber();
        public abstract AbstractInterpreter CreateInterpreter();
    }
}
