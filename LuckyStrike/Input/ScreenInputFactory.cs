namespace Input
{
    public class ScreenInputFactory : InputFactory
    {
        public override AbstractGrabber CreateGrubber()
        {
            return new ScreenGrabber();
        }

        public override AbstractInterpreter CreateInterpreter()
        {
            return new ScreenInterpreter();
        }

        public ScreenInputFactory()
        {
            this.CreateGrubber();
            this.CreateInterpreter();
        }
    }
}
