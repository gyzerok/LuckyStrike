using Common.Abstract;

namespace Common.Domain
{
    public class EmptySeat : AbstractSeat
    {
        public EmptySeat(Table table, AbstractSeat left, AbstractSeat right)
            : base(table, left, right)
        {
            
        }
    }
}
