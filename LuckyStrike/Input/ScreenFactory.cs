using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Input
{
    public class ScreenFactory : InputFactory
    {
        public override AbstractGrubber CreateGrubber()
        {
            return new ScreenGrubber();
        }

        public override AbstractInterpreter CreateInterpreter()
        {
            return new ScreenInterpreter();
        }
    }
}
