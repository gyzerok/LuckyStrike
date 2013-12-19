namespace Common.Domain
{
    public class NonEmptySeat : AbstractSeat
    {
        public AbstractPlayer Player { get; private set; }
        public Hand Hand { get; set; }
        public Activity Activity { get; private set; }

        public NonEmptySeat LeftActive
        {
            get
            {
                AbstractSeat checkingSeat = this.Left;
                while (checkingSeat is EmptySeat || (checkingSeat as NonEmptySeat).Activity.Decision != Decision.FOLD)
                {
                    checkingSeat = checkingSeat.Left;
                }

                return (NonEmptySeat) checkingSeat;
            }
        }

        public NonEmptySeat RightActive
        {
            get
            {
                AbstractSeat checkingSeat = this.Right;
                while (checkingSeat is EmptySeat || (checkingSeat as NonEmptySeat).Activity.Decision != Decision.FOLD)
                {
                    checkingSeat = checkingSeat.Right;
                }

                return (NonEmptySeat)checkingSeat;
            }
        }

        public NonEmptySeat LeftNonEmpty
        {
            get
            {
                AbstractSeat checkingSeat = this.Left;
                while (checkingSeat is EmptySeat)
                {
                    checkingSeat = checkingSeat.Left;
                }

                return (NonEmptySeat)checkingSeat;
            }
        }

        public NonEmptySeat RightNonEmpty
        {
            get
            {
                AbstractSeat checkingSeat = this.Right;
                while (checkingSeat is EmptySeat)
                {
                    checkingSeat = checkingSeat.Right;
                }

                return (NonEmptySeat)checkingSeat;
            }
        }

        public NonEmptySeat(Table table, AbstractSeat left, AbstractSeat right, AbstractPlayer player)
            : base(table, left, right)
        {
            this.Player = player;
        }

        public void Act(Activity activity)
        {
            this.Activity = activity;

            this.Player.Act(this, activity);
        }
    }
}
