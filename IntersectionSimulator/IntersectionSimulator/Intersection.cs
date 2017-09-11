using System;
using System.Threading;

namespace IntersectionSimulator
{
    public enum Direction
    {
        None = 0,
        NS = 1,
        EW = 2,
    }

    public class Intersection : IIntersection, IDirectionSwitcher
    {
        // Shows new direction and how long it will be on
        public event Action<Direction, int> DirectionSwitched;
        public Direction CurrentDirection { get; private set; }

        public void Start( )
        {
            _timer.Change( _intervalOfDirection, _intervalOfDirection );
        }

        public void Stop( )
        {
            _timer.Change( Timeout.Infinite, Timeout.Infinite );
        }

        public Intersection( int intervalOfDirection )
        {
            _intervalOfDirection = intervalOfDirection;

            CurrentDirection = Direction.NS;
            _timer = new Timer( SwitchDirection );
        }

        private readonly Timer _timer;
        private readonly int _intervalOfDirection;
        private Direction NextDirection
        {
            get { return CurrentDirection == Direction.NS ? Direction.EW : Direction.NS; }
        }

        private void SwitchDirection( object state )
        {
            CurrentDirection = NextDirection;

            var handler = DirectionSwitched;
            if (handler != null) { handler( CurrentDirection, _intervalOfDirection ); }
        }
    }
}
