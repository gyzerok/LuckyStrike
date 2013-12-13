using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Input
{
    public class TestInputFactory : InputFactory 
    {
        public override AbstractData CreateData()
        {
            throw new NotImplementedException();
        }

        public override AbstractGrubber CreateGrubber()
        {
            return new TestGrabber();
        }

        public override AbstractInterpreter CreateInterpreter()
        {
            return new TestInterpreter();
        }
    }
}
