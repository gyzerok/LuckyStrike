namespace Common.Domain
{
    public class Activity
    {
        public Decision Decision { get; private set; }
        public double Bet { get; private set; }

        public Activity(Decision decision, double bet = 0.0)
        {
            this.Decision = decision;
            this.Bet = bet;
        }
    }
}
