namespace Input
{
    public class ScreenInputFactory : InputFactory
    {
        public override AbstractData CreateData()
        {
            return new ScreenData(null,null,null,null);
        }

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
